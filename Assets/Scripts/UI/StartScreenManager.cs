using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] private HeroTeamSO availableHeros;
    [SerializeField] private HeroTeamSO chosenTeam;
    private int maxHeroes=2;

    private void Awake()
    {
        chosenTeam.characterList = new List<CharacterSO>();
    }

    public CharacterSO[] GetAvailableHeroes()
    {
        return availableHeros.characterList.ToArray();
    }


    public bool PressCharacterSelectButton(CharacterSO character)
    {
        
        if (chosenTeam.characterList.Contains(character))
            chosenTeam.characterList.Remove(character);
        else
        {
            if (chosenTeam.characterList.Count == maxHeroes)
                return false;
            chosenTeam.characterList.Add(character);
            Debug.Log("Chose " + character.name);
        }
        return true;
    }

    public void StartGame()
    {
        Debug.Log("Starting Game");
        if (chosenTeam.characterList.Count > 0)
            SceneManager.LoadScene(1);

    }
}
