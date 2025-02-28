using DamageNumbersPro;
using System;
using System.Collections.Generic;
using TrailsFX;
using UnityEngine;

public class CharacterVFX : MonoBehaviour
{
    [SerializeField] private Transform statusEffectTransform;
    [SerializeField] private TrailPoint[] trailPointArray;

    [Header("Main Particle systems")]
    [SerializeField] private ParticleSystem invigorateEffect;
    [SerializeField] private ParticleSystem levelUpEffect;
    [SerializeField] private ParticleSystem tacticalAdvantageEffect;

    [Header("Damage Number Setup")]
    [SerializeField] DamageNumber damagePrefab;
    [SerializeField] DamageNumber healPrefab;
    [SerializeField] DamageNumber textPrefab;

    private TrailOrigin[] attackOrigin;
    private Dictionary<TrailOrigin,TrailEffect> trailEffect;

    private CharacterHealth health;
    private void Awake()
    {
        InitializeTrailEffectDictionary();
        health = GetComponent<CharacterHealth>();    } 

    private void OnEnable()
    {
        health.OnInvigorate += health_OnInvigorate;
        if(health.IsHero)
        GetComponent<CharacterExperience>().OnLevelUp += CharacterVFX_OnLevelUp;
    }

private void Start()
    {
        DisableTrail();
    }

    private void OnDisable()
    {
        health.OnInvigorate -= health_OnInvigorate;
        if(health.IsHero)
        GetComponent<CharacterExperience>().OnLevelUp -= CharacterVFX_OnLevelUp;
    }
    //-----------------------------------------------------------------------------------
    //                    Public functions
    //-----------------------------------------------------------------------------------
    public void SpawnDamageText(float damage) => healPrefab.Spawn(transform.position + 2 * Vector3.up, damage);
    public void SpawnHealText(float heal) => damagePrefab.Spawn(transform.position + 2 * Vector3.up, heal);
    public void SpawnFloatingText(string text) => textPrefab.Spawn(transform.position + 2 * Vector3.up, text);

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

    public void MissEffect(GameObject missEffect, Transform target)
    {
        AttackingEffect(missEffect, target);
        textPrefab.Spawn(target.position + 2 * Vector3.up, "Missed");
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
        DisableTrail();
    }

    public void EnableTrail() //Called from anmation events
    {
        foreach (TrailOrigin to in attackOrigin)
        {
            if (trailEffect.ContainsKey(to))
                trailEffect[to].active = true;
            else
                Debug.Log(name + " does not have a trail set for origin " + to, gameObject);
        }
    }
    public void DisableTrail()
    {
        foreach (TrailPoint tp in trailPointArray)
            tp.trail.active = false;
    }

    //------------------------------------------------------------------------
    //                  Private functions
    //------------------------------------------------------------------------

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

    
    private void CharacterVFX_OnLevelUp()
    {
        if (levelUpEffect)
            levelUpEffect.Play();

        SpawnFloatingText("Level up!");

    }

}

[Serializable]
public struct TrailPoint
{
    public TrailOrigin origin;
    public TrailEffect trail;
}
