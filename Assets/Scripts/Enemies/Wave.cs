using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private float intervalBetweenEnemies = 0.5f;

    private Vector3 source;
    private Vector3 destination;
    private List<Enemy> enemiesToSpawn;

    public void Init(Vector3 src, Vector3 dest)
    {
        source = src;
        destination = dest;
        enemiesToSpawn = new List<Enemy>(enemies);
        StartCoroutine(SetIntervalEnemies());
    }

    IEnumerator SetIntervalEnemies() {
        GameObject ge = Instantiate(enemiesToSpawn[0].gameObject, source, Quaternion.identity, transform);
        Enemy enemy = ge.GetComponent<Enemy>();
        enemy.Destination = destination;
        enemy.Init(3f);
        enemiesToSpawn.RemoveAt(0);

        if (enemiesToSpawn.Count > 0) {
            yield return new WaitForSeconds(intervalBetweenEnemies);
            StartCoroutine(SetIntervalEnemies());
        }
    }
}