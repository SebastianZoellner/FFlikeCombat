using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] CharacterManager characterManager;

    private CharacterSO[] characterArray;

    public void SetHeroList(CharacterSO[] heroArray) => characterArray = heroArray;
    

    public void SpawnHeroes()
    {
        foreach (CharacterSO ch in characterArray)
        {
            //Debug.Log("Spawning " + ch.CharacterName);
            SpawnPoint spawnPoint = SpawnPointController.Instance.GetEmptySpawnPoint(SpawnPointType.Hero);
            if (!spawnPoint)
                Debug.LogWarning("No Hero Spawn Point Found");
            //Debug.Log(spawnPoint.name);

            GameObject newHero = ch.Spawn(spawnPoint.transform);

            if (!newHero)
                Debug.LogError("Spawn Failed: " + ch.name);
            else
                SpawnPointController.Instance.AssignSpawnPoint(spawnPoint, SpawnPointType.Hero);

            PCController hero = newHero.GetComponent<PCController>();
            characterManager.AddHero(hero);
            StartCoroutine(SendToStart(hero.GetComponent<CharacterCombat>()));
        }

    }

    public void SpawnWave(EnemyWaveSO wave)
    {
        List<GameObject> spawnedEnemies = wave.SpawnWave(SpawnPointController.Instance);

        foreach (GameObject go in spawnedEnemies)
        {
            EnemyController enemy = go.GetComponent<EnemyController>();
            characterManager.AddEnemy(enemy);

            StartCoroutine(SendToStart(enemy.GetComponent<CharacterCombat>()));
        }
    }

    private IEnumerator SendToStart(CharacterCombat character)
    {

        CharacterMover mover = character.GetComponent<CharacterMover>();
        yield return null;

        while (!character.Initialized || !mover.Initialized || !character.GetComponent<CharacterAnimator>().Initialized)
            yield return null;

        mover.SetNewCombatLocation();
        character.StartMoveHome();
    }

   
}
