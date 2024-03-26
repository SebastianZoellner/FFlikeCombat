using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVFX : MonoBehaviour
{
    [SerializeField] private Transform statusEffectTransform;
    [SerializeField] private ParticleSystem invigorateEffect;
    [SerializeField] private ParticleSystem tacticalAdvantageEffect;

    private void OnEnable()
    {
        GetComponent<CharacterHealth>().OnInvigorate += health_OnInvigorate;
    }

    private void OnDisable()
    {
        GetComponent<CharacterHealth>().OnInvigorate -= health_OnInvigorate;
    }

    public void AttackingEffect(GameObject effect, Transform target)
    {       
        Vector3 targetPosition = target.position + (1.5f*Vector3.up);
        Ray ray = new Ray(transform.position+0.1f*Vector3.up, targetPosition - transform.position);
        
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit))
        {
            //Debug.Log("Hit at  " + hit.point);
            CreateEffect(effect, hit.point);
        }
    }

    public void BuffingEffect(GameObject effect)
    {
        //Debug.Log("Instantiating Buffing effect");
        GameObject newEffect = Instantiate(effect, transform);
        Destroy(newEffect, 2);
    }

    public GameObject InitializeStatusVFX(GameObject statusVFX)
    {
        return Instantiate(statusVFX, statusEffectTransform);
    }

    private void CreateEffect(GameObject effect, Vector3 position)
    {
        GameObject newEffect = Instantiate(effect, position, Quaternion.identity);
        Destroy(newEffect, 2);
    }

    private void health_OnInvigorate()
    {
        if(invigorateEffect)
        invigorateEffect.Play();
    }

    public void StartTacticalAdvantage()
    {
        if (tacticalAdvantageEffect)
            tacticalAdvantageEffect.Play();
    }
}
