using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "New Character", menuName = "Game Elements/Characters")]

public class CharacterSO : ScriptableObject
{
    public string CharacterName;
    public string LabelName;
    public Faction fraction;
   public PowerSO[] powerArray;
    public float startingHealth;
    [SerializeField] AttributeValue[] attributeArray;
    private Dictionary<Attribute, float> baseAttribute = null;
    [PreviewField(75)]
    public Sprite icon;
    [TextArea]
    public string shortBlurb;
    public AnimatorOverrideController animatorController;
    [PreviewField(100)]
    [SerializeField] GameObject characterToon;
    [Range(0,10)]
    public int level;
    public float StartingEndurance;

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
    public GameObject Spawn(Transform spawnPoint)
    {
        return Instantiate(characterToon, spawnPoint);
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
