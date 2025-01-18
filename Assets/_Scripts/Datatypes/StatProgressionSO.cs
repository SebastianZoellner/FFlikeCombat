using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="StatProgression",menuName="GameElements/StatProgression")]

public class StatProgressionSO : ScriptableObject
{
    [SerializeField] CharacterClassProgression[] characterClassArray;

    private Dictionary<CharacterClass,Dictionary<Attribute,float[]>> lookupTable=null; 

    private void BuildLookupTable()
    {
        if (lookupTable != null)
            return;

        lookupTable = new Dictionary<CharacterClass, Dictionary<Attribute, float[]>>();

        foreach(CharacterClassProgression ccProgression in characterClassArray)
        {
            var statLookupTable = new Dictionary<Attribute, float[]>();
            foreach(StatProgression statProgression in ccProgression.statByLevelArray)
            {
                statLookupTable[statProgression.stat] = statProgression.valueByLevel;
            }
            lookupTable[ccProgression.characterClass] = statLookupTable;

        }
    }

    public float GetStat(Attribute stat, CharacterClass characterClass, int level)
    {
        if (lookupTable == null)
            BuildLookupTable();

        //Here we want a check that this only finds attributes that are part of level progression


        if(!lookupTable[characterClass].ContainsKey(stat))
        {
            Debug.Log("Request for " + stat + "; Not in lookup table");//Right now this should only capture Armor
            return 0;
        }

        float[] valuesByLevel = lookupTable[characterClass][stat];

        if (valuesByLevel.Length <= level)
        {
            Debug.LogError("Impossible Data Request in GetStat, Values only defined to level " + lookupTable[characterClass][stat].Length + " requested level " + level);
            return 0;
        }

        return valuesByLevel[level];
    }
        


    [System.Serializable]
    class CharacterClassProgression
    {
        public CharacterClass characterClass;
        public StatProgression[] statByLevelArray;
        
    }
}

public enum CharacterClass
{
Base,
Brute,
Fast,
Armored,
GlasCannon
}

[System.Serializable]
public class StatProgression
{
    public Attribute stat;
    public float[] valueByLevel;
}


