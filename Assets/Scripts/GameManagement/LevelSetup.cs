using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSetup : MonoBehaviour
{
    [SerializeField] LevelSO level;
    private List<GameObject> spawnedEnemies;
    [SerializeField] CharacterManager characterManager;
    [SerializeField] CharacterSO[] heroArray;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        
    }
    private void OnEnable()
    {
        ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
    }

    private void OnDisable()
    {
        ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
    }

    private void Start()
    {
        SpawnHeroes();
        audioManager.PlayMusic(level.musicArray);
        audioManager.PlayAmbiance(level.ambience);
    }

    private void SpawnHeroes()
    {
        foreach (CharacterSO ch in heroArray)
        {
            Transform spawnPoint = SpawnPointController.Instance.GetEmptySpawnPoint(SpawnPointType.Hero);
            GameObject newHero=ch.Spawn(spawnPoint);
            if (!newHero)
                Debug.LogError("Spawn Failed: "+ch.name);
            PCController hero = newHero.GetComponent<PCController>();
            characterManager.AddHero(hero);
        }
    }

    private void ActionSequencer_OnNewRoundStarted(int round)
    {
        if (round - 1 < level.GetNumberOfWaves())
        {
            NextWave(round);
        }
        else
            characterManager.SetAllEnemiesSpawned();
    }

    private void NextWave(int round)
    {
        Debug.Log("Spawning Wave " + round);

        spawnedEnemies =level.SpawnWave(round-1, SpawnPointController.Instance);
      
        foreach(GameObject go in spawnedEnemies)
        {
            EnemyController enemy = go.GetComponent<EnemyController>();
            characterManager.AddEnemy(enemy);
        }
    }

}
