using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetup : MonoBehaviour
{
    public static event Action LevelWon = delegate { };
    public static event Action<int> OnNewStage = delegate { };
    public event Action OnWaveDefeated = delegate { };
    public static event Action OnGameWon = delegate { };
    public static event Action OnGameLost = delegate { };

    public static event Action<LevelSO> StartMissionBriefing; 
    [SerializeField] LevelSO level;

    private List<GameObject> spawnedEnemies;

    [SerializeField] CharacterManager characterManager;
    [SerializeField] MomentumManager momentumManager;
    [SerializeField] HeroTeamSO heroTeam;


    private AudioManager audioManager;

    private int stage = 0;
    private bool allEnemiesSpawned;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        
    }
    private void OnEnable()
    {
        ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        characterManager.OnEnemiesDead += CharacterManager_OnEnemiesDead;
        characterManager.OnHeroesDead += LoseGame;
        momentumManager.OnMomentumLoss += LoseGame;
        momentumManager.OnMomentumWin += WinGame;
    }

   

    private void OnDisable()
    {
        ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        characterManager.OnEnemiesDead -= CharacterManager_OnEnemiesDead;
        characterManager.OnHeroesDead -= LoseGame;
        momentumManager.OnMomentumLoss -= LoseGame;
        momentumManager.OnMomentumWin -= WinGame;
    }

    

    private void Start()
    {
        SetupStage(stage);
    }

    //---------------------------------------------------------------------
    //            Private Methods
    //---------------------------------------------------------------------
    private void SpawnHeroes()
    {
        foreach (CharacterSO ch in heroTeam.characterList)
        {
            //Debug.Log("Spawning " + ch.CharacterName);
            SpawnPoint spawnPoint = SpawnPointController.Instance.GetEmptySpawnPoint(SpawnPointType.Hero);
            if (!spawnPoint)
                Debug.LogWarning("No Hero Spawn Point Found");
            Debug.Log(spawnPoint.name);

            GameObject newHero=ch.Spawn(spawnPoint.transform);

            if (!newHero)
                Debug.LogError("Spawn Failed: "+ch.name);
            PCController hero = newHero.GetComponent<PCController>();
            characterManager.AddHero(hero);
            hero.GetComponent<CharacterMover>().SetNewCombatLocation();
            hero.GetComponent<CharacterCombat>().StartMoveHome();
        }
    }

    private void ActionSequencer_OnNewRoundStarted(int round)
    {
       if(!allEnemiesSpawned)
            NextWave(round);
    }

    private void NextWave(int round)
    {     
        SpawnPointController.Instance.RemoveFallenEnemies();

        spawnedEnemies =level.SpawnWave(stage,round-1, SpawnPointController.Instance);
      
        foreach(GameObject go in spawnedEnemies)
        {
            EnemyController enemy = go.GetComponent<EnemyController>();
            characterManager.AddEnemy(enemy);
            enemy.GetComponent<CharacterMover>().SetNewCombatLocation();
            enemy.GetComponent<CharacterCombat>().StartMoveHome();
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
                hero.transform.SetParent(spawnPoint.transform);
                hero.GetComponent<CharacterMover>().SetNewCombatLocation();
                hero.GetComponent<CharacterCombat>().StartMoveHome();
            }

        }
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "UI")
        {
            Debug.Log("UI loaded");
            StartMissionBriefing.Invoke(level);
        }
    }

    private void CharacterManager_OnEnemiesDead()
    {
        if (allEnemiesSpawned)
        {
            ++stage;

            if (stage == level.GetNumberOfStages())
            {
                WinGame();
                return;
            }

            StartCoroutine(SwitchStage(stage));            
        }
        
    }

    private IEnumerator SwitchStage(int newStage)
    {
        Debug.Log("New stage " + newStage);
        yield return new WaitForSeconds(2);
        allEnemiesSpawned = false;
        audioManager.StopAll();
        CameraController.Instance.SwitchCamera(newStage);
        OnNewStage.Invoke(newStage);
        SetupStage(newStage);
    }

    private void LoseGame()
    {
        OnGameLost.Invoke();
    }

    private void WinGame()
    {
        LevelWon.Invoke();
    }
}
