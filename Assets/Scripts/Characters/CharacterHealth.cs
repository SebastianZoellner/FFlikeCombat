using UnityEngine;
using Sirenix.OdinInspector;
using System;
using DamageNumbersPro;

public class CharacterHealth : MonoBehaviour
{
    public static Action<CharacterHealth> OnAnyPCDied=delegate { };
    public static Action<CharacterHealth> OnAnyEnemyDied = delegate { };

    public Action OnHealthChanged = delegate { };

    [SerializeField] DamageNumber damagePrefab;
    [SerializeField] DamageNumber healPrefab;

    public float StartingHealth { get; private set; }
    public float PresentHealth { get; private set; }

    public SelectionIndicator selectionIndicator { get; private set; }
    public CharacterStats Stats { get; private set; }

    private void Awake()
    {
        selectionIndicator = GetComponent<SelectionIndicator>();
        Stats = GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
    }

   

    private void Start()
    {
        PresentHealth = StartingHealth;
    }


    private void OnDisable()
    {
        ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
    }



    public void TakeDamage(float damage)
    {
        Debug.Log("Taking Damage " + damage);
        damage=GameSystem.Instance.CalculateArmor(Stats.GetAttribute(Attribute.Armor), damage);
        Debug.Log("After Armor " + damage);
        ChangeHealth(-damage);
    }

    public void SetStartingHealth(float startingHealth)
    {
        this.StartingHealth = startingHealth;
    }



    private void ChangeHealth(float change)
    {
        if (change > 0)
            healPrefab.Spawn(transform.position+2*Vector3.up, change);
        else
            damagePrefab.Spawn(transform.position+2*Vector3.up, -change);

        PresentHealth += change;
        if (PresentHealth > StartingHealth)
            PresentHealth = StartingHealth;

        OnHealthChanged.Invoke();
        if (PresentHealth <= 0)
            Die();
    }

    private void ActionSequencer_OnNewRoundStarted(int obj)
    {
        float recoveredHealth = Stats.GetAttribute(Attribute.Hardiness);
        ChangeHealth(recoveredHealth);
    }

    private void Die()
    {
        if (GetComponent<PCController>())
            OnAnyPCDied.Invoke(this);
        else
            OnAnyEnemyDied.Invoke(this);

        Destroy(gameObject);
    }
}
