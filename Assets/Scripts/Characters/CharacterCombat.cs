using System;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    
    public event Action <bool> OnAttackFinished=delegate{};

    public  void Attack(PowerSO attackPower, CharacterHealth target)
    {
        if (!attackPower || !target)
        {
            OnAttackFinished.Invoke(false);
            return;
        }
           

        Debug.Log("Attacking with " + attackPower.name);
        target.TakeDamage(attackPower.GetDamage());
        OnAttackFinished.Invoke(true);
    }
}
