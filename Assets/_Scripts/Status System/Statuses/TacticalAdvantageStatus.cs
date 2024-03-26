
using UnityEngine;

public class TacticalAdvantageStatus : BaseStatus
{
    public TacticalAdvantageStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }
 
    private int actionCounter;

    public override void BeginStatus()
    {
        statusName = StatusName.TacticalAdvantage;
        statusManager.StartTacticalAdvantage();
    }

    public override void EndStatus()
    {
       
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        switch (attribute)
        {          
            case Attribute.Combat:
                return 10;

            case Attribute.Power:
                return 10;

            default:
                return 0;
        }

    }

    public override bool OnActivation()
    {
        ++actionCounter;
        if (actionCounter > 1)
            return true;

        return false;
    }

    public override bool OnTurnStart()
    {
        return false;
    }
}
