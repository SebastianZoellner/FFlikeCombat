using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumManager : MonoBehaviour
{
    public static Action<float> OnMomentumChanged = delegate { };
    public Action OnMomentumLoss = delegate { };
    public Action OnMomentumWin = delegate { };


    private static float momentum = 20;
    private const float lossMomentum = -100;
    private const float winMomentum = 100;
    private const float deathMultiplier= 4;
    private const float heavyHitMultiplier = 1;

   
    

    private void OnEnable()
    {
        CharacterHealth.OnAnyEnemyDied += EnemyDied;
        CharacterHealth.OnAnyPCDied += HeroDied;
        CharacterHealth.OnHeavyHit += CharacterHealth_OnHeavyHit;
        CharacterCombat.OnMomentumModified += CharacterCombat_OnMomentumModified;
        CharacterInitiative.OnMomentumCostPayed += CharacterInitiative_OnMomentumCostPayed;
        StatusManager.OnChangeMomentum += StatusManager_OnChangeMomentum;       
    }

   

    private void Start()
    {
        OnMomentumChanged.Invoke(momentum);
    }

    private void OnDisable()
    {
        CharacterHealth.OnAnyEnemyDied -= EnemyDied;
        CharacterHealth.OnAnyPCDied -= HeroDied;
        CharacterCombat.OnMomentumModified -= CharacterCombat_OnMomentumModified; 
        CharacterInitiative.OnMomentumCostPayed -= CharacterInitiative_OnMomentumCostPayed;
        StatusManager.OnChangeMomentum -= StatusManager_OnChangeMomentum;       
    }

    public static float GetMomentum() => momentum;

    public bool PayMomentum(float cost)
    {
        if (momentum - cost <= lossMomentum)
            return false;

        ModifyMomentum(-cost);
        return true;
    }


    private void CharacterCombat_OnMomentumModified(CharacterCombat combat, float change)
    {
        if (combat.GetComponent<PCController>())
            ModifyMomentum(change);
        else
            ModifyMomentum(-change);
    }

    private void CharacterInitiative_OnMomentumCostPayed(CharacterInitiative init, float change)
    {
        if (init.GetComponent<PCController>())
            ModifyMomentum(change);
        else
            ModifyMomentum(-change);
    }
    private void CharacterHealth_OnHeavyHit(CharacterHealth health)
    {
        if (health.GetComponent<PCController>())
            ModifyMomentum(health.Stats.GetLevel()*heavyHitMultiplier);
        else
            ModifyMomentum(-health.Stats.GetLevel() * heavyHitMultiplier);
    }

    private void HeroDied(CharacterHealth hero)
    {
        ModifyMomentum(-hero.Stats.GetLevel() * deathMultiplier);
    }

    private void EnemyDied(CharacterHealth enemy)
    {
        ModifyMomentum(enemy.Stats.GetLevel() * deathMultiplier);
    }

    private void StatusManager_OnChangeMomentum(float change)
    {
        ModifyMomentum(change);
    }

   


    private void ModifyMomentum(float change)
    {
        momentum += change;
        OnMomentumChanged.Invoke(momentum);
        if (momentum <= lossMomentum)
            OnMomentumLoss.Invoke();

        if (momentum >= winMomentum)
            OnMomentumWin.Invoke();
    }
}
