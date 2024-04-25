using System;
using UnityEngine;

public class CharacterInitiative : MonoBehaviour
{
    public static event Action<bool,CharacterInitiative>  OnAttackReadied = delegate { };
    //This updates the timeline UI, removes the buttons and lets Actionsequencer know that the player has chosen an action
    public static event Action<CharacterInitiative,float> OnMomentumCostPayed = delegate { };

    public static event Action OnActionTimeChanged = delegate { };
    
    public event Action OnActionStarted = delegate { };

    [field:SerializeField] public float nextActionTime { get; private set; }
    public PowerSO readiedAction;
    private CharacterHealth targetHealth;
    private CharacterStats stats;
    private CharacterCombat combat;
    private CharacterHealth health;
    private float noiseRange=0.1f;

    private bool isReadying = false;
    
    private void Awake()
    {
        combat = GetComponent<CharacterCombat>();
        stats = GetComponent<CharacterStats>();
        health = GetComponent<CharacterHealth>();
    }

    public void InitializeInitiative(float actionTime)
    {       
        nextActionTime =actionTime+GameSystem.Instance.CalculateReadytime(
            GetComponent<CharacterStats>().GetAttribute(Attribute.Initiative),
            UnityEngine.Random.Range(0f, 1f)           
            );
        //Debug.Log(name + " First action: " + nextActionTime);
    }

    public bool ReadyAttack(PowerSO readiedAction, CharacterHealth targetHealth)
    {
        if (!readiedAction || !targetHealth)
            return false;

        this.readiedAction = readiedAction;
        this.targetHealth = targetHealth;

        if (readiedAction.momentumEffect)
            OnMomentumCostPayed.Invoke(this,readiedAction.momentumCost);

        nextActionTime += GameSystem.Instance.CalculateReadytime(
            stats.GetAttribute(Attribute.Initiative),
            readiedAction.setupTime
            );

       // Debug.Log(name + " NextActionTime changed to " + nextActionTime);

        stats.SetDefense(readiedAction.defense);

        OnAttackReadied.Invoke(true,this);
        return true;
    }

    public bool PerformReadiedAction()
        //rv indicates if the action has been successfully initialized
    { 
        if(!targetHealth||!targetHealth.canBeTarget)
        {
            nextActionTime += 0.01f;
            readiedAction = null;
            targetHealth = null;
            return false;
        }
        OnActionStarted.Invoke(); //Tells the status manager

        if (!health.canBeTarget)
        {
            return false;
        }

        nextActionTime += GameSystem.Instance.CalculateWaitTime(
            stats.GetAttribute(Attribute.Speed),
            readiedAction.recoveryTime+GetRandomNoise()
            );

        OnAttackReadied.Invoke(false, this);
        

       // Debug.Log(name + " Action started; NextActionTime changed to " + nextActionTime);

       
       
        combat.StartAttack(readiedAction, targetHealth);     
        readiedAction = null;
        targetHealth = null;
        return true;
    }

    public void SetNextActionTime(float time)
    {
        nextActionTime = time;
    }

    public void ChangeActionTime(float delta)
    {
        nextActionTime += delta;
        if (nextActionTime < ActionSequencer.actionTime)
            nextActionTime = ActionSequencer.actionTime;

        OnActionTimeChanged.Invoke();
    }

    public string GetTimelineTip()
    {
        string tip=stats.GetName();

        if (readiedAction)
        {
            tip += "\nReadied Attack: " + readiedAction.name;
            tip += "\nTarget:         " + targetHealth.Stats.GetName();
        }
        return tip;
    }

    private float GetRandomNoise()
    {
        return UnityEngine.Random.Range(-noiseRange, noiseRange);
    }
}
