using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour {
    public Transform[] EnemySpawnPoints;
    public bool IsOpen = false;

    private int NumPlayers = 0;

    public GameObject Spawn(GameObject ToSpawn) {
        int SpawnPos = Random.Range(0, EnemySpawnPoints.Length);
        return Instantiate(ToSpawn, EnemySpawnPoints[SpawnPos]);
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            NumPlayers++;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            NumPlayers--;
        }
    }

    public void Open() {
        IsOpen = true;
    }

    public int GetNumPlayers() {
        return NumPlayers;
    }
}
