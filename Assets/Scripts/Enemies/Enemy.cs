using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour {
    public Vector3 Destination { get; set; }

    public NavMeshAgent Agent { get; set; }

    [SerializeField]
    private float Health = 200f;
    [SerializeField]
    private float currentSpeed = 0f; // just for debug purposes
    
    private float oldSpeed = 0f;

    protected virtual void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Update()
    {
        if (HasReachedEnd() || Health <= 0) {
            Destroy(gameObject);
        }

        currentSpeed = Agent.speed;
    }

    protected bool HasReachedEnd()
    {
        if (!Agent.pathPending)
        {
            if (Agent.remainingDistance <= Agent.stoppingDistance)
            {
                if (Agent.hasPath || Agent.velocity.sqrMagnitude < 1f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Init(float speed) {
        Agent.speed = speed;
        Agent.SetDestination(Destination);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    public void ChangeSpeed(float newSpeed)
    {
        oldSpeed = Agent.speed;
        Agent.speed = newSpeed;
    }

    public void AffectSpeed(float incomingMultiplier)
    {
        oldSpeed = Agent.speed;
        Agent.speed *= incomingMultiplier;
    }

    public void RestoreSpeed()
    {
        Agent.speed = oldSpeed;
    }
}
