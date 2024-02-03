using UnityEngine;

public class BleedingStatus : BaseStatus
{
    int turnCounter = 0;

    public BleedingStatus(StatusManager statusManager, int statusIndex, float intensity, int duration) : base(statusManager, statusIndex, intensity, duration)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Bleeding;
        //set up bleeding FX
    }  

    public override void OnActivation()
    {
        
    }

    public override void OnTurnStart()
    {
        ++turnCounter;
        if (turnCounter > duration)
            statusManager.LoseStatus(statusIndex);

        statusManager.Health.TakeDamage(intensity);
        Debug.Log("Bleeding");
    }
    
    public override void EndStatus()
    {
        //remove bleeding FX
    }
}

