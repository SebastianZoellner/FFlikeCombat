using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Game Elements/Enemy Wave")]

public class EnemyWaveSO : ScriptableObject
{
    public SpawnGroup[] spawnGroupArray;

    public List<GameObject> SpawnWave(SpawnPointController spawnPointController)
    {
        List<GameObject> newEnemies = new List<GameObject>();
        //Debug.Log("New Wave, number of Spawn Groups: " + waveArray[turn].spawnGroupArray.Length);


        foreach (SpawnGroup sg in spawnGroupArray)
        {
            int number = Random.Range(sg.minNumber, sg.maxNumber + 1);
            //Debug.Log("Spawning " + number);
            for (int i = 0; i < number; ++i)
            {
                SpawnPoint newSpawnPoint = spawnPointController.GetEmptySpawnPoint(SpawnPointType.Enemy);
                if (newSpawnPoint.IsFull())
                {
                    Debug.LogWarning("Returned empty spawn point not empty");
                    continue;
                }
                GameObject spawnedEnemy = sg.enemy.Spawn(newSpawnPoint.transform);
                if (!spawnedEnemy)
                    Debug.LogError("Spawn Failed");
                newEnemies.Add(spawnedEnemy);
            }
        }
        return newEnemies;
    }
}

[System.Serializable]
public struct SpawnGroup
{
    public CharacterSO enemy;
    public int minNumber;
    public int maxNumber;
}

