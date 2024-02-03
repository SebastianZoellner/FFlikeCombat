using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    private List<BaseStatus> activeStatusList;

    public CharacterHealth Health { get; private set; }
    private CharacterStats stats;


    private void Awake()
    {
        Health = GetComponent<CharacterHealth>();
        stats = GetComponent<CharacterStats>();

        activeStatusList = new List<BaseStatus>();

    }


    public void StartTurn()
    {
        
        foreach (BaseStatus bs in activeStatusList)
        {
            bs.OnTurnStart();
        }

       
    }

    public void GainStatus(StatusName newStatusName, float intensity, int duration)
    {

        BaseStatus newStatus = null;
        switch (newStatusName)
        {
            case StatusName.Bleeding:
                newStatus = new BleedingStatus(this, activeStatusList.Count, intensity, duration);
                break;
            case StatusName.Damage:
                newStatus = new DamageStatus(this, activeStatusList.Count, intensity, 0);
                break;

        }
        if (newStatus == null)
            return;

        activeStatusList.Add(newStatus);
        newStatus.BeginStatus();

    }

    public void LoseStatus(int statusIndex)
    {
        activeStatusList[statusIndex].EndStatus();
        activeStatusList.RemoveAt(statusIndex);
    }

    public string[] GetStatusNames()
    {
        string[] names = new string[] { };
        for (int i = 0; i < activeStatusList.Count; ++i)
            names[i] = "activeStatusList[i].GetStatusName()";

        return names;
    }
}

