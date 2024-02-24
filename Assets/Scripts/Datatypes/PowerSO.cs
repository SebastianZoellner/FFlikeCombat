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
    [SerializeField] SuccessEffect[] successEffectArray;

    [Header("UI Presentation")]
    public string buttonName;
    public Sprite icon;

    //this should go under the top header
    public float setupTime;
    public float recoveryTime;
    public float range;

    [Header("Animation")]
    public AnimationClip attackAnimation;

    
   [SerializeField] private Projectile projectile;
    
    [BoxGroup("VFX")]
    public GameObject attackVFX;
    [BoxGroup("VFX")]
    public GameObject hitVFX;

    [BoxGroup("SFX")]
    [SerializeField]private SimpleAudioEventSO attackSound;
    [BoxGroup("SFX")]
    public SimpleAudioEventSO hitSound;
    [BoxGroup("SFX")]
    public SimpleAudioEventSO missSound;

    [TextArea]
    public string description;

    [BoxGroup("Momentum")]
    public bool momentumEffect;
    [BoxGroup("Momentum"),ShowIf("momentumEffect")]
    public float momentumCost=0;
    [BoxGroup("Momentum"), ShowIf("momentumEffect")]
    public float momentumChange=0;
    [BoxGroup("Momentum"), ShowIf("momentumEffect")]
    public float minMomentum=-100;
    [BoxGroup("Momentum"), ShowIf("momentumEffect")]
    public float maxMomentum=100;

    public float enduranceCost;


    public bool HasProjectile()
    {
        if (projectile)
            return true;
        return false;
    }

    public float GetDamage()
    {
        return UnityEngine.Random.Range(minDamage, maxDamage);
    }

    public (StatusName,float,int) GetStatusEffect(int successLevel)
    {
        foreach(SuccessEffect se in successEffectArray)
        {
            if (successLevel == se.level)
                return (se.status, se.intensity,se.duration);
        }
        return (StatusName.None, 0,0);
    }

    public void LaunchProjectile(Vector3 launchPosition, CharacterCombat attacker,CharacterHealth targetHealth, int successLevel)
    {
       GameObject projectileInstance = Instantiate(projectile.gameObject, launchPosition, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().Setup(attacker,targetHealth, range, this, successLevel);
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
