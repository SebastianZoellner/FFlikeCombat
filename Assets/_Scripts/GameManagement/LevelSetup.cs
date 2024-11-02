using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public static event Action<int> OnNewStage = delegate { };
    public event Action OnWaveDefeated = delegate { };
   
    public static event Action OnGameInitialized = delegate { };
    public static event Action<LevelSO> StartMissionBriefing; 

    [SerializeField] LevelSO level;

    private List<GameObject> spawnedEnemies;

    [SerializeField] CharacterManager characterManager;
    [SerializeField] GameSetupSO levelSetup;
    private GameEnd gameEnd;

    private AudioManager audioManager;
    private SceneLoader sceneLoader; 

    private int stage = 0;
    private bool allEnemiesSpawned;

    private void Awake()
    {
        gameEnd = GetComponent<GameEnd>();
        audioManager = GetComponent<AudioManager>();
        sceneLoader = GetComponent<SceneLoader>();
        level = levelSetup.levelList[0];

        sceneLoader.LoadScene(level.sceneName,true);
        sceneLoader.LoadScene("UI",true);
    }

    private void OnEnable()
    {
        ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
        sceneLoader.OnSceneLoaded += SceneLoader_OnSceneLoaded;
        
        characterManager.OnEnemiesDead += CharacterManager_OnEnemiesDead;

        GameMenuFunctions.OnMainMenue += GameMenuFunctions_OnMainMenue;
        GameMenuFunctions.OnRestartLevel += GameMenuFunctions_OnRestartLevel;
    }


    private void OnDisable()
    {
        ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
        sceneLoader.OnSceneLoaded += SceneLoader_OnSceneLoaded;
        characterManager.OnEnemiesDead -= CharacterManager_OnEnemiesDead;
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
            OnGameInitialized.Invoke();
            SetupStage(stage);
        }
    }

    private void SpawnHeroes()
    {
        foreach (CharacterSO ch in levelSetup.characterList)
        {
            //Debug.Log("Spawning " + ch.CharacterName);
            SpawnPoint spawnPoint = SpawnPointController.Instance.GetEmptySpawnPoint(SpawnPointType.Hero);
            if (!spawnPoint)
                Debug.LogWarning("No Hero Spawn Point Found");
            //Debug.Log(spawnPoint.name);

            GameObject newHero=ch.Spawn(spawnPoint.transform);

            if (!newHero)
                Debug.LogError("Spawn Failed: " + ch.name);
            else
                SpawnPointController.Instance.AssignSpawnPoint(spawnPoint, SpawnPointType.Hero);

            PCController hero = newHero.GetComponent<PCController>();
            characterManager.AddHero(hero);
            StartCoroutine(SendToStart(hero.GetComponent<CharacterCombat>()));         
        }
    }

    private void ActionSequencer_OnNewRoundStarted(int round)
    {
        SpawnPointController.Instance.RemoveFallenEnemies();

        if (!allEnemiesSpawned)
            NextWave(round);
    }

    private void NextWave(int round)
    {
        Debug.Log("Spawning stage " + stage + " round(+1) " + round);
        spawnedEnemies =level.SpawnWave(stage,round-1, SpawnPointController.Instance);
      
        foreach(GameObject go in spawnedEnemies)
        {
            EnemyController enemy = go.GetComponent<EnemyController>();
            characterManager.AddEnemy(enemy);

            StartCoroutine(SendToStart(enemy.GetComponent<CharacterCombat>()));
            
        }

        Debug.Log("Spawning Wave " + round);

        if (round == level.GetNumberOfWaves(stage))
        {
            allEnemiesSpawned = true;
            Debug.Log("All enemies spawned for this stage");
        }

    }

    private void SetupStage(int stage)
    {
        Debug.Log("Settingup stage " + stage);
        SpawnPointController.Instance.SetupStage(stage);
        audioManager.PlayMusic(level.GetMusic(stage));
        audioManager.PlayAmbiance(level.GetAmbience(stage));

        if(stage==0)
            SpawnHeroes();
        else
        {
            foreach(PCController hero in characterManager.heroList)
            {
                SpawnPoint spawnPoint = SpawnPointController.Instance.GetEmptySpawnPoint(SpawnPointType.Hero);
                SpawnPointController.Instance.AssignSpawnPoint(spawnPoint, SpawnPointType.Hero);

                hero.transform.SetParent(spawnPoint.transform);
                hero.GetComponent<CharacterMover>().SetNewCombatLocation();
                hero.GetComponent<CharacterCombat>().StartMoveHome();
            }
        }
    }

    private void CharacterManager_OnEnemiesDead()
    {
        if (allEnemiesSpawned)
        {
            ++stage;

            if (stage == level.GetNumberOfStages())
            {
                gameEnd.WinGame();
                return;
            }

            StartCoroutine(SwitchStage(stage));            
        }
        
    }

    private IEnumerator SendToStart(CharacterCombat character)
    {

        CharacterMover mover = character.GetComponent<CharacterMover>();
        yield return null; 

        while(!character.Initialized || !mover.Initialized || !character.GetComponent<CharacterAnimator>().Initialized)
            yield return null;

        mover.SetNewCombatLocation();
        character.StartMoveHome();
    }

    private IEnumerator SwitchStage(int newStage)
    {
        Debug.Log("New stage " + newStage);
        yield return new WaitForSeconds(2);
        allEnemiesSpawned = false;
        audioManager.StopAll();
        //CameraController.Instance.SwitchCamera(newStage);
        OnNewStage.Invoke(newStage);
        SetupStage(newStage);
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
        stage = 0;
        sceneLoader.LoadScene(level.sceneName, true);
             
    }
}
