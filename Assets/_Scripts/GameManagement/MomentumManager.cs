using System;
using UnityEngine;

public class MomentumManager : MonoBehaviour
{
    public static Action<float> OnMomentumChanged = delegate { };
    public Action OnMomentumLoss = delegate { };
    public Action OnMomentumWin = delegate { };

    [SerializeField] MomentumGameSystem gameSystem;

    private static float momentum = 0;
    private const float lossMomentum = -100;
    private const float winMomentum = 100;
    

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
        int level = combat.GetComponent<CharacterStats>().GetRank();

        change = gameSystem.AttackMomentumChange(change, level);

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
        float momentumEffect = gameSystem.HeavyHitMomentum(health.Stats.GetRank());

        if (health.GetComponent<PCController>())
            ModifyMomentum(momentumEffect);
        else
            ModifyMomentum(-momentumEffect);
    }

    private void HeroDied(CharacterHealth hero)
    {
        ModifyMomentum(-gameSystem.DeathMomentum(hero.Stats.GetRank()));
    }

    private void EnemyDied(CharacterHealth enemy, CharacterCombat ignored)
    {
        ModifyMomentum(gameSystem.DeathMomentum(enemy.Stats.GetRank()));
    }

    private void StatusManager_OnChangeMomentum(float change)
    {
        ModifyMomentum(change);
    }


    private void ModifyMomentum(float change)
    {
        //Debug.Log("Momentum changes by " + change.ToString());
        momentum += change;
        OnMomentumChanged.Invoke(momentum);
        if (momentum <= lossMomentum)
            OnMomentumLoss.Invoke();

        if (momentum >= winMomentum)
            OnMomentumWin.Invoke();
    }
}
