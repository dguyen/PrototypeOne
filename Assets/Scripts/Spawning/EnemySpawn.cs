using UnityEngine;

[System.Serializable]
public class EnemySpawn {
    public GameObject enemy;
    public EnemyTypes enemyType;
    public int minWaveSpawn = 1;

    /**
     * Checks if Enemy can be spawned in this wave
     */
    public bool CanSpawn(int wave) {
        return wave >= minWaveSpawn;
    }
}
