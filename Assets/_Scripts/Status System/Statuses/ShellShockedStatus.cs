using UnityEngine;

public class ShellShockedStatus : BaseStatus
{
    int turnCounter = 0;
    GameObject activeVFX;

    public ShellShockedStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.ShellShocked;
        statusManager.Health.TakeDamage(intensity*3);
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
            case Attribute.Initiative:
                return -intensity * 10;

            case Attribute.Combat:
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
        --duration;
        if (duration <=0)
            return true;
        return false;

    }
}
