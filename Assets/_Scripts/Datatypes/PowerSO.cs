using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Power", menuName = "Game Elements/Powers/Power")]

public class PowerSO : ScriptableObject
{
    private enum MomentumLimit
    {
        NoLimit,
        Low,
        Typical,
        High
    }

    [SerializeField] bool ShowDerivedAttributes = false;

    [Header("Attack Parameters")]
    
    public float attack;
    public float defense;
    

    [Header("Basic Values")]
    
    public TargetType target;

    private bool hasRadius => (target == TargetType.AreaEnemies);
    [ShowIf(nameof(hasRadius))]
    public float radius;

    [SerializeField] private float timeCost;
    [Range(0, 1)]
    [SerializeField] private float setupVsRecovery=0.5f;
    [ShowInInspector, ShowIf("ShowDerivedAttributes")] private float setupTime;
    [ShowInInspector, ShowIf("ShowDerivedAttributes")] private float recoveryTime;

    [Range(0,1)]
    [SerializeField] private float damageSpread;
    [SerializeField] private int numberOfHits = 1;

    [Header("Momentum Effects")]
    [SerializeField] private MomentumLimit momentumLimit=MomentumLimit.NoLimit;
    [ShowInInspector,ShowIf("ShowDerivedAttributes")] private float minMomentum = -100;
    [ShowInInspector, ShowIf("ShowDerivedAttributes")] private float maxMomentum = 100;
     
    public bool momentumEffect;
    [ShowIf("momentumEffect")]
    [Range(0, 5)]
    [SerializeField] float momentumCostFactor = 0;

    private bool hasMomEffs => (momentumEffect && ShowDerivedAttributes);
    [ShowIf(nameof(hasMomEffs))]
    [ShowInInspector] float momentumCost = 0;
    [ShowIf(nameof(hasMomEffs))]
    [ShowInInspector] float momentumChange = 0;

    [SerializeField] SuccessEffect[] successEffectArray;

    [SerializeField] AttackSuccessEffectSO[] attackEffectsLevel1;
    [SerializeField] AttackSuccessEffectSO[] attackEffectsLevel2;
    [SerializeField] AttackSuccessEffectSO[] attackEffectsLevel3;
    [SerializeField] AttackSuccessEffectSO[] attackEffectsLevel4;


    [ShowInInspector, ShowIf("ShowDerivedAttributes")] private float baseDamage;
    [ShowInInspector, ShowIf("ShowDerivedAttributes")] float minDamage;
    [ShowInInspector, ShowIf("ShowDerivedAttributes")] float maxDamage;

    [Range(-1, 1)]
    [SerializeField] float enduranceToDamage = 0;
    [ShowInInspector, ShowIf("ShowDerivedAttributes")] float enduranceCost;

    [Button]
    private void CalculateDerivedValues()
    {
        isInitialized = false;
        InitializePower();
    }


    // [Header("UI Presentation")]
    //[BoxGroup("UI Presentation")]

    [HorizontalGroup("Display", 100)]
    [PreviewField(75)]
    [HideLabel]
    public Sprite icon;
    // [BoxGroup("UI Presentation")]
    [VerticalGroup("Display/Text")]
    public string buttonName;
    // [BoxGroup("UI Presentation")]
    [VerticalGroup("Display/Text")]
    [TextArea]
    public string description;

    [BoxGroup("Animation")]
    public float range = 2;
    [BoxGroup("Animation")] public AnimationClip attackAnimation;
    [BoxGroup("Animation")] public AnimationClip flyingAnimation;


    [SerializeField] private Projectile projectile;

    [BoxGroup("VFX")]
    public GameObject attackVFX;
    [BoxGroup("VFX")]
    public GameObject hitVFX;
    [BoxGroup("VFX")]
    public TrailOrigin[] attackOriginArray;
    [BoxGroup("SFX")]
    [SerializeField] private SimpleAudioEventSO attackSound;
    [BoxGroup("SFX")]
    public SimpleAudioEventSO hitSound;
    [BoxGroup("SFX")]
    public SimpleAudioEventSO missSound;

    private bool isInitialized = false;

    public float GetMomentumChange() => momentumChange;
    public float GetMomentumCost() => momentumCost;
    public float GetSetupTime() => setupTime;
    public float GetRecoveryTime() => recoveryTime;
    public float GetEnduranceCost()
    {
        InitializePower();
        return enduranceCost;
    }

    public bool HasProjectile()
    {
        if (projectile)
            return true;
        return false;
    }

    public bool IsAvailable(float momentum, float endurance)
    {
        InitializePower();
        return momentum >= minMomentum && momentum < maxMomentum && endurance >= enduranceCost;
    }

    public float GetDamage(float highHitModifier)
    {
        InitializePower();

        return UnityEngine.Random.Range(minDamage, maxDamage) * highHitModifier;
    }

    

