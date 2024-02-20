using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public SpawnPointType type = SpawnPointType.Enemy;
    public float radius = 1f;
    

    public bool IsFull()
    {
        EnemyController enemy = GetComponentInChildren<EnemyController>();
        if (enemy)
            return true;

        return false;
    }

    
    void OnDrawGizmos()
    {
        if(type==SpawnPointType.Enemy)
        Gizmos.color = Color.yellow;
        if (type == SpawnPointType.Hero)
            Gizmos.color = Color.blue;


        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 offset = transform.forward * 0.8f + Vector3.up;
        Vector3 spherePosition = transform.position + offset;

        Gizmos.DrawWireSphere(spherePosition, radius/4);

    }
}

[System.Serializable]
public enum SpawnPointType
{
    Hero,
    Enemy
}
