using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] float startingHealth;
    [SerializeField] float presentHealth;


    private void Start()
    {
        presentHealth = startingHealth;
    }

    public float GetHealth() => presentHealth;


    public void TakeDamage(float damage)
    {
        Debug.Log("Taking Damage " + damage);
        ChangeHealth(-damage);
    }

    public void SetStartingHealth(float startingHealth)
    {
        this.startingHealth = startingHealth;
    }



    private void ChangeHealth(float change)
    {
        presentHealth += change;
        if (presentHealth > startingHealth)
            presentHealth = startingHealth;
        if (presentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
