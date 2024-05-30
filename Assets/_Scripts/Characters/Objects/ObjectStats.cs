using UnityEngine;

public class ObjectStats : MonoBehaviour,IStats
{
    [SerializeField] ObjectSO activeObject;

    public string GetName() => activeObject.ObjectName;
    public string GetBlurb() => activeObject.shortBlurb;
    public Sprite GetIcon() => activeObject.icon;
    public float GetAttributeModifier(Attribute attribute, Faction fraction) => activeObject.GetAttributeModifier(attribute, fraction);
    public float GetStartingHealth() => activeObject.StartingHealth;
    public float GetDefenseValue() => activeObject.BaseDefense;
    public float GetArmor() => activeObject.Armor;
    public int GetLevel() => activeObject.level;

    
}