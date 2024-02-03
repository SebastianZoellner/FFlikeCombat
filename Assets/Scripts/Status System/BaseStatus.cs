public abstract class BaseStatus
{
    protected int statusIndex;
    protected StatusManager statusManager;
    protected float intensity;
    protected float duration;
    protected StatusName statusName;

    public BaseStatus(StatusManager statusManager,int statusIndex, float intensity,int duration)
    {
        this.statusManager = statusManager;
        this.statusIndex = statusIndex;
        this.intensity = intensity;
        this.duration = duration;
    }


    public abstract void BeginStatus();


    public abstract void OnActivation();


    public abstract void OnTurnStart();


    public abstract void EndStatus();

    public StatusName GetStatusName() => statusName;

}

