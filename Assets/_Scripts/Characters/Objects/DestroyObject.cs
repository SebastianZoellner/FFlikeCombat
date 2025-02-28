using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    ObjectHealth health;
    [SerializeField] GameObject destroyEffect;
    [SerializeField] GameObject visual;

    private void Awake()
    {
        health = GetComponent<ObjectHealth>();
    }

    private void OnEnable()
    {
        if (health)
            health.OnObjectDestroyed += Health_OnObjectDestroyed;
    }

    private void Start()
    {
        if(destroyEffect)
        destroyEffect.SetActive(false);
    }
    private void OnDisable()
    {
        if (health)
            health.OnObjectDestroyed -= Health_OnObjectDestroyed;
    }

    private void Health_OnObjectDestroyed()
    {
        if(visual)
        visual.SetActive(false);
        if(destroyEffect)
        destroyEffect.SetActive(true);
        Destroy(gameObject, 5);
    }
}
