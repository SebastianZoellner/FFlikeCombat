using UnityEngine;
using Sirenix.OdinInspector;
using System;
using DamageNumbersPro;

public class CharacterHealth : MonoBehaviour,IDamageable
{
    public static Action<CharacterHealth> OnAnyPCDied=delegate { };
    public static Action<CharacterHealth> OnAnyEnemyDied = delegate { };
    public static Action<CharacterHealth> OnHeroResurrected = delegate { };
    public static Action<CharacterHealth> OnHeavyHit = delegate { };
    public event Action OnDied = delegate { }; 

    public Action OnHealthChanged = delegate { };
   
    public Action OnInvigorate = delegate { };

    [SerializeField] DamageNumber damagePrefab;
    [SerializeField] DamageNumber healPrefab;

    public float StartingHealth { get; private set; }
    public float PresentHealth  { get; private set; }

    public float StartingEndurance { get; private set; }
    public float PresentEndurance { get; private set; }

   public SelectionIndicator selectionIndicator { get; private set; }
    public bool canBeTarget { get; private set; }
    public CharacterStats Stats { get; private set; }
    public bool IsHero{ get; private set; }

    private CharacterAnimator animator;

    

    private void Awake()
    {
       selectionIndicator = GetComponent<SelectionIndicator>();
        Stats = GetComponent<CharacterStats>();
        animator = GetComponent<CharacterAnimator>();
        IsHero = GetComponent<PCController>();    
    }

    private void OnEnable()
    {
        ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
    }

    private void Start()
    {
        StartingHealth = Stats.GetStartingHealth();
        StartingEndurance = Stats.GetStartingEndurance();

        PresentHealth = StartingHealth;
        PresentEndurance = StartingEndurance;
        canBeTarget = true;
    }


    private void OnDisable()
    {
        ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
    }

    //-----------------------------------------------------------------------------------
    //                    Public functions
    //-----------------------------------------------------------------------------------

    public string GetName() => Stats.GetName();
    public Transform GetTransform() => transform;
    public float GetDefenseValue() => Stats.GetDefenseValue();


    public void TakeDamage(float damage)
    {
       Debug.Log(name+"Taking Damage " + damage);
        damage=GameSystem.Instance.CalculateArmor(Stats.GetAttribute(Attribute.Armor), damage);
        if (damage < 0) damage = 0;
        //Debug.Log("After Armor " + damage);
        if (damage > HealthGameSystem.Instance.HeavyHitThreshold(StartingHealth))
            OnHeavyHit.Invoke(this);

        ChangeHealth(-damage);
        
    }

    public void Heal(float healAmount)
    {
        if(healAmount>0)
        ChangeHealth(healAmount);
    }

    public void SpendEndurance(float cost)
    {
        if (cost > 0)
            ChangeEndurance(-cost);
    }

    public void Envigorate()
    {
        float recoveredHealth = HealthGameSystem.Instance.RestHealthBonus(StartingHealth, Stats.GetAttribute(Attribute.Hardiness));
        if (recoveredHealth != 0)
            ChangeHealth(recoveredHealth);

        ChangeEndurance(HealthGameSystem.Instance.RestEnduranceBonus(StartingEndurance));
        OnInvigorate.Invoke();
    }

    public void Raise()
    {
        canBeTarget = true;
        animator.SetRaised();

        PresentEndurance = HealthGameSystem.Instance.EnduranceAfterRaise(StartingEndurance);
        PresentHealth = HealthGameSystem.Instance.HealthAfterRaise(StartingHealth);

        OnHeroResurrected.Invoke(this);
    }

    public void SetStartingValues(float startingHealth, float startingEndurance)
    {
        this.StartingHealth = startingHealth;
        this.StartingEndurance = startingEndurance;

        PresentHealth = StartingHealth;
        PresentEndurance = StartingEndurance;
    }

    public void NewStage()
    {
        ChangeEndurance(HealthGameSystem.Instance.NewStageEndurance( StartingEndurance));
        ChangeHealth(HealthGameSystem.Instance.NewStageHeal(StartingHealth - PresentHealth));
            
    }

    //------------------------------------------------------------------------
    //                  Private functions
    //------------------------------------------------------------------------


    private void ChangeHealth(float change)
    {
        if (change > 0)
            healPrefab.Spawn(transform.position+2*Vector3.up, change);
        if(change<0)
            damagePrefab.Spawn(transform.position+2*Vector3.up, -change);

        PresentHealth += change;
        if (PresentHealth > StartingHealth)
            PresentHealth = StartingHealth;

        OnHealthChanged.Invoke();
        if (PresentHealth <= 0)
        {
            Die();
            return;
        }

        if (change < 0)
            animator.SetHit();
    }

    private void ChangeEndurance(float change)
    {
        PresentEndurance += change;
        if (PresentEndurance > StartingEndurance)
            PresentEndurance = StartingEndurance;

        if (PresentEndurance < 0)
            PresentEndurance = 0;

        OnHealthChanged.Invoke();

    }

    private void ActionSequencer_OnNewRoundStarted(int obj)
    {
        float recoveredHealth = HealthGameSystem.Instance.NewRoundHealthBonus(StartingHealth, Stats.GetAttribute(Attribute.Hardiness));
        if(recoveredHealth!=0)
        ChangeHealth(recoveredHealth);
        ChangeEndurance(HealthGameSystem.Instance.NewRoundEnduranceBonus(StartingEndurance));
    }

    private void Die()
    {
        canBeTarget = false;
        animator.SetDied();
        Debug.Log(name + " died");
        if (GetComponent<PCController>())
            OnAnyPCDied.Invoke(this);
        else
            if (GetComponent<EnemyController>())
                OnAnyEnemyDied.Invoke(this);
           
        OnDied.Invoke();

        if (!IsHero)
            GetComponentInChildren<CapsuleCollider>().enabled = false;
    } 
}
