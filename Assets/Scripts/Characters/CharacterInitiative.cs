using System;
using UnityEngine;

public class CharacterInitiative : MonoBehaviour
{
    public static event Action<bool,CharacterInitiative>  OnAttackReadied = delegate { };

    

    [field:SerializeField] public float nextActionTime { get; private set; }
    public PowerSO readiedAction;
    private CharacterHealth targetHealth;
    private CharacterStats stats;
    private CharacterCombat combat;
    private float noiseRange=0.1f;

    private bool isReadying = false;
    
    private void Awake()
    {
        combat = GetComponent<CharacterCombat>();
        stats = GetComponent<CharacterStats>();
    }

    public void InitializeInitiative()
    {
        nextActionTime =GameSystem.Instance.CalculateReadytime(
            stats.GetAttribute(Attribute.Initiative),
            UnityEngine.Random.Range(0f, 1f)           
            );
    }

    public bool ReadyAttack(PowerSO readiedAction, CharacterHealth targetHealth)
    {
        if (!readiedAction || !targetHealth)
            return false;

        this.readiedAction = readiedAction;
        this.targetHealth = targetHealth;
        nextActionTime += GameSystem.Instance.CalculateReadytime(
            stats.GetAttribute(Attribute.Initiative),
            readiedAction.setupTime
            );

        Debug.Log(name + " NextActionTime changed to " + nextActionTime);
        
        OnAttackReadied.Invoke(true,this);
        return true;
    }

    public void PerformReadiedAction()
    { 
        nextActionTime += GameSystem.Instance.CalculateWaitTime(
            stats.GetAttribute(Attribute.Speed),
            readiedAction.recoveryTime+GetRandomNoise()
            );

        OnAttackReadied.Invoke(false, this);
        Debug.Log(name + " NextActionTime changed to " + nextActionTime);
        combat.StartAttack(readiedAction, targetHealth);     
        readiedAction = null;
    }

    private float GetRandomNoise()
    {
        return UnityEngine.Random.Range(-noiseRange, noiseRange);
    }
}
