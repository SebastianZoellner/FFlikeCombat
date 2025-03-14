using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour
{
    public static SpawnPointController Instance;

    private SpawnPoint[] allSpawnPointsArray;
    public List<SpawnPoint> emptyEnemySpawnPoints;
    public List<SpawnPoint> fullEnemySpawnPoints;
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
        SpawnPoint spawnPoint;
        if (type == SpawnPointType.Enemy)
            spawnPoint= GetPoint(emptyEnemySpawnPoints, fullEnemySpawnPoints);
        else
       spawnPoint= GetPoint(emptyHeroSpawnPoints, fullHeroSpawnPoints);

        if (!spawnPoint)
        {
            Debug.LogWarning("Unable to provide spawn point of type " + type);
            ProvideSpawnPointCount();
        }

        return spawnPoint;
    }

    /// <summary>
    /// Indicates to the spawn point controller that a spawn point has been filled
    /// </summary>
    /// <param name="newSpawnPoint"> the newly fille spawn point </param>
    /// <param name="type"> The fraction of the spawn point. </param>
    
    public void AssignSpawnPoint(SpawnPoint newSpawnPoint, SpawnPointType type)
    {
        if(type== SpawnPointType.Enemy)
            FillPoint(newSpawnPoint, emptyEnemySpawnPoints, fullEnemySpawnPoints);
        else
            FillPoint(newSpawnPoint,emptyHeroSpawnPoints, fullHeroSpawnPoints);
    }

   

    public void RemoveFallenEnemies()
    {
        foreach (SpawnPoint sp in emptyEnemySpawnPoints)
        {
            sp.EmptyPoint();
        }
        Debug.Log("All fallen enemies removed");
    }


    public List<IDamageable> GetAllFraction(Faction fraction)
    {
        List<IDamageable> foundList = new List<IDamageable>();

        if (fraction == Faction.Enemy)
            foreach (SpawnPoint sp in fullEnemySpawnPoints)
            {
                IDamageable ch = sp.GetComponentInChildren<IDamageable>();
                if (ch!=null)
                    foundList.Add(ch);
            }
        else
            foreach (SpawnPoint sp in fullHeroSpawnPoints)
            {
                IDamageable ch = sp.GetComponentInChildren<IDamageable>();
                if (ch!=null)
                    foundList.Add(ch);
            }

        return foundList;
    }

    public List<IDamageable> GetAllInRadius(Transform target, float radius, Faction fraction)
    {
        List<IDamageable> foundList = new List<IDamageable>();

        SpawnPoint targetSpawnPoint = target.GetComponentInParent<SpawnPoint>();
        if (targetSpawnPoint == null)
        {
            foundList.Add(target.GetComponent<IDamageable>());
            return foundList;
        }

        if (fraction == Faction.Enemy)
            foreach (SpawnPoint sp in fullEnemySpawnPoints)
            {
                Debug.Log("Distance "+SpawnPointDistance(sp, targetSpawnPoint)+" radius "+radius);
                if (SpawnPointDistance(sp, targetSpawnPoint) < radius)
                {
                    IDamageable ch = sp.GetComponentInChildren<IDamageable>();
                    if (ch!=null)
                    {
                        foundList.Add(ch);
                        Debug.Log("Added "+ch.GetName());
                    }
                }
            }
        else
            foreach (SpawnPoint sp in fullHeroSpawnPoints)
            {
                if (SpawnPointDistance(sp, targetSpawnPoint) < radius)
                {
                    IDamageable ch = sp.GetComponentInChildren<IDamageable>();
                    if (ch!=null)
                        foundList.Add(ch);
                }
            }
        return foundList;
    }



    //-------------------------------------------------------------------------------
    //                       Private Functions
    //-------------------------------------------------------------------------------

    private void OnAnyEnemyDied(CharacterHealth deadEnemy, CharacterCombat ignored)
    {
        SpawnPoint spawnPoint = deadEnemy.GetComponentInParent<SpawnPoint>();

        FreePoint(emptyEnemySpawnPoints, fullEnemySpawnPoints, spawnPoint);
    }

    private void ActionSequencer_OnNewRoundStarted(int obj)
    {
        //RemoveFallenEnemies();
    }

    private void InitializeSpawnPointLists(int stage)
    {
        Debug.Log("Initializing Spawn Points Stage " + stage);
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

        ProvideSpawnPointCount();
        // foreach (SpawnPoint sp in emptyEnemySpawnPoints)
        //   Debug.Log(sp.name + " at position " + sp.transform.position);
    }

    private void ProvideSpawnPointCount()
    {
        Debug.Log("Spawn Points initialized: " + emptyHeroSpawnPoints.Count + " empty hero points, " + emptyEnemySpawnPoints.Count + " empty enemy points");
        Debug.Log("Spawn Points initialized: " + fullHeroSpawnPoints.Count + " full hero points, " + fullEnemySpawnPoints.Count + " full enemy points");
    }


    private SpawnPoint GetPoint(List<SpawnPoint> emptyList, List<SpawnPoint> fullList)
    {
        if (emptyList.Count == 0)
        {
            Debug.LogWarning("No empty Spawn points available");
            return null;
        }
        SpawnPoint spawnPoint = emptyList[Random.Range(0, emptyList.Count)];
        if (!spawnPoint)
            Debug.LogWarning("No empty spawnpoint found");
        

        return spawnPoint;
    }

    private void FillPoint(SpawnPoint newSpawnPoint, List<SpawnPoint> emptyList, List<SpawnPoint> fullList)
    {
        emptyList.Remove(newSpawnPoint);
        fullList.Add(newSpawnPoint);
    }

    private void FreePoint(List<SpawnPoint> emptyList, List<SpawnPoint> fullList, SpawnPoint spawnPoint)
    {
        emptyList.Add(spawnPoint);
        fullList.Remove(spawnPoint);
    }

    private float SpawnPointDistance(SpawnPoint sp1, SpawnPoint sp2)
    {
        //Debug.Log("Evaluating Spawn Points " + sp1.name + " and " + sp2.name);
        return (sp1.GetCombatLocation().position - sp2.GetCombatLocation().position).magnitude;
    }

}

public enum Faction
{
    Hero,
    Enemy
}
