using UnityEngine;

public class ShellShockedStatus : BaseStatus
{
    int turnCounter = 0;

    public ShellShockedStatus(StatusManager statusManager, int statusIndex, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, statusIndex, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.ShellShocked;
        statusManager.Health.TakeDamage(intensity*3);

    }

    public override void EndStatus()
    {
        
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
        ++turnCounter;
        if (turnCounter > duration)
            return true;
        return false;

    }
}
