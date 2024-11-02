using UnityEngine;

public class SpawnPointIndicator : MonoBehaviour
{
    [SerializeField] private SpawnPointType type = SpawnPointType.Enemy;
    [SerializeField] private float radius = 1f;
    private const float OffsetMultiplier = 0.8f; // Using a constant for hardcoded value

    void OnDrawGizmos()
    {
        // Set color based on the spawn point type
        switch (type)
        {
            case SpawnPointType.Enemy:
                Gizmos.color = Color.yellow;
                break;
            case SpawnPointType.Hero:
                Gizmos.color = Color.blue;
                break;
            default:
                Gizmos.color = Color.white; // Default color if needed
                break;
        }

        // Draw main wire sphere representing the spawn point
        Gizmos.DrawWireSphere(transform.position, radius);

        // Draw secondary wire sphere with an offset
        Vector3 offset = transform.forward * OffsetMultiplier + Vector3.up;
        Vector3 spherePosition = transform.position + offset;
        Gizmos.DrawWireSphere(spherePosition, radius / 4);
    }
}

[System.Serializable]
public enum SpawnPointType
{
    Hero,
    Enemy
}

