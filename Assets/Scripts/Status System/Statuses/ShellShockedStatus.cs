using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellShockedStatus : BaseStatus
{
    int turnCounter = 0;
    public ShellShockedStatus(StatusManager statusManager, int statusIndex, float intensity, int duration) : base(statusManager, statusIndex, intensity, duration)
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

    public override void OnActivation()
    {
        
    }

    public override void OnTurnStart()
    {
        ++turnCounter;
        if (turnCounter > duration)
            statusManager.LoseStatus(statusIndex);

    }
}
