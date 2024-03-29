﻿using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GameObject enemyGO;
    public List<Wave> waves;

    [SerializeField] private Transform Destination = null;

    // Called on the "Run" button
    public void ThrowNextWave()
    {
        waves[0].Init(transform.position, Destination.position);
        MGR_Game.Instance.SetPhase2();
        //waves.RemoveAt(0);
    }


}