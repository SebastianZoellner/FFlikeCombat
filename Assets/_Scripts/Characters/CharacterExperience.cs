using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterExperience : MonoBehaviour
{
    public static event Action<CharacterExperience> OnAnyLevelUp=delegate { };
    public event Action OnLevelUp = delegate { };
    public event Action OnExperienceChanged = delegate { };

    public int level { get; private set; } = 1;
    public float experience { get; private set;}

    private bool canLevelUp = false;
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
        characterCombat.OnAttackFinished += CharacterCombat_OnAttackFinished;
    }

   

    private void OnDisable()
    {
        CharacterHealth.OnAnyEnemyDied -= CharacterHealth_OnAnyEnemyDied;
        characterCombat.OnAttackFinished -= CharacterCombat_OnAttackFinished;
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
            canLevelUp = true;
        }
    }

   
    private void CharacterCombat_OnAttackFinished(bool finnished)
    {
        if (!finnished || !canLevelUp)
            return;
        LevelUp();
    }


    private void LevelUp()
    {
        ++level;
        OnLevelUp.Invoke();
        OnAnyLevelUp.Invoke(this);
        canLevelUp = false;
    }

    [Button]
    private void TestLevel()
    {
        canLevelUp = true;
    }
        
}
