using UnityEngine;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public EntityType type { get; private set; }

    public CharacterStats Stats { get; private set; }
    public IDamageable Health { get; private set; }
    public StatusManager StatusManager { get; private set; }
    public CharacterExperience experience { get; private set; }

    private void Awake()
    {
        if (type == EntityType.Character)
        {
            Health = GetComponent<CharacterHealth>();
            Stats = GetComponent<CharacterStats>();
            StatusManager = GetComponent<StatusManager>();
            experience = GetComponent<CharacterExperience>();
        }

        if(type==EntityType.Object)
        {
            Health = GetComponent<ObjectHealth>();
        }
    }
}
    public enum EntityType
    {
    Character,
    Object
    }

