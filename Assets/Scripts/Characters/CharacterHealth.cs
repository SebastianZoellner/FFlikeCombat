using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class CharacterHealth : MonoBehaviour
{
    public static Action<CharacterHealth> OnAnyPCDied=delegate { };
    public static Action<CharacterHealth> OnAnyEnemyDied = delegate { };

    public Action OnHealthChanged = delegate { };

    public float StartingHealth { get; private set; }
    public float PresentHealth { get; private set; }

    public SelectionIndicator selectionIndicator { get; private set; }
    public CharacterStats Stats { get; private set; }

    private void Awake()
    {
        selectionIndicator = GetComponent<SelectionIndicator>();
        Stats = GetComponent<CharacterStats>();
    }

    private void Start()
    {
        PresentHealth = StartingHealth;
    }

    //public float GetHealth() => PresentHealth;


    public void TakeDamage(float damage)
    {
        Debug.Log("Taking Damage " + damage);
        ChangeHealth(-damage);
    }

    public void SetStartingHealth(float startingHealth)
    {
        this.StartingHealth = startingHealth;
    }



    private void ChangeHealth(float change)
    {
        PresentHealth += change;
        if (PresentHealth > StartingHealth)
            PresentHealth = StartingHealth;

        OnHealthChanged.Invoke();
        if (PresentHealth <= 0)
            Die();
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
