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
    public PowerSO[] leveledPowerArray;
   // public float startingHealth;
    [SerializeField] AttributeValue[] attributeBonusArray;
    private Dictionary<Attribute, float> baseAttribute = null;
    [PreviewField(75)]
    public Sprite icon;
    [TextArea]
    public string shortBlurb;
    [TextArea]
    public string blurb;
    public AnimatorOverrideController animatorController;
    [PreviewField(100)]
    [SerializeField] GameObject characterToon;
    [Range(0,10)]
    public int rank;
    //public float StartingEndurance;
    public CharacterClass characterClass;
    [SerializeField] StatProgressionSO statProgression;
    [Range (0,1)]
    [SerializeField] float impactToPower=0.5f;
    [Range(0, 1)]
    [SerializeField] float celerityToSpeed=0.5f;
    [SerializeField] float setHardiness = 0;
    [SerializeField] float setRecovery = 20;
    public float xpValue { get; private set; }

    public float GetBaseAttribute(Attribute attribute)
    {
        if (baseAttribute == null)
            SetAttributes();

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
        if (!spawnPoint)
        {
            Debug.LogError("Spawn point not defined for " + CharacterName);
            return null;
        }

        var newCharacter= Instantiate(characterToon, spawnPoint);
        if (newCharacter == null)
            Debug.LogError(CharacterName + "not spawned");

        return newCharacter;
    }

/*
    private void InitializeBaseAttribute()
    {
        baseAttribute = new Dictionary<Attribute, float>();
        foreach (Attribute att in System.Enum.GetValues(typeof(Attribute)))
            baseAttribute[att] = 0;

        foreach (AttributeValue av in attributeBonusArray)
            baseAttribute[av.attribute] = av.value;

    }
*/
    private void SetAttributes()
    {
        baseAttribute = new Dictionary<Attribute, float>();
        foreach (Attribute att in System.Enum.GetValues(typeof(Attribute)))
            baseAttribute[att] = 0;


        baseAttribute[Attribute.Agillity] = statProgression.GetStat(Attribute.Agillity, characterClass, rank);
        baseAttribute[Attribute.Combat] = statProgression.GetStat(Attribute.Combat, characterClass, rank);
        baseAttribute[Attribute.Armor] = statProgression.GetStat(Attribute.Armor, characterClass, rank);

        float impact = statProgression.GetStat(Attribute.Impact, characterClass, rank);
        baseAttribute[Attribute.Power] = impactToPower * impact;
        float celerity = 2 * (1 - impactToPower) * impact;
        baseAttribute[Attribute.Speed] = celerityToSpeed * celerity;
        baseAttribute[Attribute.Initiative] = (1 - celerityToSpeed) * celerity;
        float health = statProgression.GetStat(Attribute.BaseHealth, characterClass, rank);
        float hardinessWeight = 4;
        float maxHpInHardiness = 0.5f;
        if (setHardiness * hardinessWeight <= maxHpInHardiness * health)
        {
            baseAttribute[Attribute.Hardiness] = setHardiness;
            baseAttribute[Attribute.BaseHealth] = health - (hardinessWeight * setHardiness);
        }
        else
            Debug.LogWarning("Hardiness set too high for " + CharacterName);

        float baseEndurance = statProgression.GetStat(Attribute.BaseEndurance, characterClass, rank);
        float recoveryWeight = 4;
        float maxEnduranceInRecovery = 0.5f;

        if (setRecovery * recoveryWeight <= maxEnduranceInRecovery * baseEndurance)
        {
            baseAttribute[Attribute.BaseEndurance] = setRecovery;
            baseAttribute[Attribute.BaseEndurance] = baseEndurance - (recoveryWeight * setRecovery);
        }


        foreach (AttributeValue av in attributeBonusArray)
            baseAttribute[av.attribute] += av.value;
        xpValue=CalculateXPValue(rank);

    }

    private float CalculateXPValue(int rank)//so far, this really only works through level 4
    {
        float baseXpValue = 2;
        xpValue = baseXpValue + rank;
        if (rank > 3)
            xpValue += (rank - 3);
        if(rank>5)
            xpValue += (rank - 5)*2;
        if (rank > 7)
            xpValue += (rank - 7) * 4;

        return xpValue;
    }
}
