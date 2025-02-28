using UnityEngine;

public class BoostInitiativeStatus : BaseStatus
{
    int turnCounter;
    GameObject activeVFX;

    public BoostInitiativeStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.BoostInitiative;
       
        if (statusVFX)
        {
            //Debug.Log("Starting blinded VFX");
            activeVFX = statusManager.InitializeStatusVFX(statusVFX);
        }
    }


    public override void EndStatus()
    {
    if (activeVFX)
    {
        Debug.Log("Destroying enraged VFX");
        GameObject.Destroy(activeVFX);
    }
}

    public override float GetAttributeEffect(Attribute attribute)
    {
        switch (attribute)
        {
            case Attribute.Initiative:
                return intensity * 5f;
            default:
                return 0;
        }
    }

    public override bool OnActivation()
    {
        return duration == -1;
    }

    public override bool OnTurnStart()
    {
        if (duration < 1)
            return false;

        ++turnCounter;
        if (turnCounter > duration)
            return true;

        return false;
    }
}
