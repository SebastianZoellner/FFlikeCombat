using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;
    private List<ObjectController> objectList;

    private void Awake()
    {
        if(Instance)
        {
            Debug.LogError("Multiple Copies of ObjectManager!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddObject(ObjectController newObject)
    {
        if (objectList == null)
            objectList = new List<ObjectController>();

        objectList.Add(newObject);
    }

    public void RemoveObject(ObjectController removedObject)
    {
        if (objectList != null)
            objectList.Remove(removedObject);
    }

    public float GetAttributeModifier(Attribute attribute, Faction fraction)
    {
        float modifier = 0;
        if (objectList == null)
            return 0;
        foreach (ObjectController oc in objectList)
        {
            modifier += oc.GetModifier(attribute,fraction);
        }
        return modifier;
    }
}
