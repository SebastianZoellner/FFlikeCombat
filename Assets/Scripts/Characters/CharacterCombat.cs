using System;
using System.Collections;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{ 
    public event Action <bool> OnAttackFinished=delegate{};

    private CharacterStats stats;
    private CharacterVFX effects;
    private float attackTime = 1;

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject missEffect;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        effects = GetComponent<CharacterVFX>();
    }

    public  void Attack(PowerSO attackPower, CharacterHealth target)
    {
        if (!attackPower || !target)
        {
            OnAttackFinished.Invoke(false);
            return;
        }
        float attackValue = attackPower.attack;
        float defenseValue = target.Stats.GetDefenseValue();
        float critModifier = 0;

        stats.SetDefense(attackPower.defense);

        Debug.Log(name+" Attacking "+target.Stats.GetName()+" with " + attackPower.name);
        int successLevel = GameSystem.Instance.TestAttack(attackValue, defenseValue, critModifier);

        if (successLevel> 0)
        {
            Debug.Log("Hit, success level " + successLevel);
            ManageHit(attackPower, target,successLevel);
        }
        else
        {
            Debug.Log("Missed");
            effects.AttackingEffect(missEffect, target.transform);
        }
        StartCoroutine(FinishAttack());
    }

    private void ManageHit(PowerSO attackPower, CharacterHealth target, int successLevel)
    {
        target.TakeDamage(attackPower.GetDamage());
        (StatusName status,float intensity,int duration)=attackPower.GetStatusEffect(successLevel);

        if (status != StatusName.None)
            target.GetComponent<StatusManager>().GainStatus(status, intensity,duration);

        effects.AttackingEffect(hitEffect, target.transform);
    }

    IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(attackTime);
        OnAttackFinished.Invoke(true);
    }
}
