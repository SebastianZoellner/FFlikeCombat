using UnityEngine;

public class DamageStatus : BaseStatus
{
    public DamageStatus(StatusManager statusManager, int statusIndex, float intensity, float damageModifier, int duration, GameObject statusVFX) : base(statusManager, statusIndex, intensity, damageModifier, duration, statusVFX)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Damage;
        statusManager.Health.TakeDamage(intensity*damageModifier);     
        statusManager.LoseStatus(this);
    }



    public override bool OnActivation() { return false; }


    public override bool OnTurnStart() { return false; }
    
    
    public override void EndStatus()
    {
        
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        return 0;
    }
}
