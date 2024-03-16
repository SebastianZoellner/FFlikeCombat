using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{ 
    public static Action<float> OnChangeMomentum;
    
    private List<BaseStatus> activeStatusList;

    public CharacterHealth Health { get; private set; }
    
    private CharacterStats stats;
    private CharacterVFX effects;
    private CharacterInitiative initiative;
    private StatusController statusController;

    private bool isHero = false;

    private void Awake()
    {
        statusController = FindObjectOfType<StatusController>();
        Health = GetComponent<CharacterHealth>();
        stats = GetComponent<CharacterStats>();
        effects = GetComponent<CharacterVFX>();
        initiative = GetComponent<CharacterInitiative>();

        isHero = GetComponent<PCController>();
        activeStatusList = new List<BaseStatus>();
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
        BaseStatus newStatus = statusController.GetNewStatus(this,newStatusName,intensity,duration,damageModifier);
       
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

    public void EndAll()
    { 
        Debug.Log("Ending all statuses");
        List<BaseStatus> endedStatusList = new List<BaseStatus>();

        foreach (BaseStatus bs in activeStatusList)
        {
                endedStatusList.Add(bs);
        }

        foreach (BaseStatus bs in endedStatusList)
            activeStatusList.Remove(bs);

        activeStatusList.Clear();
    }

    //-----------------------------------------------------------------
    //                  Private Functions
    //-----------------------------------------------------------------

    private void Health_OnDied()
    {
        EndAll();
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
            if (activeStatusList.Count == 0)  //Activation may have killed the character, in which case this list is now empty
                return;
        }

        foreach (BaseStatus bs in endedStatusList)
            activeStatusList.Remove(bs);
    }
}



