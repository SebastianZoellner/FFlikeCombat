using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindedStatus : BaseStatus
{
    int turnCounter;

    public BlindedStatus(StatusManager statusManager, int statusIndex, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, statusIndex, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Blinded;
        statusManager.ModifyMomentum(-intensity);
    }

    public override void EndStatus()
    {
        
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        switch (attribute)
        {
            case Attribute.Combat:
                return -intensity * 10;

            case Attribute.Agillity:
                return -intensity * 5;
            default:
                return 0;
        }
    }

    public override void OnActivation() { }
    
    public override bool OnTurnStart()
    {
        ++turnCounter;
        if (turnCounter > duration)
            return true; ;

        return false;
    }
}
