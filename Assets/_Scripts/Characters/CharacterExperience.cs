using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterExperience : MonoBehaviour
{
    public event Action OnLevelUp = delegate { };
    public event Action OnExperienceChanged = delegate { };

    public int level { get; private set; } = 1;
    public float experience { get; private set;}
    private CharacterCombat characterCombat;
    private CharacterStats characterStats;

    private void Awake()
    {
        characterCombat = GetComponent<CharacterCombat>();
        characterStats = GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        CharacterHealth.OnAnyEnemyDied += CharacterHealth_OnAnyEnemyDied;
    }

    private void OnDisable()
    {
        CharacterHealth.OnAnyEnemyDied -= CharacterHealth_OnAnyEnemyDied;
    }

    public float GetLevelUpCost() => GameSystem.Instance.GetLevelUpCost(level, characterStats.GetRank());

    private void CharacterHealth_OnAnyEnemyDied(CharacterHealth defeatedEnemy, CharacterCombat damageSource)
    {
        if (damageSource != characterCombat)
            return;

        CharacterStats stats = defeatedEnemy.GetComponent<CharacterStats>();

        if (!stats)
            return;

        GainExperience(stats.GetExperienceValue());
    }

    public void GainExperience(float gain)
    {
        if (gain <= 0)
            return;

        experience += gain;
        OnExperienceChanged.Invoke();

        if (GameSystem.Instance.TestLevelUp(experience, level,characterStats.GetRank()))
        {
            ++level;
            OnLevelUp.Invoke();
        }
    }

    [Button]
    private void TestLevel()
    {
        ++level;
        OnLevelUp.Invoke();
    }
        
}
