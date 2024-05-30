using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Team", menuName = "Game Elements/HeroTeam")]

public class HeroTeamSO : ScriptableObject
{
    public List<CharacterSO> characterList;   
}
