using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] private HeroTeamSO availableHeros;
    [SerializeField] private HeroTeamSO chosenTeam;
    [SerializeField] private Slider progressBar;
    [SerializeField] private UIFader fader;

    private int maxHeroes=2;

    bool isLoading = false;
    AsyncOperation loadingOperation;

    private void Awake()
    {
        chosenTeam.characterList = new List<CharacterSO>();
    }

    private void Update()
    {
        if (!isLoading)
            return;
        //Debug.Log("Loading progress " + loadingOperation.progress);
        progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
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
        
        if (chosenTeam.characterList.Count > 0)
        {
            Debug.Log("Starting Game");
            fader.FadeOut();
            loadingOperation = SceneManager.LoadSceneAsync(1);
            isLoading = true;
        }


    }
}
