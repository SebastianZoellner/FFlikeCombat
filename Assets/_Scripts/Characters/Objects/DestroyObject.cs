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
        destroyEffect.SetActive(false);
    }
    private void OnDisable()
    {
        if (health)
            health.OnObjectDestroyed -= Health_OnObjectDestroyed;
    }

    private void Health_OnObjectDestroyed()
    {
        visual.SetActive(false);
        destroyEffect.SetActive(true);
        Destroy(gameObject, 5);
    }
}
