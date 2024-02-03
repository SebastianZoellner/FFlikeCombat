using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private CharacterSO character;

    private float defense;
    private void Awake()
    {
        CharacterHealth health = GetComponent<CharacterHealth>();

        health.SetStartingHealth(character.startingHealth);
    }

    public string GetName() => character.name;
    public PowerSO GetPower(int index) => character.GetPower(index);
    public bool HasPowerID(int index) => character.HasPowerID(index);
    public int GetNumbeOfPowers() => character.powerArray.Length;
    public float GetDefenseValue() => defense;

    public void SetDefense(float powerDefenseValue)
    {
        defense = powerDefenseValue;
    }

}
