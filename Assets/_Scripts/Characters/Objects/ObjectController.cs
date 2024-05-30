
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private ObjectStats stats;
    private ObjectHealth health;

    private void Awake()
    {
        stats = GetComponent<ObjectStats>();
        health = GetComponent<ObjectHealth>();
    }
    private void OnEnable()
    {
        if (health)
            health.OnObjectDestroyed += Health_OnObjectDestroyed;
    }

    private void Start()
    {
        if(ObjectManager.Instance)
        ObjectManager.Instance.AddObject(this);
    }

    private void OnDisable()
    {
        if (health)
            health.OnObjectDestroyed -= Health_OnObjectDestroyed;
    }

    

    public float GetModifier(Attribute attribute, Faction fraction)
    {
        return stats.GetAttributeModifier(attribute, fraction);
    }

    private void Health_OnObjectDestroyed()
    {
        if (ObjectManager.Instance)
            ObjectManager.Instance.RemoveObject(this);
    }
}
