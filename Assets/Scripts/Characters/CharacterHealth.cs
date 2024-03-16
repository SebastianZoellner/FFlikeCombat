using UnityEngine;
using Sirenix.OdinInspector;
using System;
using DamageNumbersPro;

public class CharacterHealth : MonoBehaviour
{
    public static Action<CharacterHealth> OnAnyPCDied=delegate { };
    public static Action<CharacterHealth> OnAnyEnemyDied = delegate { };
    public static Action<CharacterHealth> OnHeavyHit = delegate { };
    public event Action OnDied = delegate { }; 

    public Action OnHealthChanged = delegate { };

    [SerializeField] DamageNumber damagePrefab;
    [SerializeField] DamageNumber healPrefab;

    public float StartingHealth { get; private set; }
    public float PresentHealth { get; private set; }

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
        PresentHealth = StartingHealth;
        canBeTarget = true;
    }


    private void OnDisable()
    {
        ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
    }

    //-----------------------------------------------------------------------------------
    //                    Public functions
    //-----------------------------------------------------------------------------------

    public void TakeDamage(float damage)
    {
        //Debug.Log("Taking Damage " + damage);
        damage=GameSystem.Instance.CalculateArmor(Stats.GetAttribute(Attribute.Armor), damage);
        if (damage < 0) damage = 0;
        //Debug.Log("After Armor " + damage);
        if (damage > StartingHealth / 2)
            OnHeavyHit.Invoke(this);

        ChangeHealth(-damage);
        
    }

    public void Heal(float healAmount)
    {
        ChangeHealth(healAmount);
    }

    public void SetStartingHealth(float startingHealth)
    {
        this.StartingHealth = startingHealth;
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

    private void ActionSequencer_OnNewRoundStarted(int obj)
    {
        float recoveredHealth = Stats.GetAttribute(Attribute.Hardiness);
        if(recoveredHealth!=0)
        ChangeHealth(recoveredHealth);
    }

    private void Die()
    {
        canBeTarget = false;
        animator.SetDied();
       // Debug.Log(name + " died");
        if (GetComponent<PCController>())
            OnAnyPCDied.Invoke(this);
        else
            OnAnyEnemyDied.Invoke(this);

        OnDied.Invoke();
              
        GetComponentInChildren<CapsuleCollider>().enabled = false;
    }
}
