
using System;
using UnityEngine;

public class HealthGameSystem : MonoBehaviour
{
    public static HealthGameSystem Instance;
    private void Awake()
    {
        Instance = this;       
    }
    /// <summary>
    /// Amount of endurence recovered when advancing stages
    /// </summary>
    public float NewStageEndurance(float startingEndurance) => startingEndurance / 4.0f; //

    /// <summary>
    /// Amount of damage healed when advancing stages
    /// </summary>  
    public float NewStageHeal(float damageTaken) => damageTaken / 3.0f;

    public float EnduranceAfterRaise(float startingEndurance) => startingEndurance / 2.0f;

    public float HealthAfterRaise(float startingHealth) => startingHealth / 2.0f;

    /// <summary>
    /// Endurance bonus from taking the Rest action
    /// </summary>
    public float RestEnduranceBonus(float startingEndurance) => startingEndurance / 2.0f;

    public float RestHealthBonus(float startingHealth, float hardiness) => hardiness;

    /// <summary>
    /// Damage taken above this threshold counts as a Heavy Hit
    /// </summary>
    public float HeavyHitThreshold(float startingHealth) => startingHealth / 2.0f;

    public float NewRoundEnduranceBonus(float startingEndurance) => startingEndurance / 8.0f;
    public float NewRoundHealthBonus(float startingHealth, float hardiness) => hardiness;


}
