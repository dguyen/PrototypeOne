using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public int minimumSpawnAmount = 5;
    public GameObject spawnArea;
    public GameObject[] enemies;

    private int enemiesLeft;
    private int enemiesToSpawn;
    private List<GameObject> spawnedEnemies;

    public void SpawnNext(int wave) {
        enemiesToSpawn = CalculateSpawnAmount(wave);
        spawnedEnemies = new List<GameObject>();
        StartCoroutine(BeginSpawning());
    }

    private int CalculateSpawnAmount(int wave) {
        // Currently spawning at a (wave * 2 + minSpawn)
        // Todo: Use different function i.e. logarithmic
        return (wave * 2) + minimumSpawnAmount;
    }

    private IEnumerator BeginSpawning() {
        yield return StartCoroutine(Spawn());
        if (enemiesToSpawn <= 0) {
            yield return null;
        } else {
            StartCoroutine(BeginSpawning());
        }
    }

    private IEnumerator Spawn() {
        Vector3 spawnLocation = GetRandomPosition();
        spawnedEnemies.Add(Instantiate(enemies[0], spawnLocation, Quaternion.identity));
        enemiesToSpawn--;
        yield return new WaitForSeconds(1);
    }

    private Vector3 GetRandomPosition() {
        float randomX = Random.Range(spawnArea.transform.position.x - (5 * spawnArea.transform.localScale.x / 2), spawnArea.transform.position.x + (5 * spawnArea.transform.localScale.x / 2));
        float randomY = Random.Range(spawnArea.transform.position.y - (5 * spawnArea.transform.localScale.y / 2), spawnArea.transform.position.y + (5 * spawnArea.transform.localScale.y / 2));
        float randomZ = Random.Range(spawnArea.transform.position.y - (5 * spawnArea.transform.localScale.z / 2), spawnArea.transform.position.y + (5 * spawnArea.transform.localScale.z / 2)); 
        return new Vector3 (randomX, randomY, randomZ);
    }

    public int EnemiesLeft() {
        spawnedEnemies.RemoveAll(item => item == null);
        return spawnedEnemies.Count;
    }
}
