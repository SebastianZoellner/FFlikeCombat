using System;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public static Action OnAnyActionFinished = delegate { };
    public event Action <bool> OnAttackFinished=delegate{};
    public static event Action <CharacterCombat,float> OnMomentumModified=delegate{};

    private CharacterStats stats;
    private CharacterMover mover;
    private CharacterVFX effects;
    private CharacterAnimator animator;
    private CharacterAudio sound;
    

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject missEffect;
    [SerializeField] Transform attackOrigin;

    private PowerSO attackPower;
    private CharacterHealth target;
    private int successLevel;

    

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        effects = GetComponent<CharacterVFX>();
        mover = GetComponent<CharacterMover>();
        animator = GetComponent<CharacterAnimator>();
        sound = GetComponent<CharacterAudio>();
    }

    private void OnEnable()
    {
        mover.OnMovementFinished += Mover_OnMovementFinished;
        animator.OnAttackAnimationFinished += Animator_OnAttackAnimationFinished;
    }

    public  void StartAttack(PowerSO attackPower, CharacterHealth target)
    {
        
        this.attackPower = attackPower;
        this.target = target;
        Debug.Log("Starting attack");
        animator.SetMove(true);
        mover.MoveTo(target.transform.position, attackPower.range);
    }

    private void RollAttack(PowerSO attackPower, CharacterHealth target)
    {
        float attackValue = attackPower.attack;
        attackValue = GameSystem.Instance.CalculateAttack(stats.GetAttribute(Attribute.Combat), attackValue);

        float defenseValue = target.Stats.GetDefenseValue();
        float critModifier = 0;

        Debug.Log(name + " Attacking " + target.Stats.GetName() + " with " + attackPower.name);
        successLevel = GameSystem.Instance.TestAttack(attackValue, defenseValue, critModifier);
        
    }

    public void ManageHit()
        //This manages all the rules effects of a hit 
    {
        float damage = attackPower.GetDamage();
        damage = GameSystem.Instance.CalculateDamage(stats.GetAttribute(Attribute.Power), damage);
        target.TakeDamage(damage);

        (StatusName status,float intensity,int duration)=attackPower.GetStatusEffect(successLevel);

        if (status != StatusName.None)
            target.GetComponent<StatusManager>().GainStatus(status, intensity,duration);

       if(attackPower.momentumEffect)
        {
            OnMomentumModified.Invoke(this,attackPower.momentumChange);
        }
    }

    private void Mover_OnMovementFinished()
    {
        
        if ( target && Vector3.Distance(transform.position, target.transform.position) < attackPower.range)
        {          
            PerformAttack();
        }
        else
        {          
            animator.SetMove(false);
            OnAttackFinished.Invoke(true);
            OnAnyActionFinished.Invoke();
        }
    }

    private void Animator_OnAttackAnimationFinished()
    {
        mover.MoveHome();
    }

    private void PerformAttack()
    {
        //Debug.Log("Starting attack animation");
        animator.SetAttack(attackPower.attackAnimation);
        //start beginning attack FX

        sound.PlayAttackSound(attackPower);
        RollAttack(attackPower, target);
    }

    private void Impact() 
        //This manages all the FX of the hit and the lauch of projectiles
    {
        if (attackPower.HasProjectile())
        {
            //Debug.Log("Has a projectile");
            if (attackPower.hitVFX)
            {
                GameObject muzzleVFX = Instantiate(attackPower.hitVFX, attackOrigin.position, Quaternion.Euler(attackOrigin.forward));
                muzzleVFX.transform.forward = gameObject.transform.forward;
                var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
                if (psMuzzle != null)
                {
                    Destroy(muzzleVFX, psMuzzle.main.duration);
                }
                else
                {
                    var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(muzzleVFX, psChild.main.duration);
                }
            }

            sound.PlayShootSound(attackPower);

            attackPower.LaunchProjectile(attackOrigin.position, this, target,successLevel);
            return;
        }

       // Debug.Log("No projectile");
        if (successLevel > 0)
        {
            Debug.Log("Hit, success level " + successLevel);
            if(attackPower.hitVFX)
            effects.AttackingEffect(attackPower.hitVFX, target.transform);

            sound.SetHitSound(attackPower,target);

            ManageHit();
        }
        else
        {
            Debug.Log("Missed");
            effects.AttackingEffect(missEffect, target.transform);
        }     
    }

    
}
