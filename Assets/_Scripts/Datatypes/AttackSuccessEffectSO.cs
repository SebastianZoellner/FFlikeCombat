using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Attack Effect", menuName = "Game Elements/Powers/Attack Effect")]

public class AttackSuccessEffectSO : ScriptableObject
{
    public StatusName status;
    [SerializeField] GameObject vfx;

    public float intensity;
    [Range(-1,10)]
   [SerializeField] int duration;
    [ShowInInspector] private float damageCost;

    [Button]
    public float DamageCost()
    {
        damageCost= BaseDamageCost() * intensity * DurationModifier();
        return damageCost;
    }

    public BaseStatus InitializeStatus(StatusManager manager, float damageModifier)
    {
        BaseStatus newStatus = null;

        switch (status)
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
            case StatusName.Healing:
                newStatus=new HealingStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.BoostCombat:
                newStatus = new BoostCombatStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.BoostAgillity:
                newStatus = new BoostAgillityStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.BoostInitiative:
                newStatus = new BoostInitiativeStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.ModifyEndurance:
                newStatus = new ModifyEnduranceStatus(manager, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.ModifyPower:
                newStatus = new ModifyPowerStatus(manager, intensity, damageModifier, duration, vfx);
                break;

        }

        return newStatus;
    }





    private float BaseDamageCost()
    {
        switch (status)
        {
            case StatusName.Bleeding:
                return 3 / 2.25f;
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
            case StatusName.Healing:
                return -0.9f;
            case StatusName.BoostInitiative:
                return -0.6f;
            case StatusName.BoostAgillity:
                return -0.25f;
            case StatusName.BoostCombat:
                return -0.25f;
            case StatusName.ModifyEndurance:
                return -0.2f;
            case StatusName.ModifyPower:
                return -1f; //this may be too low
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
