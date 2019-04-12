using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    private GameObject[] Players;
    private PlayerHealth[] PlayerHealths;
    private PlayerHealth PlayerHealth;
    private EnemyHealth EnemyHealth;
    private UnityEngine.AI.NavMeshAgent Nav;
    
    void Awake() {
        Players = GameObject.FindGameObjectsWithTag("Player");
        PlayerHealths = new PlayerHealth[Players.Length];

        for (int i = 0; i < Players.Length; i++) {
            PlayerHealths[i] = Players[i].GetComponent<PlayerHealth>();
        }
        
        EnemyHealth = GetComponent<EnemyHealth>();
        Nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update() {
        if (EnemyHealth.currentHealth <= 0) {
            Nav.enabled = false;
            return;
        }

        GameObject ClosestPlayer = GetClosestPlayer();
        if (ClosestPlayer != null) {
            Nav.SetDestination (ClosestPlayer.transform.position);
        } else {
            Nav.enabled = false;
        }
    }

    /**
     * Returns the closest alive player GameObject
     */
    GameObject GetClosestPlayer() {
        GameObject ClosestPlayer = null;
        float ClosestDistanceSqr = Mathf.Infinity;
        Vector3 CurrentPosition = transform.position;

        for (int i = 0; i < Players.Length; i++) {
            if (PlayerHealths[i].currentHealth <= 0) {
                continue;
            }

            Transform TmpTransform = Players[i].transform;
            Vector3 DirectionToTarget = TmpTransform.position - CurrentPosition;
            float DSqrToTarget = DirectionToTarget.sqrMagnitude;
            if(DSqrToTarget < ClosestDistanceSqr) {
                ClosestDistanceSqr = DSqrToTarget;
                ClosestPlayer = Players[i];
            }
        }
        return ClosestPlayer;
    }
}
