using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_2 : Enemy {
    protected override void Start() {
        base.Start();
        Agent.speed = 5f;
    }
}
