using UnityEngine;

//Reduces Combat by 10* intensity and defense by 5* intensity

public class BlindedStatus : BaseStatus
{
    int turnCounter;
    GameObject activeVFX;

    public BlindedStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Blinded;
        statusManager.ModifyMomentum(-intensity);
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
            Debug.Log("Destroying blinded VFX");
            GameObject.Destroy(activeVFX);
        }
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

    public override bool OnActivation() { return false; }
    
    public override bool OnTurnStart()
    {
        ++turnCounter;
        if (turnCounter > duration)
            return true; ;

        return false;
    }
}
