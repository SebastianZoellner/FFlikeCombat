

using UnityEngine;

public abstract class BaseStatus
{
    protected int statusIndex;
    protected StatusManager statusManager;
    protected float intensity;
    protected float duration;
    protected float damageModifier;
    protected StatusName statusName;

    protected GameObject statusVFX;

    public BaseStatus(StatusManager statusManager,int statusIndex, float intensity,float damageModifier,int duration, GameObject statusVFX)
    {
        this.statusManager = statusManager;
        this.statusIndex = statusIndex;
        this.intensity = intensity;
        this.duration = duration;
        this.damageModifier = damageModifier;
        this.statusVFX = statusVFX;
    }

    protected BaseStatus(StatusManager statusManager, int statusIndex, float intensity, int duration)
    {
        this.statusManager = statusManager;
        this.statusIndex = statusIndex;
        this.intensity = intensity;
        this.duration = duration;
    }

    public abstract void BeginStatus();


    public abstract void OnActivation();


    public abstract bool OnTurnStart();


    public abstract void EndStatus();

    public abstract float GetAttributeEffect(Attribute attribute);

    public StatusName GetStatusName() => statusName;

}

