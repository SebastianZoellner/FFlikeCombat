using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockedStatus : BaseStatus
{
    public ShockedStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Shocked;
        statusManager.ModifyNextActionTime(intensity * 0.1f);
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
