using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterMover : MonoBehaviour
{
    public event Action OnMovementFinished = delegate { };

    private NavMeshAgent agent;
    private bool isMoving;
    private bool isReturning;

    private Quaternion defaultFacing;
    private Vector3 defaultLocation;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 0.1f;
    private float facingTreshold=5;
    private float distanceThreshold = 0.1f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;  
    }
    private void Start()
    {
        SetNewCombatLocation();
    }

    private void Update()
    {
        if (!isMoving && !isReturning)
            return;

        //Debug.Log(agent.stoppingDistance + " stopping, remaining: " + agent.remainingDistance);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance+distanceThreshold)
        {
            if (isMoving)
            {
                isMoving = false;
                OnMovementFinished.Invoke();
                return;
            }
            if (Quaternion.Angle(transform.rotation, defaultFacing) < facingTreshold)
            {
                isReturning = false;
                OnMovementFinished.Invoke();
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, defaultFacing, rotationSpeed);
        }


    }

    public void SetNewCombatLocation()
    {
        SpawnPoint spawnPoint = GetComponentInParent<SpawnPoint>();
        if (!spawnPoint)
            Debug.LogWarning(name + " does not have a spawn point");
        defaultLocation = spawnPoint.GetCombatLocation().position;
        defaultFacing = Quaternion.LookRotation(spawnPoint.GetCombatLocation().forward, Vector3.up);
    }

    public void MoveTo(Vector3 position, float distance)
    {
        //Debug.Log("Mover input parameters: position " + position + " distance" + distance);
        agent.stoppingDistance = distance;
        agent.SetDestination(position);
        isMoving = true;
    }

    public void MoveHome()
    {
        //Debug.Log("Moving Home");
        agent.stoppingDistance = 0;
        agent.SetDestination(defaultLocation);
        isReturning = true;
    }

    public bool IsHome()
    {
       // Debug.Log(Quaternion.Angle(transform.rotation, defaultFacing) + " " + Vector3.Distance(transform.position, defaultLocation));
        return (Quaternion.Angle(transform.rotation, defaultFacing) < facingTreshold && Vector3.Distance(transform.position, defaultLocation) < distanceThreshold);
    }

}
