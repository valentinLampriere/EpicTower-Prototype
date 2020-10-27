using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour {
    public Vector3 Destination { get; set; }

    public NavMeshAgent Agent { get; set; }

    protected virtual void Awake() {
        Agent = GetComponent<NavMeshAgent>();
    }
    protected virtual void Start() {
        Agent = GetComponent<NavMeshAgent>();
    }
    public void Init(float speed) {
        Agent.speed = speed;
        Agent.SetDestination(Destination);
    }
    protected virtual void Update() {
        if (HasReachedEnd()) {
            Destroy(gameObject);
        }
    }

    protected bool HasReachedEnd() {
        if (!Agent.pathPending) {
            if (Agent.remainingDistance <= Agent.stoppingDistance) {
                if (Agent.hasPath || Agent.velocity.sqrMagnitude < 1f) {
                    return true;
                }
            }
        }
        return false;
    }
}
