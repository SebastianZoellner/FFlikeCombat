using System;
using UnityEngine;

public class ObjectHealth : MonoBehaviour,IDamageable
{
    public event Action OnObjectDestroyed = delegate { };
    public float StartingHealth { get; private set; }
    public float PresentHealth { get; private set; }

    public SelectionIndicator selectionIndicator { get; private set; }
    public bool canBeTarget { get; private set; }
    public ObjectStats Stats { get; private set; }

    public bool IsHero { get; private set; } = false;

    private void Awake()
    {
        selectionIndicator = GetComponent<SelectionIndicator>();
        Stats = GetComponent<ObjectStats>();
    }

    private void Start()
    {
        StartingHealth = Stats.GetStartingHealth();

        PresentHealth = StartingHealth;
        canBeTarget = true;
    }

    //-----------------------------------------------------------------------------------
    //                    Public functions
    //-----------------------------------------------------------------------------------
    public string GetName() => Stats.GetName();
    public Transform GetTransform() => transform;
    public float GetDefenseValue() => Stats.GetDefenseValue();

    public void TakeDamage(float damage)
    {
        Debug.Log(name + "Taking Damage " + damage);
        damage = GameSystem.Instance.CalculateArmor(Stats.GetArmor(), damage);
        if (damage < 0) damage = 0;
        

        ChangeHealth(-damage);

    }

    //------------------------------------------------------------------------
    //                  Private functions
    //------------------------------------------------------------------------


    private void ChangeHealth(float change)
    {
        /*
        if (change > 0)
            healPrefab.Spawn(transform.position + 2 * Vector3.up, change);
        if (change < 0)
            damagePrefab.Spawn(transform.position + 2 * Vector3.up, -change);
        */

        PresentHealth += change;
        if (PresentHealth > StartingHealth)
            PresentHealth = StartingHealth;

       // OnHealthChanged.Invoke();
        if (PresentHealth <= 0)
        {
            Destroy();
            return;
        }   
    }

    private void Destroy()
    {
        canBeTarget = false;
        //animator.SetDied();
        //GetComponentInChildren<BoxCollider>().enabled = false;
        
        OnObjectDestroyed.Invoke();
       
    }
}
