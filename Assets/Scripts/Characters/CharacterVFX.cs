using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVFX : MonoBehaviour
{
    [SerializeField] Transform StatusEffectTransform;
    public void AttackingEffect(GameObject effect, Transform target)
    {
        GameObject newEffect;
        Vector3 targetPosition = target.position + Vector3.up;
        Ray ray = new Ray(transform.position, targetPosition+(0.75f*Vector3.up) - transform.position);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit))
        {
            newEffect=Instantiate(effect, hit.point, Quaternion.identity);
            Destroy(newEffect, 2);
        }
    }

    public GameObject InitializeStatusVFX(GameObject statusVFX)
    {
        return Instantiate(statusVFX, StatusEffectTransform);
    }
}
