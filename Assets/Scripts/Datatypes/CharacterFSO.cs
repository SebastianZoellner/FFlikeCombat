using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "New Character", menuName = "Game Elements/New Character")]

public class CharacterFSO : ScriptableObject
{
    //Data fields
    public string CharacterName;
    [Range(0, 10)]
    public int level;

    [BoxGroup("Stats")]
    public float startingHealth;
    [BoxGroup("Stats")]
    public float StartingEndurance;
    [BoxGroup("Stats")]
    [SerializeField] AttributeValue[] attributeArray;
    [BoxGroup("Stats")]
    public PowerSO[] powerArray;
    [BoxGroup("Stats")]
  
    [HorizontalGroup("Visual",100)]
    [PreviewField(75)]
    [HideLabel]
    public Sprite icon;

    [VerticalGroup("Visual/Toon")]
    [PreviewField(100)]
    [HideLabel]
    [SerializeField] GameObject characterToon;

     public AnimatorOverrideController animatorController;

   //Internal fields
    private Dictionary<Attribute, float> baseAttribute = null;


    public float GetBaseAttribute(Attribute attribute)
    {
        if (baseAttribute == null)
            InitializeBaseAttribute();

        return baseAttribute[attribute];
    }

    public bool HasPowerID(int powerId)
    {
        return powerId < powerArray.Length;
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
