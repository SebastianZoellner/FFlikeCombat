using System;
using UnityEngine;

public class PCController : MonoBehaviour
{
    public event Action OnActionStarted = delegate { };
    public event Action OnActionEnded = delegate { };
    public event Action OnHasActed = delegate { };

    public CharacterStats stats { get; private set; }
     
    private CharacterInitiative initiative;
    private CharacterHealth health;
    private SelectionIndicator selectionIndicator;

    private CharacterHealth target;
    private PowerSO selectedPower;

    //---------------------------------------------
    //      Lifecycle Functions
    //-----------------------------------------------

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        initiative = GetComponent<CharacterInitiative>();
        health = GetComponent<CharacterHealth>();
        selectionIndicator = GetComponent<SelectionIndicator>();
    }

    private void OnEnable()
    {
        CharacterHealth.OnAnyEnemyDied += OnAnyEnemyDied;
    }
    private void OnDisable()
    {
        CharacterHealth.OnAnyEnemyDied -= OnAnyEnemyDied;
    }

    //---------------------------------------------
    //      Basic Getters
    //-----------------------------------------------

    public string GetName() => stats.GetName();

    //---------------------------------------------
    //      Public Methods
    //----------------------------------------------
    
    public void SetSelected()
    {
        selectedPower = null;
        target = null;
        if (target)
            target.selectionIndicator.SetSelected();

        selectionIndicator.SetSelected();
    }

    public void SetDeselected()
    {
        if (target)
            target.selectionIndicator.SetDeselected();

        selectionIndicator.SetDeselected();
    }

    public void SetPower(PowerSO power)
    {
        selectedPower = power;

        if (target && selectedPower.target == TargetType.Enemy && target.IsHero)
        {
            target.selectionIndicator.SetDeselected();
            target = null;
        }

        if (target && selectedPower.target == TargetType.Friend && !target.IsHero)
        {
            target.selectionIndicator.SetDeselected();
            target = null;
        }

        if (selectedPower.target == TargetType.Self)
        {
            target = health;          
        }

        if(target!=null)
            StartPower();
    }

    
    public void SetTarget(CharacterHealth targetHealth)
    {
        if (!targetHealth)
            return;

        if (!targetHealth.canBeTarget)
            return;

        if(selectedPower)
        {
            if (selectedPower.target == TargetType.Enemy && targetHealth.IsHero)
                return;
            if (selectedPower.target == TargetType.Friend && !targetHealth.IsHero)
                return;
        }


        if (target)
            target.selectionIndicator.SetDeselected();

        target = targetHealth;
        
        target.selectionIndicator.SetSelected();
        //Debug.Log("Set new target: " + target.name);

        if (target && selectedPower)
            StartPower();
    }


    private void StartPower()
    {
        if (selectedPower.target == TargetType.Enemy)
            if (!target || target.IsHero)
            {
                Debug.LogWarning("Target for single enemy power not set or not enemy");
                return;
            }

        if (selectedPower.target == TargetType.Friend)
            if (!target || !target.IsHero)
            {
                Debug.LogWarning("Target for single hero power not set or not hero");
                return;
            }

        initiative.ReadyAttack(selectedPower, target);
    }

    //---------------------------------------------
    //      Private Methods
    //---------------------------------------------

    private void OnAnyEnemyDied(CharacterHealth deadHealth)
    {
        if (deadHealth == target)
        {
            target = null;
            deadHealth.selectionIndicator.SetDeselected();
        }
    }

}
