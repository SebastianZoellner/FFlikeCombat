using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVFX : MonoBehaviour
{
    public void AttackingEffect(GameObject effect, Transform target)
    {
        GameObject newEffect;
        Vector3 targetPosition = target.position + Vector3.up;
        Ray ray = new Ray(transform.position, targetPosition - transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit))
        {
            newEffect=Instantiate(effect, hit.point, Quaternion.identity);
            Destroy(newEffect, 2);
        }
    }
}
