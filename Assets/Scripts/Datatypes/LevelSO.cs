using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Game Elements/Levels")]


public class LevelSO : ScriptableObject
{
    [SerializeField] Wave[] waveArray;
    public int lastWave;

    [BoxGroup("Audio")]
    public AudioClip ambience;
    [BoxGroup("Audio")]
    public AudioClip[] musicArray;




    public List<GameObject> SpawnWave(int turn, SpawnPointController spawnPointController)
    {
        List<GameObject> newEnemies = new List<GameObject>();
        //Debug.Log("New Wave, number of Spawn Groups: " + waveArray[turn].spawnGroupArray.Length);


        foreach (SpawnGroup sg in waveArray[turn].spawnGroupArray)
        {
            int number = Random.Range(sg.minNumber, sg.maxNumber + 1);
            //Debug.Log("Spawning " + number);
            for (int i = 0; i < number; ++i)
            {
                Transform newSpawnPoint = spawnPointController.GetEmptySpawnPoint(SpawnPointType.Enemy);
                GameObject spawnedEnemy = sg.enemy.Spawn(newSpawnPoint);
                if (!spawnedEnemy)
                    Debug.LogError("Spawn Failed");
                newEnemies.Add(spawnedEnemy);
            }
        }
        return newEnemies;
    }

    public int GetNumberOfWaves()=> waveArray.Length;
}
[System.Serializable]
public struct Wave
{
    public SpawnGroup[] spawnGroupArray;
}

[System.Serializable]
public struct SpawnGroup
{
    public CharacterSO enemy;
    public int minNumber;
    public int maxNumber;
}