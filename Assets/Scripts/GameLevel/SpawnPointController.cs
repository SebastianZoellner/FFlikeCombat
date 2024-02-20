using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public static SpawnPointController Instance;

    private SpawnPoint[] allSpawnPointsArray;
    private List<SpawnPoint> emptyEnemySpawnPoints;
    private List<SpawnPoint> fullEnemySpawnPoints;
    private List<SpawnPoint> emptyHeroSpawnPoints;
    private List<SpawnPoint> fullHeroSpawnPoints;

    private void Awake()
    {
        Instance = this;

        allSpawnPointsArray = GetComponentsInChildren<SpawnPoint>();

        InitializeSpawnPointLists();
    }

    private void OnEnable()
    {
        CharacterHealth.OnAnyEnemyDied += OnAnyEnemyDied;
        ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
    }


    private void OnDisable()
    {
        CharacterHealth.OnAnyEnemyDied -= OnAnyEnemyDied;
    }


    public Transform GetEmptySpawnPoint(SpawnPointType type)
    {
        if (type == SpawnPointType.Enemy)
            return GetPoint(ref emptyEnemySpawnPoints, ref fullEnemySpawnPoints);
        return GetPoint(ref emptyHeroSpawnPoints, ref fullHeroSpawnPoints);
    }


   

    public void FreeSpawnPoint(SpawnPoint spawnPoint)
    {
        if (spawnPoint.type == SpawnPointType.Enemy)
            FreePoint(ref emptyEnemySpawnPoints, ref fullEnemySpawnPoints, spawnPoint);
        else
            FreePoint(ref emptyHeroSpawnPoints, ref fullHeroSpawnPoints, spawnPoint);
    }

    

    private void OnAnyEnemyDied(CharacterHealth deadEnemy)
    {
        SpawnPoint spawnPoint = deadEnemy.GetComponentInParent<SpawnPoint>();
        
        FreeSpawnPoint(spawnPoint);
    }

    private void ActionSequencer_OnNewRoundStarted(int obj)
    {
        RemoveFallenEnemies();
    }

    private void InitializeSpawnPointLists()
    {
        emptyEnemySpawnPoints = new List<SpawnPoint>();
        fullEnemySpawnPoints = new List<SpawnPoint>();
        fullHeroSpawnPoints = new List<SpawnPoint>();
        emptyHeroSpawnPoints = new List<SpawnPoint>();

        foreach (SpawnPoint sp in allSpawnPointsArray)
        {

            if (sp.IsFull())
            {
                if (sp.type == SpawnPointType.Enemy)
                    fullEnemySpawnPoints.Add(sp);
                if (sp.type == SpawnPointType.Hero)
                    fullHeroSpawnPoints.Add(sp);
            }
            else
            {
                if (sp.type == SpawnPointType.Enemy)
                    emptyEnemySpawnPoints.Add(sp);
                if (sp.type == SpawnPointType.Hero)
                    emptyHeroSpawnPoints.Add(sp);
            }
        }
    }

    private Transform GetPoint(ref List<SpawnPoint> emptyList, ref List<SpawnPoint> fullList)
    {
        if (emptyList.Count == 0)
        {
            Debug.LogWarning("No empty Spawn points available");
            return null;
        }
        SpawnPoint spawnPoint = emptyList[Random.Range(0, emptyList.Count)];
        emptyList.Remove(spawnPoint);//Here we are putting alot of trust in the other programs to work
        fullList.Add(spawnPoint);

        return spawnPoint.transform;
    }


    private void FreePoint(ref List<SpawnPoint> emptyList, ref List<SpawnPoint> fullList, SpawnPoint spawnPoint)
    {
        emptyList.Add(spawnPoint);
        fullList.Remove(spawnPoint);
    }

    private void RemoveFallenEnemies()
    {
        foreach (SpawnPoint sp in emptyEnemySpawnPoints)
        {
            if (sp.transform.childCount > 0)
                foreach (Transform tr in sp.transform)
                    Destroy(tr.gameObject);
        }
    }


   

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
