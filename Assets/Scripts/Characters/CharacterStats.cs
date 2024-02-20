using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private CharacterSO character;

    private float baseDefense;
    private float endurance;
    private StatusManager statusManager;

    private void Awake()
    {
        CharacterHealth health = GetComponent<CharacterHealth>();

        health.SetStartingHealth(character.startingHealth);

        statusManager = GetComponent<StatusManager>();
        endurance = character.StartingEndurance;

    }

    public string GetName() => character.name;
    public Sprite GetIcon() => character.icon;
    public PowerSO GetPower(int index) => character.GetPower(index);
    public bool HasPowerID(int index) => character.HasPowerID(index);
    public int GetNumbeOfPowers() => character.powerArray.Length;
    public float GetLevel() => character.level;
    

    public AnimatorOverrideController GetAnimatorOverrideController() => character.animatorController;
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
        if(!statusManager)
            statusManager= GetComponent<StatusManager>();
        float statusModifiers = statusManager.GetAttributeModifiers(attribute);
        return (character.GetBaseAttribute(attribute)+statusModifiers);
    }

    public PowerSO[] GetAvailablePowers()
    {
        List<PowerSO> availablePowerList = new List<PowerSO>();
        float momentum = MomentumManager.GetMomentum();
        foreach(PowerSO pow in character.powerArray)
        {
            Debug.Log("Available power; considering " + pow.name);
            if (pow.enduranceCost > endurance) continue;
            Debug.Log("Passed endurance");
            if (pow.momentumEffect)
            {
                if (pow.momentumCost > momentum) continue;
                Debug.Log("Passed momentum cost");
                if (pow.maxMomentum < momentum) continue;
                Debug.Log("Passed maximum momentum");
                if (pow.minMomentum >= momentum) continue;
                Debug.Log("Passed min momentum");
            }
            availablePowerList.Add(pow);
        }

        return availablePowerList.ToArray();
    }
}
