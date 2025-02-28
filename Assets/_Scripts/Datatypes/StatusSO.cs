
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Status", menuName = "Game Elements/Powers/CharacterStatus")]

public class StatusSO : ScriptableObject
{
    [SerializeField] StatusName statusType;
    [SerializeField] GameObject vfx;

    [Range(0,10)]
    [SerializeField] float intensity;
    [Range(-1, 10)]
    [SerializeField] int duration;


    public float DamageCost()
    {
        return BaseDamageCost() * intensity * DurationModifier();
    }

    public BaseStatus InitializeStatus(StatusManager manager, float intensity, int duration, float damageModifier)
    {

        BaseStatus newStatus = null;

        switch (statusType)
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
                newStatus = new BlockingStatus(manager, intensity, damageModifier, duration, vfx);
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

    private float BaseDamageCost()
    {
        switch (statusType)
        {
            case StatusName.Bleeding:
                return 3/2.25f;
            case StatusName.Damage:
                return 1;
            case StatusName.Entangled:
                return 2;
            case StatusName.ShellShocked:
                return 1;
            case StatusName.Blinded:
                return 1;
            case StatusName.Disadvantaged:
                return 0.5f;
            case StatusName.Shocked:
                return 0.5f;
            case StatusName.Boosted:
                return -2;
            case StatusName.TacticalAdvantage:
                return -2;
            case StatusName.Blocking:
                return 0.4f;
            case StatusName.Resting:
                return 2.5f;
            case StatusName.Enraged:
                return -0.9f;


        }
        return 0;
    }

    private float DurationModifier()
    {
        switch (duration)
        {
            case -1:
                return 0.75f;     //We use a negative duration to indicate that it ends until the end of the next action by the affected character.
            case 0:
                return 1;
            case 1:
                return 1;
            case 2:
                return 1.75f;
            case 3:
                return 2.25f;
        }

        if (duration > 3)
            return 2.5f;

        return 1;
    }
}
