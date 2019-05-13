using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    public int minimumSpawnAmount = 5;
    public GameObject spawnArea;
    public EnemySpawn[] enemies;
    public Area[] areas;

    private int enemiesLeft;
    private int[] enemiesToSpawn;
    private List<GameObject> spawnedEnemies;

    private List<int> BossEnemies = new List<int>(); 
    private List<int> MiniBossEnemies = new List<int>(); 
    private List<int> NormalEnemies = new List<int>(); 
    private List<int> SpecialEnemies = new List<int>(); 

    public void SpawnNext(int wave) {
        UpdateEnemies(wave);
        spawnedEnemies = new List<GameObject>();
        StartCoroutine(BeginSpawning(wave));
    }

    /**
     * Returns the number of enemies to be spawned in a NORMAL wave
     */
    private int CalculateSpawnAmount(int wave) {
        // Currently spawning at a (wave * 2 + minSpawn)
        // Todo: Use different function i.e. logarithmic
        return (wave * 2) + minimumSpawnAmount;
    }

    private IEnumerator BeginSpawning(int wave) {
        enemiesToSpawn = WhatToSpawn(wave);

        for (int i = 0; i < enemiesToSpawn.Length; i++) {
            yield return StartCoroutine(Spawn(enemies[enemiesToSpawn[i]].enemy));
        }
        yield return null;
    }

    private IEnumerator Spawn(GameObject ToSpawn) {
        spawnedEnemies.Add(GetRandomArea().Spawn(ToSpawn));
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

    /**
     * Returns the amount of Enemies left
     * Todo: Replace with a efficient method
     */
    public int EnemiesLeft() {
        spawnedEnemies.RemoveAll(item => item == null);
        return spawnedEnemies.Count;
    }

    /**
     * Returns an array of integers referencing enemies based on the wave
     */
    private int[] WhatToSpawn(int wave) {
        if (wave % 5 == 0 && wave % 10 != 0) {
            Debug.Log("Special wave: " + wave);
            return GetSpecialEnemies(Mathf.RoundToInt(CalculateSpawnAmount(wave)/2));
        } else if (wave % 10 == 0) {
            Debug.Log("Boss wave: " + wave);
            return GetBossEnemies(wave/10);
        } else {
            Debug.Log("Normal wave: " + wave);
            return GetNormalEnemies(CalculateSpawnAmount(wave));
        }
    }

    /**
     * Returns an array of ints referencing special enemies
     */
    private int[] GetSpecialEnemies(int numberOfEnemies) {
        int[] newEnemies = new int[numberOfEnemies];
        for (int i = 0; i < newEnemies.Length; i++) {
            newEnemies[i] = SpecialEnemies[Random.Range(0, SpecialEnemies.Count)];
        }
        return newEnemies;
    }

    /**
     * Returns an array of ints referencing random normal enemies
     */
    private int[] GetNormalEnemies(int numberOfEnemies) {
        int[] newEnemies = new int[numberOfEnemies];
        for (int i = 0; i < newEnemies.Length; i++) {
            newEnemies[i] = NormalEnemies[Random.Range(0, NormalEnemies.Count)];
        }
        return newEnemies;
    }

    /**
     * Returns an array of ints referencing random bosses
     */
    private int[] GetBossEnemies(int numBosses) {
        int[] bosses = new int[numBosses];
        for (int i = 0; i < bosses.Length; i++) {
            bosses[i] = BossEnemies[Random.Range(0, BossEnemies.Count)];
        }
        return bosses;
    }

    /**
     * Update which enemies can be spawned
     */
    private void UpdateEnemies(int wave) {
        BossEnemies.Clear();
        MiniBossEnemies.Clear();
        SpecialEnemies.Clear();
        NormalEnemies.Clear();
        for (int i = 0; i < enemies.Length; i++) {
            if (!enemies[i].CanSpawn(wave)) {
                continue;
            } else if (enemies[i].enemyType == EnemyTypes.Boss) {
                BossEnemies.Add(i);
            } else if (enemies[i].enemyType == EnemyTypes.Miniboss) {
                MiniBossEnemies.Add(i);
            } else if (enemies[i].enemyType == EnemyTypes.Special)  {
                SpecialEnemies.Add(i);
            } else {
                NormalEnemies.Add(i);
            }
        }
    }
}
