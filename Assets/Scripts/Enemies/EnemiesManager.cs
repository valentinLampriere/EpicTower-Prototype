﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GameObject enemyGO;
    public List<Wave> waves;

    [SerializeField] private Transform Destination;

    public void ThrowNextWave() {
        waves[0].Init(transform.position, Destination.position);

        waves.RemoveAt(0);
    }
}