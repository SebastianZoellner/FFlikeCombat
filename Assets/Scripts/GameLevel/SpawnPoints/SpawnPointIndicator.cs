using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointIndicator : MonoBehaviour
{
    [SerializeField] SpawnPointType type = SpawnPointType.Enemy;
    [SerializeField] float radius = 1f;

    void OnDrawGizmos()
    {
        if (type == SpawnPointType.Enemy)
            Gizmos.color = Color.yellow;
        if (type == SpawnPointType.Hero)
            Gizmos.color = Color.blue;


        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 offset = transform.forward * 0.8f + Vector3.up;
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

