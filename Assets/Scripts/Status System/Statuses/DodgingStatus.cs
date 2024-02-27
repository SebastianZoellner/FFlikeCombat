using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgingStatus : BaseStatus
{
    public DodgingStatus(StatusManager statusManager, int statusIndex, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, statusIndex, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        
    }

    public override void EndStatus()
    {
        
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        switch (attribute)
        {
            case Attribute.Agillity:
                return intensity * 10;

            default:
                return 0;
        }
    }

    public override bool OnActivation()
    {
        return true;
    }

    public override bool OnTurnStart()
    {
       return false;
    }
}
