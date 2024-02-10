using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Game Elements/Characters")]

public class CharacterSO : ScriptableObject
{
    public string CharacterName;
   public PowerSO[] powerArray;
    public float startingHealth;
    [SerializeField] AttributeValue[] attributeArray;
    private Dictionary<Attribute, float> baseAttribute = null;
    public Sprite icon;

    public float GetBaseAttribute(Attribute attribute)
    {
        if (baseAttribute == null)
            InitializeBaseAttribute();

        return baseAttribute[attribute];

    }

   

    public bool HasPowerID(int powerId)
    {
        return powerId<powerArray.Length;
    }

    public PowerSO GetPower(int powerId)
    {
        if (HasPowerID(powerId))
            return powerArray[powerId];
        else 
            return null;
    } 
    
    private void InitializeBaseAttribute()
    {
        baseAttribute = new Dictionary<Attribute, float>();
        foreach (Attribute att in System.Enum.GetValues(typeof(Attribute)))
            baseAttribute[att] = 0;

        foreach (AttributeValue av in attributeArray)
            baseAttribute[av.attribute] = av.value;

    }
}
