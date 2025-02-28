using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharacterMover : MonoBehaviour
{
    public event Action OnMovementFinished = delegate { };

    public bool Initialized { get; private set; } = false;

    private NavMeshAgent agent;
    private bool isMoving;
    private bool isReturning;

    private Quaternion defaultFacing;
    private Vector3 defaultLocation;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed = 400;
    private float facingTreshold=5;
    private float distanceThreshold = 0.1f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        Initialized = true;
    }
    private void Start()
    {
        SetNewCombatLocation();
    }

    private void Update()
    {
        if (!isMoving && !isReturning)
            return;

        float dis = agent.stoppingDistance + distanceThreshold;
        //Debug.Log(dis + " stopping, remaining: " + agent.remainingDistance + " pending: " + agent.pathPending);

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance+distanceThreshold)
        {
            
            if (isMoving)
            {
                isMoving = false;
                OnMovementFinished.Invoke();
                return;
            }
            float angleDifference = Quaternion.Angle(transform.rotation, defaultFacing);
            if ( angleDifference< facingTreshold)
            {
                isReturning = false;
                OnMovementFinished.Invoke();
            }
           
            float step = rotationSpeed * Time.deltaTime; 
            //Debug.Log("angle: "+ angleDifference+" Step: "+step);
            if (angleDifference > step)
                transform.rotation = Quaternion.Slerp(transform.rotation, defaultFacing, step / angleDifference);
            else
                transform.rotation = defaultFacing;
            
            
        }


    }

    public void SetNewCombatLocation()
    {
        SpawnPoint spawnPoint = GetComponentInParent<SpawnPoint>();
        if (!spawnPoint)
        {
            Debug.LogWarning(name + " does not have a spawn point",gameObject);
            return;
        }
        defaultLocation = spawnPoint.GetCombatLocation().position;
        defaultFacing = Quaternion.LookRotation(spawnPoint.GetCombatLocation().forward, Vector3.up);
    }

    public void MoveTo(Vector3 targetPosition, float distance)
    {
      
        //Debug.Log("Mover input parameters: position " + position + " distance" + distance);
        if(Vector3.Distance(transform.position,targetPosition)<distance+distanceThreshold)
        {
            OnMovementFinished.Invoke();
            return;
        }

        agent.stoppingDistance = distance;
        agent.SetDestination(targetPosition);
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
