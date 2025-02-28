using UnityEngine;

public class EnragedStatus : BaseStatus
{
    int turnCounter;
    GameObject activeVFX;

    public EnragedStatus(StatusManager statusManager, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Enraged;
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
            case Attribute.Combat:
                return -intensity * 2.5f;

            case Attribute.Agillity:
                return -intensity * 5;
            case Attribute.Power:
                return intensity * 5;
            default:
                return 0;
        }
    }
    /// <summary>
    /// Returns true if the status is ending and false otherwise
    /// </summary>
    /// <returns></returns>
    public override bool OnActivation()
    {
        return duration==-1;
    }

    /// <summary>
    /// Returns true if the status is ending and false otherwise
    /// </summary>
    /// <returns></returns>
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
