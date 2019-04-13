using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public int minimumSpawnAmount = 5;
    public GameObject spawnArea;
    public GameObject[] enemies;
    public Area[] areas;

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
        spawnedEnemies.Add(GetRandomArea().Spawn(enemies[0]));

        enemiesToSpawn--;
        yield return new WaitForSeconds(1);
    }

    /**
     * Returns a semi-random valid spawning area
     */
    private Area GetRandomArea() {
        float p = Random.Range(0, 100)/100f;
        float cumulativeP = 0.0f;
        float[] areaP = GetProbability(); // Potentially move this to class variable per wave to improve efficiency
        for (int i = 0; i < areas.Length; i++) {
            cumulativeP += areaP[i];
            if (p <= cumulativeP) {
                return areas[i];
            }
        }
        return areas[1];
    }

    /**
     * Returns an array determining the probability of spawning an enemy
     */
    private float[] GetProbability() {
        float total = 0;
        float[] probabilities = new float[areas.Length];
        for (int i = 0; i < areas.Length; i++) {
            if (areas[i].IsOpen) {
                probabilities[i] = 1 + areas[i].GetNumPlayers();
                total += probabilities[i];
            }
        }
        for (int i = 0; i < probabilities.Length; i++) {
            if (probabilities[i] > 0) {
                probabilities[i] = probabilities[i]/total;
            }
        }
        return probabilities;
    }

    public int EnemiesLeft() {
        spawnedEnemies.RemoveAll(item => item == null);
        return spawnedEnemies.Count;
    }
}
