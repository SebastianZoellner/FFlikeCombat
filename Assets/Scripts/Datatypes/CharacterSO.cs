using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "Game Elements/Characters")]

public class CharacterSO : ScriptableObject
{
    public string CharacterName;
   public PowerSO[] powerArray;
    public float startingHealth;

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
}
