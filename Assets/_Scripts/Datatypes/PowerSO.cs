using System;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Power", menuName = "Game Elements/Powers")]

public class PowerSO : ScriptableObject
{
    [Header("Attack Parameters")]
    [SerializeField] float minDamage;
    [SerializeField] float maxDamage;
    public float attack;
    public float defense;
    public float setupTime;
    public float recoveryTime;
    public TargetType target;
    public float radius;


    [SerializeField] SuccessEffect[] successEffectArray;

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

    [Header("Animation")]
    public float range = 2;
    public AnimationClip attackAnimation;
    public AnimationClip flyingAnimation;


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



    [BoxGroup("Momentum")]
    public bool momentumEffect;
    [BoxGroup("Momentum"), ShowIf("momentumEffect")]
    public float momentumCost = 0;
    [BoxGroup("Momentum"), ShowIf("momentumEffect")]
    public float momentumChange = 0;
    [BoxGroup("Momentum"), ShowIf("momentumEffect")]
    public float minMomentum = -100;
    [BoxGroup("Momentum"), ShowIf("momentumEffect")]
    public float maxMomentum = 100;

    public float enduranceCost;


    public bool HasProjectile()
    {
        if (projectile)
            return true;
        return false;
    }

    public float GetDamage(float highHitModifier)
    {
        return UnityEngine.Random.Range(minDamage, maxDamage) * highHitModifier;
    }

    public (StatusName, float, int) GetStatusEffect(int successLevel)
    {
        foreach (SuccessEffect se in successEffectArray)
        {
            if (successLevel == se.level)
                return (se.status, se.intensity, se.duration);
        }
        return (StatusName.None, 0, 0);
    }

    public void LaunchProjectile(Vector3 launchPosition, CharacterCombat attacker, CharacterHealth targetHealth)
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
