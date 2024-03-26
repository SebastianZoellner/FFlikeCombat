using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingStatus : BaseStatus
{

    GameObject activeVFX;

    public BlockingStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Blocking;

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
            case Attribute.Armor:
                return 40;

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
