using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private CharacterSO character;

    private float baseDefense;
    private StatusManager statusManager;
    private CharacterHealth health;

    private void Awake()
    {
        statusManager = GetComponent<StatusManager>();
        health = GetComponent<CharacterHealth>();

        health.SetStartingValues(character.startingHealth,character.StartingEndurance);   
    }

    public string GetName() => character.CharacterName;
    public string GetBlurb() => character.shortBlurb;
    public Sprite GetIcon() => character.icon;
    public PowerSO GetPower(int index) => character.GetPower(index);
    public bool HasPowerID(int index) => character.HasPowerID(index);
    public int GetNumbeOfPowers() => character.powerArray.Length;
    public int GetLevel() => character.level;
    

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

    public PowerSO[] GetAvailablePowers(bool hero)
    {
        List<PowerSO> availablePowerList = new List<PowerSO>();
        float momentum = MomentumManager.GetMomentum();
        foreach(PowerSO pow in character.powerArray)
        {
            Debug.Log("Available power; considering " + pow.name);
            if (pow.enduranceCost >health.PresentEndurance ) continue;
            Debug.Log("Passed endurance");
            if (pow.momentumEffect)
            {
                if (hero)
                {
                    if (pow.momentumCost > 0 && pow.momentumCost > momentum) continue;
                    // Debug.Log("Passed momentum cost");
                    if (pow.maxMomentum < momentum) continue;
                    //Debug.Log("Passed maximum momentum");
                    if (pow.minMomentum >= momentum) continue;
                    //Debug.Log("Passed min momentum");
                }
                if(!hero)
                {
                    if (pow.momentumCost > 0 && pow.momentumCost <- momentum) continue;
                    Debug.Log("Passed momentum cost");
                    if (-pow.maxMomentum > momentum) continue;
                    Debug.Log("Passed maximum momentum");
                    if (-pow.minMomentum <= momentum) continue;
                    Debug.Log("Passed min momentum");
                }
            }
            availablePowerList.Add(pow);
        }

        return availablePowerList.ToArray();
    }
}
