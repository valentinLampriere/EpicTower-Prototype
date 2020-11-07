using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour{
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private float intervalBetweenEnemies = 0.5f;

    private Vector3 source;
    private Vector3 destination;

    public void Init(Vector3 src, Vector3 dest) {
        source = src;
        destination = dest;
        StartCoroutine(SetIntervalEnemies());
    }

    IEnumerator SetIntervalEnemies() {
        GameObject ge = Instantiate(enemies[0].gameObject, source, Quaternion.identity, transform);
        Enemy enemy = ge.GetComponent<Enemy>();
        enemy.Destination = destination;
        enemy.Init(3f);
        enemies.RemoveAt(0);

        if (enemies.Count > 0) {
            yield return new WaitForSeconds(intervalBetweenEnemies);
            StartCoroutine(SetIntervalEnemies());
        }
    }
}
