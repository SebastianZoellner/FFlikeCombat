using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [field: SerializeField] public SpawnPointType type { get; private set; } = SpawnPointType.Enemy;
    [field: SerializeField] public int Stage { get; private set; }

    [SerializeField] Transform combatLocation;

    private bool isEmptied=false;

    public bool IsFull()
    {
        if(isEmptied)
        {
            isEmptied = false;
            return false;
        }

        IDamageable character = GetComponentInChildren<IDamageable>();
        if (character != null)
        {
            Debug.Log("Spawn point " + name + " holds " + character.GetTransform().name);
            return true;
        }

        return false;
    }

    public Transform GetCombatLocation()
    {
        if (combatLocation)
            return combatLocation;

        return transform;
    }
    public void EmptyPoint()
    {
        IDamageable character = GetComponentInChildren<IDamageable>();
        if (character != null)
        {
            Destroy(character.GetTransform().gameObject);
            isEmptied = true;
        }
    }
    
   
}

