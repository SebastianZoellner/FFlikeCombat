using UnityEngine;

public class RestingStatus : BaseStatus
{
    public RestingStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Resting;
        statusManager.Health.Envigorate();
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
