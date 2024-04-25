using System;
using System.Collections;
using System.Collections.Generic;
using TrailsFX;
using UnityEngine;

public class CharacterVFX : MonoBehaviour
{
    [SerializeField] private Transform statusEffectTransform;
    [SerializeField] private TrailPoint[] trailPointArray;
    [SerializeField] private ParticleSystem invigorateEffect;
    [SerializeField] private ParticleSystem tacticalAdvantageEffect;

    TrailOrigin[] attackOrigin;

    private Dictionary<TrailOrigin,TrailEffect> trailEffect;

    private void Awake()
    {
        InitializeTrailEffectDictionary();  
    }

   

    private void OnEnable()
    {
        GetComponent<CharacterHealth>().OnInvigorate += health_OnInvigorate;
    }

    private void OnDisable()
    {
        GetComponent<CharacterHealth>().OnInvigorate -= health_OnInvigorate;
    }

    private void Start()
    {
        StopTrail();
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

   

    public void StartTacticalAdvantage()
    {
        if (tacticalAdvantageEffect)
            tacticalAdvantageEffect.Play();
    }

    public void StartAttack(bool ranged, TrailOrigin[] origin)
    {
        attackOrigin = origin;
        //StartTrail();
    }

    public void EndAttack()
    {
        StopTrail();
    }

    private void InitializeTrailEffectDictionary()
    {
        trailEffect = new Dictionary<TrailOrigin, TrailEffect>();
        foreach(TrailPoint tp in trailPointArray)
        {
            trailEffect[tp.origin] = tp.trail;
        }
    }

    private void CreateEffect(GameObject effect, Vector3 position)
    {
        GameObject newEffect = Instantiate(effect, position, Quaternion.identity);
        Destroy(newEffect, 2);
    }

    private void health_OnInvigorate()
    {
        if (invigorateEffect)
            invigorateEffect.Play();
    }

    private void StartTrail() //Called from anmation events
    {
        foreach (TrailOrigin to in attackOrigin)
        {
            if (trailEffect.ContainsKey(to))
                trailEffect[to].active = true;
            else
                Debug.Log(name + " does not have a trail set for origin "+to, gameObject);
        }
    }
    private void StopTrail()
    {
        foreach(TrailPoint tp in trailPointArray)      
        tp.trail.active = false;
    }
}

[Serializable]
public struct TrailPoint
{
    public TrailOrigin origin;
    public TrailEffect trail;
}
