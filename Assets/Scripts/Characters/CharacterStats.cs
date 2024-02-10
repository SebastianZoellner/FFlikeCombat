using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private CharacterSO character;

    private float baseDefense;
    private StatusManager statusManager;
    private void Awake()
    {
        CharacterHealth health = GetComponent<CharacterHealth>();

        health.SetStartingHealth(character.startingHealth);

        statusManager = GetComponent<StatusManager>();
    }

    public string GetName() => character.name;
    public Sprite GetIcon() => character.icon;
    public PowerSO GetPower(int index) => character.GetPower(index);
    public bool HasPowerID(int index) => character.HasPowerID(index);
    public int GetNumbeOfPowers() => character.powerArray.Length;
    public float GetDefenseValue() 
    {
        float statusModifiers = statusManager.GetAttributeModifiers(Attribute.Agillity);
        return GameSystem.Instance.CalculateDefense(GetAttribute(Attribute.Agillity)+statusModifiers, baseDefense);
    }

    public void SetDefense(float powerDefenseValue)
    {
        baseDefense = powerDefenseValue;
    }

    public float GetAttribute(Attribute attribute)
    {
        float statusModifiers = statusManager.GetAttributeModifiers(attribute);
        return (character.GetBaseAttribute(attribute)+statusModifiers);
    }
}
