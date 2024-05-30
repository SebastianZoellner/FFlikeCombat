using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSetup", menuName = "Game Elements/GameSetup")]

public class GameSetupSO : ScriptableObject
{
    public List<CharacterSO> characterList;
    public List<LevelSO> levelList;  
}
