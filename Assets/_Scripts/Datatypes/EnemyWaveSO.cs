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
        int testcount=0;

        foreach (SpawnGroup sg in spawnGroupArray)
        {
            int number = Random.Range(sg.minNumber, sg.maxNumber + 1);
            testcount += number;
            Debug.Log("Spawning " + number+" "+sg.enemy.name);
            for (int i = 0; i < number; ++i)
            {
                SpawnPoint newSpawnPoint = spawnPointController.GetEmptySpawnPoint(SpawnPointType.Enemy);
                
                if (!newSpawnPoint)
                {
                    Debug.Log("No new Spawnpoint found");
                    continue;
                }

                if (newSpawnPoint.IsFull())
                {
                    Debug.LogWarning("Returned empty spawn point not empty: "+newSpawnPoint.name);
                    continue;
                }
                Debug.Log("Targeting Spawn point " + newSpawnPoint.name);
                GameObject spawnedEnemy = sg.enemy.Spawn(newSpawnPoint.transform);
                if (!spawnedEnemy)
                    Debug.LogError("Spawn Failed");
                else
                {
                    spawnPointController.AssignSpawnPoint(newSpawnPoint, SpawnPointType.Enemy);
                    Debug.Log("Spawn succeeded; spawn point " + newSpawnPoint.name);
                }
                newEnemies.Add(spawnedEnemy);
            }
        }
        //Debug.Log("Number of spawns attempted: " + testcount);
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

