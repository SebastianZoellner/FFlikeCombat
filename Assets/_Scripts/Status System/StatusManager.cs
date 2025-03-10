using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    public static Action<float> OnChangeMomentum = delegate { };
    public event Action OnStatusChanged=delegate { };
    
    private List<BaseStatus> activeStatusList;

    public CharacterHealth Health { get; private set; }
    
    private CharacterStats stats;
    private CharacterVFX effects;
    private CharacterInitiative initiative;
    //private StatusController statusController;

    

    private bool isHero = false;
    private bool isObject = false;

    private void Awake()
    {
        //statusController = FindObjectOfType<StatusController>();
        Health = GetComponent<CharacterHealth>();
        stats = GetComponent<CharacterStats>();
        effects = GetComponent<CharacterVFX>();
        initiative = GetComponent<CharacterInitiative>();
        if (!initiative)
            isObject = true;
        isHero = GetComponent<PCController>();
        activeStatusList = new List<BaseStatus>();
    }

    

    private void OnEnable()
    {
        ActionSequencer.OnNewRoundStarted += StartTurn;
        Health.OnDied += Health_OnDied;
        if(!isObject)
        initiative.OnActionStarted += ActionStarted;
    }

   

    private void OnDisable()
    {
        ActionSequencer.OnNewRoundStarted -= StartTurn;
        Health.OnDied -= Health_OnDied;
        if (!isObject)
            initiative.OnActionStarted -= ActionStarted;

    }
    //----------------------------------------------------------
    //              Public functions
    //----------------------------------------------------------

    public float GetAttributeModifiers(Attribute attribute)
    {
        if (activeStatusList == null)
            return 0;

        float modifiers = 0;
        
        foreach (BaseStatus bs in activeStatusList)
        {
            modifiers+=bs.GetAttributeEffect(attribute);
        }
        return modifiers;
    }



    public void GainStatus(AttackSuccessEffectSO attackStatus, float damageModifier)
    {
        BaseStatus newStatus = attackStatus.InitializeStatus(this, damageModifier);

        Debug.Log("Gaining status on character " + name);

        if (newStatus == null)
            return;

        activeStatusList.Add(newStatus);
        newStatus.BeginStatus();
        OnStatusChanged.Invoke();
    }

    public void LoseStatus(BaseStatus status)
    {
        status.EndStatus();
        activeStatusList.Remove(status);
        OnStatusChanged.Invoke();
    }

    public string[] GetStatusNames()
    {
        string[] names = new string[activeStatusList.Count];
        for (int i = 0; i < activeStatusList.Count; ++i)
            names[i] = activeStatusList[i].GetStatusOutput();

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
        if (isObject)
            return;

        initiative.ChangeActionTime(change);
    }

    public void HealDamage(float intensity)
    {
        float healAmount = GameSystem.Instance.CalculateDamage(stats.GetAttribute(Attribute.Power), intensity);
        Health.Heal(healAmount);
    } 

    public void EndAll()
    { 
        //Debug.Log("Ending all statuses");
        List<BaseStatus> endedStatusList = new List<BaseStatus>();

        foreach (BaseStatus bs in activeStatusList)
        {
                endedStatusList.Add(bs);
        }

        foreach (BaseStatus bs in endedStatusList)
        {
            LoseStatus(bs);
            //Debug.Log("Removing " + bs.GetStatusName());
        }
        //Debug.Log("All statuses ended");

        activeStatusList.Clear();
    }

    public void StartTacticalAdvantage()
    {
        effects.StartTacticalAdvantage();
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
            LoseStatus(bs);
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
        {
            LoseStatus(bs);
        }
    }
}



