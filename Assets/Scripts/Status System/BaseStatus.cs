

using UnityEngine;

public abstract class BaseStatus
{
    //protected int statusIndex;
    protected StatusManager statusManager;
    protected float intensity;
    protected float duration;
    protected float damageModifier;
    protected StatusName statusName;

    protected GameObject statusVFX;

    public BaseStatus(StatusManager statusManager, float intensity,float damageModifier,int duration, GameObject statusVFX)
    {
        this.statusManager = statusManager;
        this.intensity = intensity;
        this.duration = duration;
        this.damageModifier = damageModifier;
        this.statusVFX = statusVFX;
    }

    public abstract void BeginStatus();


    public abstract bool OnActivation();


    public abstract bool OnTurnStart();


    public abstract void EndStatus();

    public abstract float GetAttributeEffect(Attribute attribute);

    public StatusName GetStatusName() => statusName;

}

