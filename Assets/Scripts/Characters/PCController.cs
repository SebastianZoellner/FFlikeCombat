using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : MonoBehaviour
{
    public event Action OnActionStarted = delegate{};
    public event Action OnActionEnded=delegate { };
    public event Action OnHasActed = delegate { };

    
    [SerializeField] private CharacterHealth target;

    private CharacterCombat combat;
    public CharacterStats stats { get; private set; }

    private bool hasActed;

    

    private void Awake()
    {
        combat = GetComponent<CharacterCombat>();
        stats = GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        combat.OnAttackFinished += Combat_OnAttackFinished;
    }

   
    private void OnDisable()
    {
        combat.OnAttackFinished -= Combat_OnAttackFinished;
    }

    

 public void StartTurn(int turnNumber)
    {
        hasActed = false;
    }

    public void StartAttack(int powerId)
    {
        if (hasActed)
        {
            Debug.Log(stats.GetName() + " has acted");
            return;
        }
        if (!stats.HasPowerID(powerId - 1))
            return; 
        
        OnActionStarted.Invoke();

        combat.Attack(stats.GetPower(powerId - 1), target);
    }



    public void SetTarget(CharacterHealth targetHealth)
    {
        target = targetHealth;
        Debug.Log("Set new target: " + target.name);
    }

    private void Combat_OnAttackFinished(bool success)
    {
        OnActionEnded.Invoke();
        hasActed = success;
        if (success)
            OnHasActed.Invoke();
    }
}
