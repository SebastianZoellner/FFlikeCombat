using System;
using System.Collections;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public static Action OnAnyActionFinished = delegate { };
    public event Action <bool> OnAttackFinished=delegate{};

    private CharacterStats stats;
    private CharacterMover mover;
    private CharacterVFX effects;
    private float attackTime = 5;

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject missEffect;

    private PowerSO attackPower;
    private CharacterHealth target;
    private int successLevel;

    private Vector3 startingPosition;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        effects = GetComponent<CharacterVFX>();
        mover = GetComponent<CharacterMover>();

        startingPosition = transform.position;
    }
    private void OnEnable()
    {
        mover.OnMovementFinished += Mover_OnMovementFinished;
    }
    
   

    public  void StartAttack(PowerSO attackPower, CharacterHealth target)
    {
        this.attackPower = attackPower;
        this.target = target;

        mover.MoveTo(target.transform.position, attackPower.range);
    }

    private int RollAttack(PowerSO attackPower, CharacterHealth target)
    {
        float attackValue = attackPower.attack;
        attackValue = GameSystem.Instance.CalculateAttack(stats.GetAttribute(Attribute.Combat), attackValue);

        float defenseValue = target.Stats.GetDefenseValue();
        float critModifier = 0;

        stats.SetDefense(attackPower.defense);

        Debug.Log(name + " Attacking " + target.Stats.GetName() + " with " + attackPower.name);
        int successLevel = GameSystem.Instance.TestAttack(attackValue, defenseValue, critModifier);
        return successLevel;
    }

    private void ManageHit(PowerSO attackPower, CharacterHealth target, int successLevel)
    {
        float damage = attackPower.GetDamage();
        damage = GameSystem.Instance.CalculateDamage(stats.GetAttribute(Attribute.Power), damage);
        target.TakeDamage(damage);

        (StatusName status,float intensity,int duration)=attackPower.GetStatusEffect(successLevel);

        if (status != StatusName.None)
            target.GetComponent<StatusManager>().GainStatus(status, intensity,duration);

        effects.AttackingEffect(hitEffect, target.transform);
    }

    private void Mover_OnMovementFinished()
    {
        if ( target && Vector3.Distance(transform.position, target.transform.position) < attackPower.range)
        {
            PerformAttack();
        }
        else
        {
            OnAttackFinished.Invoke(true);
            OnAnyActionFinished.Invoke();
        }
    }

    private void PerformAttack()
    {
        successLevel = RollAttack(attackPower, target);

        if (successLevel > 0)
        {
            Debug.Log("Hit, success level " + successLevel);
            ManageHit(attackPower, target, successLevel);
        }
        else
        {
            Debug.Log("Missed");
            effects.AttackingEffect(missEffect, target.transform);
        }

        mover.MoveHome();
    }

    
}
