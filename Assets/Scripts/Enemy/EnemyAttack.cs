using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    public float AttackDelay = 0.5f;
    public int AttackDamage = 30;

    private GameObject[] Players;
    private PlayerHealth[] PlayerHealths;
    private bool[] InRangePlayers;
    private EnemyHealth EnemyHealth;
    private bool InRange;
    private float Timer;

    void Awake() {
        Players = GameObject.FindGameObjectsWithTag("Player");
        PlayerHealths = new PlayerHealth[Players.Length];
        InRangePlayers = new bool[Players.Length];

        for (int i = 0; i < Players.Length; i++) {
            PlayerHealths[i] = Players[i].GetComponent<PlayerHealth>();
        }
        EnemyHealth = GetComponent<EnemyHealth>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            for (int i = 0; i < Players.Length; i++) {
                if (other.gameObject == Players[i] && PlayerHealths[i].currentHealth > 0) {
                    InRangePlayers[i] = true;
                    InRange = true;
                }
            }
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Player") {
            for (int i = 0; i < Players.Length; i++) {
                if (other.gameObject == Players[i] && PlayerHealths[i].currentHealth > 0) {
                    InRangePlayers[i] = true;
                    InRange = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            for (int i = 0; i < Players.Length; i++) {
                if (other.gameObject == Players[i]) {
                    InRangePlayers[i] = false;
                }
            }
        }
        InRange = false;
    }

    void Update() {
        Timer += Time.deltaTime;
        if (Timer >= AttackDelay && InRange && EnemyHealth.currentHealth > 0) {
            Attack();
            //Todo Attack animation
        }
    }

    /**
     * Attacks the closest player
     */
    void Attack() {
        Timer = 0f;
        int PlayerRef = GetClosestPlayer();
        if(PlayerRef >= 0) {
            if (PlayerHealths[PlayerRef] != null) {
                PlayerHealths[PlayerRef].TakeDamage(AttackDamage);
            }
        }
    }

    /**
     * Returns the closest alive player GameObject
     */
    int GetClosestPlayer() {
        int ClosestPlayerRef = -1;
        float ClosestDistanceSqr = Mathf.Infinity;
        Vector3 CurrentPosition = transform.position;

        for (int i = 0; i < Players.Length; i++) {
            if (PlayerHealths[i].currentHealth <= 0 || !InRangePlayers[i]) {
                continue;
            }

            Transform TmpTransform = Players[i].transform;
            Vector3 DirectionToTarget = TmpTransform.position - CurrentPosition;
            float DSqrToTarget = DirectionToTarget.sqrMagnitude;
            if(DSqrToTarget < ClosestDistanceSqr) {
                ClosestDistanceSqr = DSqrToTarget;
                ClosestPlayerRef = i;
            }
        }
        return ClosestPlayerRef;
    }
}
