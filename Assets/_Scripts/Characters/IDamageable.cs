using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    public float StartingHealth { get;}
    public float PresentHealth { get; }

    public SelectionIndicator selectionIndicator { get;}
    public bool canBeTarget { get;}

    public bool IsHero { get; }

    public void TakeDamage(float damage, CharacterCombat source);
    public string GetName();
    public Transform GetTransform();
    public float GetDefenseValue();
    

}
