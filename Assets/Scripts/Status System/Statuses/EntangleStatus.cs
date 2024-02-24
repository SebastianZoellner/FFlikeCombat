
using UnityEngine;

public class EntangleStatus : BaseStatus
{
    int turnCounter;
    GameObject activeVFX;

    public EntangleStatus(StatusManager statusManager, int statusIndex, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, statusIndex, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Entangled;
        if (statusVFX)
        {
            Debug.Log("Entangled animation started");
            activeVFX = statusManager.InitializeStatusVFX(statusVFX);
        }
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
                return -intensity * 30;
                
            case Attribute.Speed:
                return -intensity * 20;
            default:
                return 0;
        }
    }

    public override void OnActivation()
    {
       
    }

    public override bool OnTurnStart()
    {
        ++turnCounter;
        if (turnCounter > duration)
            return true; ;

        return false;
    }

    
}
