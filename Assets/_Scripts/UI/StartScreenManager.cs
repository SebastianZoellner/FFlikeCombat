using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    public event Action<LevelSO> OnMissionSelected = delegate { };
    public event Action OnHeroesCleared = delegate { };

    [SerializeField] private GameSetupSO availableHeros;
    [SerializeField] private GameSetupSO chosenTeam;
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

    public LevelSO[] GetAvailableMissions()
    {
        return availableHeros.levelList.ToArray();
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

    public void PressMissionSelectButton(LevelSO level)
    {
        chosenTeam.levelList.Clear();
        chosenTeam.levelList.Add(level);
        maxHeroes = level.numberOfHeroes;
        if (chosenTeam.characterList.Count > level.numberOfHeroes)
        {
            chosenTeam.characterList.Clear();
            OnHeroesCleared.Invoke();
        }
        Debug.Log("Chose " + level.name);
        OnMissionSelected.Invoke(level);
    }


    public void StartGame()
    {
        
        if (chosenTeam.characterList.Count > 0 && chosenTeam.levelList.Count==1 )
        {
            Debug.Log("Starting Game");
            Debug.Log("Level " + chosenTeam.levelList[0].LevelName);
            fader.FadeOut();
            loadingOperation = SceneManager.LoadSceneAsync(1);
            isLoading = true;
        }


    }
}
