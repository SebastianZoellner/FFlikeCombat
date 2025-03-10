using UnityEngine;

public class BleedingStatus : BaseStatus
{
     private GameObject activeVFX;

    public BleedingStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Bleeding;
        if (statusVFX)
        {
            //Debug.Log("Bleedin animation started");
            activeVFX = statusManager.InitializeStatusVFX(statusVFX);
        }
    }  

    public override bool OnActivation()
    {
        //Debug.Log("Bleeding");
        statusManager.Health.TakeDamage(intensity * damageModifier,null);
        --duration;

        if (duration<=0)
            return true;

        return false;
    }

    public override bool OnTurnStart()
    {
        return false;    
    }

    public override void EndStatus()
    {
        if (activeVFX)
        {
            //Debug.Log("Destroying Bleeding VFX");
            GameObject.Destroy(activeVFX);
        }
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        return 0;
    }
}

