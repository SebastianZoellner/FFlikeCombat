using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    public static Action OnAnyActionFinished = delegate { };
    public event Action <bool> OnAttackFinished=delegate{};
    public static event Action <CharacterCombat,float> OnMomentumModified=delegate{};
    public static event Action<CharacterHealth, CharacterHealth,PowerSO> OnAnyAttackStarted = delegate { };//Updates avatar and calls feelmanager
    public static event Action OnAnyAttackEnded = delegate { };//used by feelmanager
    public static event Action OnAnyPowerHit = delegate { };//used by feel manager


    public bool Initialized { get; private set; } = false;

    private CharacterStats stats;
    private CharacterMover mover;
    private CharacterVFX effects;
    private CharacterAnimator animator;
    private CharacterAudio sound;
    private  CharacterHealth health;
    

    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject missEffect;
    [SerializeField] Transform attackOrigin;

    private PowerSO attackPower;
    //private CharacterHealth target;
    private IDamageable[] targetArray;
    //private int successLevel;
    private bool hasActed;
    public bool IsHero { get; private set; }

   // private float highHitDamage=1; //Damage modifier for hit chance >100%

//-------------------------------------------------------------
//                 Lifecycle Functions
//-------------------------------------------------------------

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        effects = GetComponent<CharacterVFX>();
        mover = GetComponent<CharacterMover>();
        animator = GetComponent<CharacterAnimator>();
        sound = GetComponent<CharacterAudio>();
        health = GetComponent<CharacterHealth>();
        if (GetComponent<PCController>())
            IsHero = true;
        //Debug.Log(name + " Past Awake");
        Initialized = true;
    }

    private void OnEnable()
    {
        mover.OnMovementFinished += Mover_OnMovementFinished;
        animator.OnActionAnimationFinished += Animator_OnActionAnimationFinished;
    }

    private void OnDisable()
    {
        mover.OnMovementFinished -= Mover_OnMovementFinished;
        animator.OnActionAnimationFinished -= Animator_OnActionAnimationFinished;
    }
    //--------------------------------------------------------------------
    //                    Public Functions
    //--------------------------------------------------------------------

    public  void StartAttack(PowerSO attackPower, IDamageable target)
    {
        
        if (!health.canBeTarget)
            return;

        this.attackPower = attackPower;

        Vector3 moveToPosition=new Vector3();

        float range=0;

        switch (attackPower.target)
            {
            case TargetType.Enemy:
                if (target==null) return;
               // Debug.Log("Starting attack "+attackPower.name+" against " + target.GetName());            
                hasActed = false;
                targetArray = new IDamageable[1];
                targetArray[0] = target;

                moveToPosition = target.GetTransform().position;
                range = attackPower.range;

               if(target is CharacterHealth targetHealth)
                OnAnyAttackStarted(health, targetHealth,attackPower);//Eventually we need an avatar for items, but that's for later.

                             
                break;

            case TargetType.AllEnemies:
                hasActed = false;
                if(IsHero)
                targetArray = SpawnPointController.Instance.GetAllFraction(Faction.Enemy).ToArray();
                else
                    targetArray= SpawnPointController.Instance.GetAllFraction(Faction.Hero).ToArray();
                Debug.Log("All Enemy attack with " + targetArray.Length + " targets");

                moveToPosition = transform.position + transform.forward;
                range = 0;

                OnAnyAttackStarted(health, null,attackPower);

                break;

            case TargetType.Self:
            
                hasActed = false;
                targetArray = new IDamageable[1];
                targetArray[0] = health;

                moveToPosition = transform.position + transform.forward;
                range = 0;

                OnAnyAttackStarted(health, null,attackPower);
                break;

            case TargetType.AllFriends:
                hasActed = false;
                if (IsHero)
                    targetArray = SpawnPointController.Instance.GetAllFraction(Faction.Hero).ToArray();
                else
                    targetArray = SpawnPointController.Instance.GetAllFraction(Faction.Enemy).ToArray();

                //Debug.Log("Target array size " + targetArray.Length);

                moveToPosition = transform.position + transform.forward;
                range = 0;

                OnAnyAttackStarted(health, null, attackPower);
                break;

            case TargetType.AreaEnemies:
                hasActed = false;
                if (IsHero)
                    targetArray = SpawnPointController.Instance.GetAllInRadius(target.GetTransform(),attackPower.radius,Faction.Enemy).ToArray();
                else
                    targetArray = SpawnPointController.Instance.GetAllInRadius(target.GetTransform(), attackPower.radius, Faction.Hero).ToArray();

                moveToPosition = target.GetTransform().position;
                range = attackPower.range;
                OnAnyAttackStarted(health, null, attackPower);
                break;

        }

        animator.SetMove(true);
        mover.MoveTo(moveToPosition, range);
        //Debug.Log("Moving to " + target.transform.position);
    }

    public void StartMoveHome()
    {
        hasActed = true;     
        animator.SetMove(true);
        mover.MoveHome();
    }
   

    public bool ManageHit(IDamageable target)
        //This manages all the rules effects of a hit, called from the projectile function 
    {
        int successLevel = 0;
        float highHitDamage = 1;

        (successLevel, highHitDamage) = RollAttack(attackPower, target);
        if (successLevel == 0)
            return false;

        
        float damage = attackPower.GetDamage(highHitDamage);
       
        damage = GameSystem.Instance.CalculateDamage(stats.GetAttribute(Attribute.Power), damage);
       
        target.TakeDamage(damage);

        if (attackPower.momentumEffect)
        {
            OnMomentumModified.Invoke(this, attackPower.momentumChange);
        }

        if (!target.canBeTarget) //Target died
            return true;

        
        float damageModifier= GameSystem.Instance.CalculateDamage(stats.GetAttribute(Attribute.Power), 1);
        (StatusName status,float intensity,int duration)=attackPower.GetStatusEffect(successLevel);

        if (status != StatusName.None && target is CharacterHealth)
        {
            //Debug.Log("Applying Status effects");
            CharacterHealth targetCharacter = (CharacterHealth)target;
            targetCharacter.GetComponent<StatusManager>().GainStatus(status, intensity, duration, damageModifier);
        }

        return true;
    }

    //----------------------------------------------------------------------------------
    //            Private Functions
    //----------------------------------------------------------------------------------


    private (int,float) RollAttack(PowerSO attackPower, IDamageable target)
    {
        float attackValue = attackPower.attack;
        attackValue = GameSystem.Instance.CalculateAttack(stats.GetAttribute(Attribute.Combat), attackValue);

        float defenseValue = target.GetDefenseValue();
        float critModifier = 0;

        //Debug.Log(name + " Attacking " + target.Stats.GetName() + " with " + attackPower.name);
        return GameSystem.Instance.TestAttack(attackValue, defenseValue, critModifier);

    }

    private void Mover_OnMovementFinished()
    {
        
        if ( !hasActed)
        {          
            PerformAction();
        }
        else
        {          
            animator.SetMove(false);
            OnAttackFinished.Invoke(true);
            OnAnyActionFinished.Invoke();
        }
    }

    private void Animator_OnActionAnimationFinished()
    {
        OnAnyAttackEnded.Invoke();
        //FeelManager.Instance.EndAttack();
        effects.EndAttack();

        if (!mover.IsHome())
        {
            //Debug.Log("Switching to Move");
            mover.MoveHome();
            animator.SetMove(true);
        }
        else
        {
            //Debug.Log("Switching to Idle");
            animator.SetIdle();
            OnAttackFinished.Invoke(true);
            OnAnyActionFinished.Invoke();
        }
    }

    private void PerformAction()
        //Starts the Action after the Movement
        //All action effects are generated from Animation Events
    {
        hasActed = true;
        //Debug.Log("Starting action animation");
        health.SpendEndurance(attackPower.enduranceCost);
        animator.SetAttack(attackPower);
        effects.StartAttack(attackPower.HasProjectile(),attackPower.attackOriginArray);
        //start beginning attack FX

        sound.SetAttack(attackPower);
       
    }

    private void Impact() //Triggered from animations
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
                Destroy(muzzleVFX, 2);
                
            }

            sound.PlayShootSound(attackPower.hitSound);
            foreach (IDamageable target in targetArray)
            {             
                attackPower.LaunchProjectile(attackOrigin.position, this, target);
            }
            return;
        }

        //Debug.Log("No projectile");

        foreach (IDamageable target in targetArray)
        {
            bool hasHit = ManageHit(target);
            if (hasHit)
            {
                // Debug.Log("Hit, success level " + successLevel);
                if (attackPower.hitVFX)
                    effects.AttackingEffect(attackPower.hitVFX, target.GetTransform());

                sound.SetHitSound(attackPower, target.GetTransform());
                OnAnyPowerHit.Invoke();
            }
            else
            {
                //Debug.Log("Missed");
                effects.AttackingEffect(missEffect, target.GetTransform());
                sound.PlayHitSound(attackPower.missSound);
            }
        }
    }

    private void Buff()//Triggered from animations
    {
       
        foreach (CharacterHealth target in targetArray)
        {
            Debug.Log( target.name+" Buff applied ");
            if (attackPower.hitVFX)
            {
                CharacterVFX targetVFX = target.GetComponent<CharacterVFX>();
                if (targetVFX)
                {
                    targetVFX.BuffingEffect(attackPower.hitVFX);
                    //Debug.Log("Spawning VFX");
                }
                
            }

            sound.SetHitSound(attackPower, target.GetTransform());
            //FeelManager.Instance.BuffEffect();

            ManageBuff(target);
        }
    }

    public void ManageBuff(CharacterHealth target)
    //This manages all the rules effects of a buff
    //Right now this is a static effect, we may want to add a random effect later.
    {
        float heal = attackPower.GetDamage(1);
        heal = GameSystem.Instance.CalculateDamage(stats.GetAttribute(Attribute.Power), heal);
        target.Heal(heal);
     
        (StatusName status, float intensity, int duration) = attackPower.GetStatusEffect(0);

        if (status != StatusName.None)
        {
            float damageModifier = GameSystem.Instance.CalculateDamage(stats.GetAttribute(Attribute.Power), 1);
            target.GetComponent<StatusManager>().GainStatus(status, intensity, duration, damageModifier);
        }

        if (attackPower.momentumEffect)
        {
            OnMomentumModified.Invoke(this, attackPower.momentumChange);
        }
    }


}
