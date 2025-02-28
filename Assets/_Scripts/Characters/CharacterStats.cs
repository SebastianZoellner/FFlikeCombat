using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour,IStats
{
    [SerializeField] private CharacterSO character;

    private float baseDefense;
    private StatusManager statusManager;
   private CharacterHealth health;
    private CharacterExperience experience;

    private void Awake()
    {
        statusManager = GetComponent<StatusManager>();
        health = GetComponent<CharacterHealth>();
        experience = GetComponent<CharacterExperience>();

        health.SetStartingValues(character.GetBaseAttribute(Attribute.BaseHealth),character.GetBaseAttribute(Attribute.BaseEndurance));   
    }

    public string GetName() => character.CharacterName;
    public string GetBlurb() => character.shortBlurb;
    public Sprite GetIcon() => character.icon;
    public float GetStartingHealth() => character.GetBaseAttribute(Attribute.BaseHealth);

    public float GetExperienceValue() => character.xpValue;
    

    public float GetStartingEndurance() => character.GetBaseAttribute(Attribute.BaseEndurance);


    public PowerSO GetPower(int index) => character.GetPower(index);
    public bool HasPowerID(int index) => character.HasPowerID(index);
    public int GetNumbeOfPowers() => character.powerArray.Length;
    public int GetRank() => character.rank;
    

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
        if(ObjectManager.Instance)
            statusModifiers+=ObjectManager.Instance.GetAttributeModifier(attribute,character.fraction);
        return (character.GetBaseAttribute(attribute)+statusModifiers);
    }

    public PowerSO[] GetAvailablePowers(bool hero)
    {
        List<PowerSO> availablePowerList = new List<PowerSO>();
        float momentum = MomentumManager.GetMomentum();

        if (experience != null)
            foreach (PowerSO pow in character.leveledPowerArray)
            {
                //Debug.Log("Available power; considering " + pow.name);
                if (hero && pow.IsAvailable(momentum, health.PresentEndurance, experience.level))
                    availablePowerList.Add(pow);
                if (!hero && pow.IsAvailable(-momentum, health.PresentEndurance, experience.level))
                    availablePowerList.Add(pow);
            }

        foreach (PowerSO pow in character.powerArray)
        {
            //Debug.Log("Available power; considering " + pow.name);
            if (hero && pow.IsAvailable(momentum, health.PresentEndurance, 0))
                availablePowerList.Add(pow);
            if (!hero && pow.IsAvailable(-momentum, health.PresentEndurance, 0))
                availablePowerList.Add(pow);
        }




        return availablePowerList.ToArray();
    }
}
