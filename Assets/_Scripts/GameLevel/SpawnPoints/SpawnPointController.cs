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

        emptyEnemySpawnPoints = new List<SpawnPoint>();
        fullEnemySpawnPoints = new List<SpawnPoint>();
        fullHeroSpawnPoints = new List<SpawnPoint>();
        emptyHeroSpawnPoints = new List<SpawnPoint>();

    }

    private void OnEnable()
    {
        CharacterHealth.OnAnyEnemyDied += OnAnyEnemyDied;
        ActionSequencer.OnNewRoundStarted += ActionSequencer_OnNewRoundStarted;
    }


    private void OnDisable()
    {
        CharacterHealth.OnAnyEnemyDied -= OnAnyEnemyDied;
        ActionSequencer.OnNewRoundStarted -= ActionSequencer_OnNewRoundStarted;
    }

    //--------------------------------------------------------------------------
    //                  Public methods
    //--------------------------------------------------------------------------

    public void SetupStage(int stage)
    {
        InitializeSpawnPointLists(stage);
    }

    public SpawnPoint GetEmptySpawnPoint(SpawnPointType type)
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

    public void RemoveFallenEnemies()
    {
        foreach (SpawnPoint sp in emptyEnemySpawnPoints)
        {
            sp.EmptyPoint();
        }
    }

    public List<CharacterHealth> FindNearby(float radius)
    {
        List<CharacterHealth> foundList = new List<CharacterHealth>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach(Collider co in colliders)
        {
            CharacterHealth ch = co.GetComponentInParent<CharacterHealth>();
            if (ch)
                foundList.Add(ch);
        }
        return foundList;
    }

    public List<CharacterHealth> GetAllFraction(Fraction fraction)
    {
        List<CharacterHealth> foundList = new List<CharacterHealth>();

        if (fraction == Fraction.Enemy)
            foreach (SpawnPoint sp in fullEnemySpawnPoints)
            {
                CharacterHealth ch = sp.GetComponentInChildren<CharacterHealth>();
                if (ch)
                    foundList.Add(ch);
            }
        else
            foreach (SpawnPoint sp in fullHeroSpawnPoints)
            {
                CharacterHealth ch = sp.GetComponentInChildren<CharacterHealth>();
                if (ch)
                    foundList.Add(ch);
            }

        return foundList;
    } 



    //-------------------------------------------------------------------------------
    //                       Private Functions
    //-------------------------------------------------------------------------------

    private void OnAnyEnemyDied(CharacterHealth deadEnemy)
    {
        SpawnPoint spawnPoint = deadEnemy.GetComponentInParent<SpawnPoint>();
        
        FreeSpawnPoint(spawnPoint);
    }

    private void ActionSequencer_OnNewRoundStarted(int obj)
    {
        //RemoveFallenEnemies();
    }

    private void InitializeSpawnPointLists(int stage)
    {
        RemoveFallenEnemies();

        emptyEnemySpawnPoints.Clear();
        fullEnemySpawnPoints.Clear();
        fullHeroSpawnPoints.Clear();
        emptyHeroSpawnPoints.Clear();

        Debug.Log(allSpawnPointsArray.Length + " total spawn points");

        foreach (SpawnPoint sp in allSpawnPointsArray)
        {
            if (sp.Stage != stage)
                continue;

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

        Debug.Log("Spawn Points initialized: " + emptyHeroSpawnPoints.Count + " empty hero points, " + emptyEnemySpawnPoints.Count + " empty enemy points");
        Debug.Log("Spawn Points initialized: " + fullHeroSpawnPoints.Count + " full hero points, " + fullEnemySpawnPoints.Count + " full enemy points");
        // foreach (SpawnPoint sp in emptyEnemySpawnPoints)
        //   Debug.Log(sp.name + " at position " + sp.transform.position);
    }

    

    private SpawnPoint GetPoint(ref List<SpawnPoint> emptyList, ref List<SpawnPoint> fullList)
    {
        if (emptyList.Count == 0)
        {
            Debug.LogWarning("No empty Spawn points available");
            return null;
        }
        SpawnPoint spawnPoint = emptyList[Random.Range(0, emptyList.Count)];
        emptyList.Remove(spawnPoint);//Here we are putting alot of trust in the other programs to work
        fullList.Add(spawnPoint);

        return spawnPoint;
    }


    private void FreePoint(ref List<SpawnPoint> emptyList, ref List<SpawnPoint> fullList, SpawnPoint spawnPoint)
    {
        emptyList.Add(spawnPoint);
        fullList.Remove(spawnPoint);
    }

    


   

}

public enum Fraction
{
    Hero,
    Enemy
}
