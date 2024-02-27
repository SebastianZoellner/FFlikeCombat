using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{

    public static Action<float> OnChangeMomentum;
    
    [SerializeField] StatusEffect[] statusEffectArray;

    private List<BaseStatus> activeStatusList;

    public CharacterHealth Health { get; private set; }
    
    private CharacterStats stats;
    private CharacterVFX effects;
    private CharacterInitiative initiative;

    private Dictionary<StatusName, GameObject> statusEffectDictionary;
    private bool isHero = false;

    private void Awake()
    {
        Health = GetComponent<CharacterHealth>();
        stats = GetComponent<CharacterStats>();
        effects = GetComponent<CharacterVFX>();
        initiative = GetComponent<CharacterInitiative>();

        isHero = GetComponent<PCController>();
        activeStatusList = new List<BaseStatus>();
        initializeDictionary();

    }

    

    private void OnEnable()
    {
        ActionSequencer.OnNewRoundStarted += StartTurn;
        Health.OnDied += Health_OnDied;
        initiative.OnActionStarted += ActionStarted;
    }

   

    private void OnDisable()
    {
        ActionSequencer.OnNewRoundStarted -= StartTurn;
        Health.OnDied -= Health_OnDied;
        initiative.OnActionStarted -= ActionStarted;

    }
    //----------------------------------------------------------
    //              Public functions
    //----------------------------------------------------------

    public float GetAttributeModifiers(Attribute attribute)
    {
        float modifiers = 0;
        foreach (BaseStatus bs in activeStatusList)
        {
            modifiers+=bs.GetAttributeEffect(attribute);
        }
        return modifiers;
    }

    public void GainStatus(StatusName newStatusName, float intensity, int duration,float damageModifier)
    {
        BaseStatus newStatus = null;
        GameObject vfx = null;

        if (statusEffectDictionary.ContainsKey(newStatusName))
            vfx = statusEffectDictionary[newStatusName];

        switch (newStatusName)
        {
            case StatusName.Bleeding:                
                newStatus = new BleedingStatus(this, activeStatusList.Count, intensity, damageModifier,duration,vfx);
                break;
            case StatusName.Damage:
                newStatus = new DamageStatus(this, activeStatusList.Count, intensity, damageModifier,0,vfx);
                break;
            case StatusName.Entangled:
                newStatus = new EntangleStatus(this, activeStatusList.Count, intensity, damageModifier, duration,vfx);
                break;
            case StatusName.ShellShocked:
                newStatus = new ShellShockedStatus(this, activeStatusList.Count, intensity, damageModifier, duration,vfx);
                break;
            case StatusName.Blinded:
                newStatus = new BlindedStatus(this, activeStatusList.Count, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.Disadvantaged:
                newStatus=new DisadvantagedStatus(this, activeStatusList.Count, intensity, damageModifier, duration, vfx);
                break;
            case StatusName.Shocked:
                newStatus = new ShockedStatus(this, activeStatusList.Count, intensity, damageModifier, duration, vfx);
                break;

        }
        if (newStatus == null)
            return;

        activeStatusList.Add(newStatus);
        newStatus.BeginStatus();
    }

    public void LoseStatus(BaseStatus status)
    {
        status.EndStatus();
        activeStatusList.Remove(status);
    }

    public string[] GetStatusNames()
    {
        string[] names = new string[activeStatusList.Count];
        for (int i = 0; i < activeStatusList.Count; ++i)
            names[i] = activeStatusList[i].GetStatusName().ToString();

        return names;
    }

    public GameObject InitializeStatusVFX(GameObject StatusVFX)
    {
        return effects.InitializeStatusVFX(StatusVFX);
    }

    public void ModifyMomentum(float change)
    {
        if (isHero)
            OnChangeMomentum.Invoke(change);
        else
            OnChangeMomentum.Invoke(-change);
    }

    public void ModifyNextActionTime(float change)
    {
        initiative.ChangeActionTime(change);
    }

    private void initializeDictionary()
    {
        statusEffectDictionary = new Dictionary<StatusName, GameObject>();

        foreach(StatusEffect se in statusEffectArray)
        {
            statusEffectDictionary[se.name] = se.effect;
        }
    }

    private void Health_OnDied()
    {
        Debug.Log("Ending statuses");
        foreach (BaseStatus bs in activeStatusList)
            bs.EndStatus();
        activeStatusList.Clear();
    }

    private void StartTurn(int turn)
    {
        List<BaseStatus> endedStatusList = new List<BaseStatus>();

        foreach (BaseStatus bs in activeStatusList)
        {
            if(bs.OnTurnStart())
                endedStatusList.Add(bs);
        }

        foreach (BaseStatus bs in endedStatusList)
            activeStatusList.Remove(bs);
    }

    private void ActionStarted()
    {
        List<BaseStatus> endedStatusList = new List<BaseStatus>();
        foreach (BaseStatus bs in activeStatusList)
        {
            if(bs.OnActivation())
                endedStatusList.Add(bs);
        }

        foreach (BaseStatus bs in endedStatusList)
            activeStatusList.Remove(bs);
    }
}

[System.Serializable]
struct StatusEffect
{
    public StatusName name;
    public GameObject effect;
}

