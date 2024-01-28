using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Power", menuName = "Game Elements/Powers")]

public class PowerSO : ScriptableObject
{
    [SerializeField] float minDamage;
    [SerializeField] float maxDamage;

    public float GetDamage()
    {
        return Random.Range(minDamage, maxDamage);
    }
}
