
public class EntangleStatus : BaseStatus
{
    int turnCounter;

    public EntangleStatus(StatusManager statusManager, int statusIndex, float intensity, int duration) : base(statusManager, statusIndex, intensity, duration)
    {
    }

    public override void BeginStatus()
    {
        statusName = StatusName.Entangled;
    }

    public override void EndStatus()
    {
     //remove fx   
    }

    public override float GetAttributeEffect(Attribute attribute)
    {
        switch (attribute)
            {
            case Attribute.Initiative:
                return -intensity * 30;
                
            case Attribute.Speed:
                return -intensity * 20;
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
