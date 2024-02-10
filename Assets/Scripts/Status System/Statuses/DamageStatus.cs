using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStatus : BaseStatus
{
    public DamageStatus(StatusManager statusManager, int statusIndex, float intensity, int duration) : base(statusManager, statusIndex, intensity, duration)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Damage;
        statusManager.Health.TakeDamage(intensity);
        Debug.Log("Removing Status Index " + statusIndex);
        statusManager.LoseStatus(statusIndex);
    }



    public override void OnActivation() { }


    public override void OnTurnStart() { }
    
    
    public override void EndStatus()
    {
        
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        return 0;
    }
}
