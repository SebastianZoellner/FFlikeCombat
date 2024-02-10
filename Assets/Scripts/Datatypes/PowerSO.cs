using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power", menuName = "Game Elements/Powers")]

public class PowerSO : ScriptableObject
{
    [SerializeField] float minDamage;
    [SerializeField] float maxDamage;
    public float attack;
    public float defense;
    [SerializeField] SuccessEffect[] successEffectArray;
    public string buttonName;
    public Sprite icon;
    public float setupTime;
    public float recoveryTime;
    public float range;

    public float GetDamage()
    {
        return UnityEngine.Random.Range(minDamage, maxDamage);
    }

    public (StatusName,float,int) GetStatusEffect(int successLevel)
    {
        foreach(SuccessEffect se in successEffectArray)
        {
            if (successLevel == se.level)
                return (se.status, se.intensity,se.duration);
        }
        return (StatusName.None, 0,0);
    }
}


[Serializable]
public struct SuccessEffect
{
    public int level;
    public StatusName status;
    public float intensity;
    public int duration;

}
