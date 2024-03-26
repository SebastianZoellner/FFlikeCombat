using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostedStatus : BaseStatus
{
    int turnCounter = 0;
    GameObject activeVFX;

    public BoostedStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Boosted;
        
        if (statusVFX)
        {
            activeVFX = statusManager.InitializeStatusVFX(statusVFX);
        }
    }

    public override void EndStatus()
    {
        if (activeVFX)
            GameObject.Destroy(activeVFX);
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        switch (attribute)
        {
            case Attribute.Speed:
                return intensity * 10;

            case Attribute.Combat:
                return intensity * 10;

            case Attribute.Power:
                return intensity * 10;

            case Attribute.Agillity:
                return -intensity * 10;

            default:
                return 0;
        }
    }

    public override bool OnActivation()
    {
        return false;
    }

    public override bool OnTurnStart()
    {
        ++turnCounter;
        if (turnCounter > duration)
            return true;
        return false;
    }
}
