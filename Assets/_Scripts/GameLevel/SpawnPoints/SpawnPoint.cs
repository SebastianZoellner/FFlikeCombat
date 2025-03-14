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

        IDamageable[] characterArray = GetComponentsInChildren<IDamageable>();

        if (characterArray.Length > 1)
            Debug.LogError("More than one IDamageable in spawn point " + name);

        if (characterArray.Length>0)
        {
            Debug.Log("Spawn point " + name + " holds " + characterArray[0].GetTransform().name);
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

