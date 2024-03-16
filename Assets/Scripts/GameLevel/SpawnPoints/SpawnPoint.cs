using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [field: SerializeField] public SpawnPointType type { get; private set; } = SpawnPointType.Enemy;
    [field: SerializeField] public int Stage {get; private set;}


    [SerializeField] Transform combatLocation;
    

    public bool IsFull()
    {
        CharacterHealth character = GetComponentInChildren<CharacterHealth>();
        if (character)
            return true;

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
        CharacterHealth character = GetComponentInChildren<CharacterHealth>();
        if (character)
            Destroy(character.gameObject);
    }
    
   
}