    public AttackSuccessEffectSO[] GetStatusEffects(int successLevel)
    {
        switch (successLevel)
        {
            case 1:
                if (attackEffectsLevel1.Length > 0)
                    return attackEffectsLevel1;
                break;
            case 2:
                if (attackEffectsLevel3.Length > 0)
                    return attackEffectsLevel2;
                break;
            case 3:
                if (attackEffectsLevel3.Length > 0)
                    return attackEffectsLevel3;
                break;
            case 4:
                if (attackEffectsLevel4.Length > 0)
                    return attackEffectsLevel4;
                break;
        }

        return null;
    }

    public void LaunchProjectile(Vector3 launchPosition, CharacterCombat attacker, IDamageable targetHealth)
    {
        //Debug.Log("Launching projectile " + projectile.name+" at "+targetHealth.transform.position);
        GameObject projectileInstance = Instantiate(projectile.gameObject, launchPosition, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().Setup(attacker, targetHealth, range, this);
    }

    public void PlayAttackSound(AudioSource source)
    {
        if (!attackSound) return;

        attackSound.Play(source);
    }

    /* public void PlayHitSound(AudioSource source)
     {
         if (!hitSound) return;

         hitSound.Play(source);
     }
    */
    
    private void InitializePower()
    {
        if (isInitialized)
            return;
        CalculateInitiative();
        CalculateBaseDamage();
        InititializeMomentum();
        InitializeEndurance();
        isInitialized = true;
    }

    private void InitializeEndurance()
    {
        float baseEnduranceCost = 50;

        enduranceCost = timeCost * baseEnduranceCost * (1 + enduranceToDamage);
    }

    private void CalculateInitiative()
    {
        setupTime = timeCost * setupVsRecovery;
        recoveryTime = timeCost * (1 - setupVsRecovery);
    }

    private void CalculateBaseDamage()
    {
        float momentumCostEffect = 0.2f;

        baseDamage = timeCost * GameSystem.Instance.GetBaseDamage()/numberOfHits;       //base damage depends on the time cost
       // Debug.Log("Base damage " + baseDamage.ToString());
        baseDamage += baseDamage * momentumCostFactor *momentumCostEffect;              //momentum effects change damage
        baseDamage -= CalculateEffectCost();                                            //special effects reduce damage
       // Debug.Log("Base damage " + baseDamage.ToString());
        float attackBonusEffect=1;
        float defenseBonusEffect = 0.3f;
        baseDamage *= (1 -attackBonusEffect* attack/100);
        baseDamage *=  (1 - defenseBonusEffect *defense / 100);
       // Debug.Log("Base damage " + baseDamage.ToString());
        float speedEffect = 0.3f;

       baseDamage*= 1+(setupVsRecovery - 0.5f) * speedEffect;                           //faster powers loose up to 15% damage

        float enduranceEffect=0.5f;

        baseDamage *= 1 + (enduranceEffect * enduranceToDamage);                        //endurance costs reduce or increase the damage

       // Debug.Log("Base damage " + baseDamage.ToString());
        minDamage = (1 - damageSpread) * baseDamage;
        maxDamage = 2 * baseDamage - minDamage;
    }

    private float CalculateEffectCost()
    {
        float stageWeight = 0.25f;//This is assuming each success level hasthesame probabillity

        float cost = 0;

        foreach (AttackSuccessEffectSO ase in attackEffectsLevel1)
            cost += ase.damageCost;
        foreach (AttackSuccessEffectSO ase in attackEffectsLevel2)
            cost += ase.damageCost;
        foreach (AttackSuccessEffectSO ase in attackEffectsLevel3)
            cost += ase.damageCost;
        foreach (AttackSuccessEffectSO ase in attackEffectsLevel4)
            cost += ase.damageCost;

        return cost * stageWeight;
    }

    private void InititializeMomentum()
    {
        CalculateMomentumCost();

        minMomentum = -100;
        maxMomentum = 100;

        switch (momentumLimit)
        {
            case MomentumLimit.NoLimit:
                break;
            case MomentumLimit.Low:
                maxMomentum = -30;
                break;
            case MomentumLimit.Typical:
                minMomentum = -30;
                maxMomentum = 30;
                break;
            case MomentumLimit.High:
                minMomentum = 30;
                break;
        }
    }

    private void CalculateMomentumCost()
    {
        momentumChange = 0;
        momentumCost = 0;
        if (!momentumEffect)
            return;     
            momentumChange = (1 - momentumCostFactor) * timeCost;
         
    }
}


[Serializable]
public struct SuccessEffect
{
    public int level;
    public StatusName status;
    public float intensity;
    public int duration;

}

public enum TargetType
{
    Enemy,
    AllEnemies,
    Self,
    Friend,
    AllFriends,
    AreaEnemies
}
