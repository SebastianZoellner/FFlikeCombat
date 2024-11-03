using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageStage : MonoBehaviour
{
    public event Action <int>OnStageDefeated = delegate { };

    [SerializeField] CharacterManager characterManager;
    private AudioManager audioManager;
    private Spawner spawner;

    [ShowInInspector] private bool allEnemiesSpawned;
    [ShowInInspector] private int round;
    private Stage activeStage;
    [ShowInInspector] private int activeStageIndex;

   

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        spawner = GetComponent<Spawner>();
    }

    private void OnEnable()
    {
        characterManager.OnEnemiesDead += CharacterManager_OnEnemiesDead;
        ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
    }

    private void OnDisable()
    {
        characterManager.OnEnemiesDead -= CharacterManager_OnEnemiesDead;
        ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
    }

    public void NextStage(Stage stage, int index)
    {
        CleanupStage(activeStage);
        Debug.Log("<color=yellow> Starting Stage " + index + "</color>");
        activeStage = stage;
        activeStageIndex = index;
        SetupStage(activeStage, index);
        MoveHeroes(index);
    }

    private void CleanupStage(Stage activeStage)
    {
        if (activeStage.waveArray == null)
            return;
    }

    private void SetupStage(Stage stage, int index)
    {
        Debug.Log("Settingup stage " + index);
        SpawnPointController.Instance.SetupStage(index);
        audioManager.PlayMusic(stage.music);
        audioManager.PlayAmbiance(stage.ambience);
        allEnemiesSpawned = false;
        round = 0;

        if(index==0)
            spawner.SpawnHeroes();
    }

    private void MoveHeroes(int index)
    {
        if (index == 0)
            return;

        foreach (PCController hero in characterManager.heroList)
        {
            SpawnPoint spawnPoint = SpawnPointController.Instance.GetEmptySpawnPoint(SpawnPointType.Hero);
            SpawnPointController.Instance.AssignSpawnPoint(spawnPoint, SpawnPointType.Hero);

            hero.transform.SetParent(spawnPoint.transform);
            hero.GetComponent<CharacterMover>().SetNewCombatLocation();
            hero.GetComponent<CharacterCombat>().StartMoveHome();
        }
    }

    private void ActionSequencer_OnNewRoundStarted(int round)
    {
        SpawnPointController.Instance.RemoveFallenEnemies();

        if (!allEnemiesSpawned)
            NextWave(round-1);
    }

    private void NextWave(int round)
    {
        Debug.Log("Spawning stage " + activeStageIndex + " round(+1) " + round);

        if (round < activeStage.waveArray.Length)
            spawner.SpawnWave(activeStage.waveArray[round]);

        if (round == activeStage.waveArray.Length-1)
        {
            allEnemiesSpawned = true;
            Debug.Log("All enemies spawned for this stage");
        }
    }

    private void CharacterManager_OnEnemiesDead()
    {
        if (allEnemiesSpawned)
        {
            Debug.Log("<color=yellow> Ending Stage " + activeStageIndex + "</color>");
            audioManager.StopAll();
            OnStageDefeated.Invoke(activeStageIndex);
        }
    }
}
