using System.Collections;
using UnityEngine;

public class PCController : MonoBehaviour
{
    public CharacterStats stats { get; private set; }

    private CharacterInitiative initiative;
    private CharacterHealth health;
    private SelectionIndicator selectionIndicator;

    private IDamageable target;
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
    public IDamageable GetTarget() => target;

    //---------------------------------------------
    //      Public Methods
    //----------------------------------------------

    public void SetSelected()
    {
        selectedPower = null;
        target = null;
        if (target != null)
            target.selectionIndicator.SetSelected();

        selectionIndicator.SetSelected();
    }

    public void SetDeselected()
    {
        selectedPower = null;

        if (target != null)
            target.selectionIndicator.SetDeselected();

        target = null;

        selectionIndicator.SetDeselected();
    }

    public void SetPower(PowerSO power)
    {
        selectedPower = power;

        if (target != null && selectedPower.target == TargetType.Enemy && target.IsHero)
        {
            target.selectionIndicator.SetDeselected();
            target = null;
        }

        if (target != null && selectedPower.target == TargetType.Friend && !target.IsHero)
        {
            target.selectionIndicator.SetDeselected();
            target = null;
        }

        if (selectedPower.target == TargetType.Self)
        {
            target = health;
        }
        //Debug.Log("Set new power: " + selectedPower.name);
        if (target != null)
            StartPower();
    }


    public void SetTarget(IDamageable targetHealth)
    {
        if (targetHealth == null)
            return;

        if (!targetHealth.canBeTarget)
            return;

        if (selectedPower)
        {
            if (selectedPower.target == TargetType.Enemy && targetHealth.IsHero)
                return;
            if (selectedPower.target == TargetType.Friend && !targetHealth.IsHero)
                return;
        }


        if (target != null)
            target.selectionIndicator.SetDeselected();

        target = targetHealth;

        target.selectionIndicator.SetSelected();
        //Debug.Log("Set new target: " + target.GetTransform().name);

        if (target != null && selectedPower)
           StartCoroutine(DelayedStartPower());
    }

    //---------------------------------------------
    //      Private Methods
    //---------------------------------------------

    private void OnAnyEnemyDied(IDamageable deadHealth, CharacterCombat ignored)
    {
        if (deadHealth == target)
        {
            target = null;
            deadHealth.selectionIndicator.SetDeselected();
        }
    }


    private void StartPower()
    {
        if (selectedPower.target == TargetType.Enemy)
            if (target == null || target.IsHero)
            {
                Debug.LogWarning("Target for single enemy power not set or not enemy");
                return;
            }

        if (selectedPower.target == TargetType.Friend)
            if (target == null || !target.IsHero)
            {
                Debug.LogWarning("Target for single hero power not set or not hero");
                return;
            }


        if (initiative.ReadyAttack(selectedPower, target))
            SetDeselected();
    }
    private IEnumerator DelayedStartPower()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        StartPower();
    }
}