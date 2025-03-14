// increases the endurance by 5*intensity

using UnityEngine;

public class ModifyEnduranceStatus : BaseStatus
{
    public ModifyEnduranceStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.ModifyEndurance;
        statusManager.Health.ModifyEndurance(intensity*5);
        statusManager.LoseStatus(this);
    }

    public override void EndStatus()
    {
       
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        return 0;
    }

    public override bool OnActivation()
    {
        return false;
    }

    public override bool OnTurnStart()
    {
        return false;
    }
}
