using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [SerializeField] StatusEffect[] statusEffectArray;

    private Dictionary<StatusName, GameObject> statusEffectDictionary;

    private void Awake()
    {
        InitializeDictionary();
    }

    public BaseStatus GetNewStatus(StatusManager manager, StatusName newStatusName, float intensity, int duration, float damageModifier)
    {
        BaseStatus newStatus = null;
        GameObject vfx = null;

        if (statusEffectDictionary.ContainsKey(newStatusName))
            vfx = statusEffectDictionary[newStatusName];

        switch (newStatusName)
        {
            case StatusName.Bleeding:
                newStatus = new BleedingStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.Damage:
                newStatus = new DamageStatus(manager, intensity, damageModifier, 0, vfx);
                break;
            case StatusName.Entangled:
                newStatus = new EntangleStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.ShellShocked:
                newStatus = new ShellShockedStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.Blinded:
                newStatus = new BlindedStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.Disadvantaged:
                newStatus = new DisadvantagedStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.Shocked:
                newStatus = new ShockedStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.Boosted:
                newStatus = new BoostedStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.TacticalAdvantage:
                newStatus = new TacticalAdvantageStatus(manager, 0, 0, 0, null);
                break;
            case StatusName.Blocking:
                newStatus=new BlockingStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.Resting:
                newStatus = new RestingStatus(manager, intensity, 0, 0, null);
                break;
            case StatusName.Enraged:
                newStatus = new EnragedStatus(manager, intensity, damageModifier, duration, vfx);
                break;


        }

        return newStatus;
    }


    private void InitializeDictionary()
    {
        statusEffectDictionary = new Dictionary<StatusName, GameObject>();

        foreach (StatusEffect se in statusEffectArray)
        {
            statusEffectDictionary[se.name] = se.effect;
        }
    }
}

[System.Serializable]
struct StatusEffect
{
    public StatusName name;
    public GameObject effect;
}
