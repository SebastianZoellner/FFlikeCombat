using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public CharacterStats Stats { get; private set; }
    public CharacterHealth Health { get; private set; }
    public StatusManager StatusManager { get; private set; }

    private void Awake()
    {
        Stats = GetComponent<CharacterStats>();
        Health = GetComponent<CharacterHealth>();
        StatusManager = GetComponent<StatusManager>();
    }
}
