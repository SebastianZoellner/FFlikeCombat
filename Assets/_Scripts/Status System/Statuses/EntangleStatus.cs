
using UnityEngine;

public class EntangleStatus : BaseStatus
{
    int turnCounter;
    GameObject activeVFX;

    public EntangleStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Entangled;
        if (statusVFX)
        {
            activeVFX = statusManager.InitializeStatusVFX(statusVFX);
        }
        statusManager.ModifyNextActionTime(intensity * 0.1f);
    }

    public override void EndStatus()
    {
        Debug.Log("Entangle status FX destoyed?");
        GameObject.Destroy(activeVFX);
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        switch (attribute)
            {
            case Attribute.Initiative:
                return -intensity * 15;
                
            case Attribute.Speed:
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
            return true; ;

        return false;
    }

    
}
