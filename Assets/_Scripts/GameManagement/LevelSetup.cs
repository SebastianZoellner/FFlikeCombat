using System;
using System.Collections;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public static event Action<int> OnNewStage = delegate { };
    //public event Action OnWaveDefeated = delegate { };
   
    public static event Action OnGameInitialized = delegate { };
    public static event Action<LevelSO> StartMissionBriefing; 

    [SerializeField] LevelSO level;

    [SerializeField] CharacterManager characterManager;
    [SerializeField] GameSetupSO levelSetup;
    private GameEnd gameEnd;
    private ManageStage stageManager;
    private Spawner spawner;

    private SceneLoader sceneLoader; 

    private int stageIndex = 0;

    private void Awake()
    {
        gameEnd = GetComponent<GameEnd>();
        sceneLoader = GetComponent<SceneLoader>();
        spawner = GetComponent<Spawner>();
        stageManager = GetComponent<ManageStage>();

        level = levelSetup.levelList[0];

        sceneLoader.LoadScene(level.sceneName,true);
        sceneLoader.LoadScene("UI",true);
    }

    private void OnEnable()
    {
        //ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
        sceneLoader.OnSceneLoaded += SceneLoader_OnSceneLoaded;

        //characterManager.OnEnemiesDead += CharacterManager_OnEnemiesDead;
        stageManager.OnStageDefeated += StageManager_OnStageDefeated;
        GameMenuFunctions.OnMainMenue += GameMenuFunctions_OnMainMenue;
        GameMenuFunctions.OnRestartLevel += GameMenuFunctions_OnRestartLevel;
    }

    private void OnDisable()
    {
        //ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
        sceneLoader.OnSceneLoaded -= SceneLoader_OnSceneLoaded;
        //characterManager.OnEnemiesDead -= CharacterManager_OnEnemiesDead;
        stageManager.OnStageDefeated -= StageManager_OnStageDefeated;
        GameMenuFunctions.OnMainMenue -= GameMenuFunctions_OnMainMenue;
        GameMenuFunctions.OnRestartLevel -= GameMenuFunctions_OnRestartLevel;
    } 

    //---------------------------------------------------------------------
    //            Private Methods
    //---------------------------------------------------------------------
    private void SceneLoader_OnSceneLoaded(string sceneName)
    {
        if (sceneName == "UI")
        {
            Debug.Log("UI loaded");
            StartMissionBriefing.Invoke(level);
        }

        if (sceneName == level.sceneName)
        {
            sceneLoader.SetActiveScene(sceneName);
            OnGameInitialized.Invoke();
            spawner.SetHeroList(levelSetup.characterList.ToArray());
            stageManager.NextStage(level.GetStage(stageIndex),stageIndex);
        }
    }

    private void StageManager_OnStageDefeated(int defeatedStageIndex)
    {
        stageIndex=defeatedStageIndex+1;

        if (stageIndex == level.GetNumberOfStages())
        {
            gameEnd.WinGame();
            return;
        }

        StartCoroutine(SwitchStage());
    }

    private IEnumerator SwitchStage()
    {
        Debug.Log("New stage " + stageIndex);
        yield return new WaitForSeconds(2);
        
        OnNewStage.Invoke(stageIndex);
        stageManager.NextStage(level.GetStage(stageIndex),stageIndex);
    }

    private void GameMenuFunctions_OnMainMenue()
    {
        Debug.Log("<color=yellow>Returning to Main Menue</color>");
        sceneLoader.UnloadScene("UI");
        sceneLoader.UnloadScene(level.sceneName);
        sceneLoader.LoadScene("StartScreen",false);
    }

    private void GameMenuFunctions_OnRestartLevel()
    {
        Debug.Log("<color=yellow>Restarting Level</color>");
        sceneLoader.UnloadScene(level.sceneName);
        stageIndex = 0;
        sceneLoader.LoadScene(level.sceneName, true);         
    }

   
}
