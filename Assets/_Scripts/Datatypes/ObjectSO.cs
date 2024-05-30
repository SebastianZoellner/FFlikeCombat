using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "New Object", menuName = "Game Elements/Object")]


public class ObjectSO : ScriptableObject
{
    public string ObjectName;
    public string LabelName; 
    public float StartingHealth;
    public float BaseDefense;
    public float Armor;

    [SerializeField] ObjectAttributeModifier[] modifierArray;

    [PreviewField(75)]
    public Sprite icon;
    [TextArea]
    public string shortBlurb;
    
    [PreviewField(100)]
    [SerializeField] GameObject itemToon;
    [Range(0, 10)]
    public int level; //not used right now
    
    public float GetAttributeModifier(Attribute attribute, Faction fraction)
    {
        foreach(ObjectAttributeModifier oam in modifierArray)
        {
            if (oam.attribute == attribute && oam.affectedFraction == fraction)
                return oam.modifier;         //here we can add some level modifier   
        }
        return 0;
    }
}

[System.Serializable]
public struct ObjectAttributeModifier
{
    public Attribute attribute;
    public float modifier;
    public Faction affectedFraction;
}
