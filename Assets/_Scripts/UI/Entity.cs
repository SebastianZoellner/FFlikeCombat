using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public EntityType type { get; private set; }

    public CharacterStats Stats { get; private set; }
    public CharacterHealth Health { get; private set; }
    public StatusManager StatusManager { get; private set; }

    private void Awake()
    {
        Health = GetComponent<CharacterHealth>();

        if (type == EntityType.Character)
        {
            Stats = GetComponent<CharacterStats>();
            StatusManager = GetComponent<StatusManager>();
        }
    }
}
    public enum EntityType
    {
    Character,
    Object
    }

