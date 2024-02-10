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
    [SerializeField] private float rotationSpeed = 2;
    private float facingTreshold=5;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        defaultFacing = Quaternion.LookRotation(transform.forward, Vector3.up);
        defaultLocation = transform.position;
    }

    private void Update()
    {
        if (!isMoving && !isReturning)
            return;

        Debug.Log(agent.isStopped + " " + agent.remainingDistance);
        if (agent.remainingDistance <= agent.stoppingDistance)
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
    public void MoveTo(Vector3 position, float distance)
    {
        agent.stoppingDistance = distance;
        agent.SetDestination(position);
        isMoving = true;
    }

    public void MoveHome()
    {
        agent.stoppingDistance = 0;
        agent.SetDestination(defaultLocation);
        isReturning = true;
    }

}
