using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    private bool CanMove = true;
    private GameObject[] Players;
    private PlayerHealth[] PlayerHealths;
    private PlayerHealth PlayerHealth;
    private EnemyHealth EnemyHealth;
    private Animator EnemyAnimator;
    private UnityEngine.AI.NavMeshAgent Nav;
    
    void Awake() {
        Players = GameObject.FindGameObjectsWithTag("Player");
        EnemyAnimator = GetComponent<Animator>();
        PlayerHealths = new PlayerHealth[Players.Length];

        for (int i = 0; i < Players.Length; i++) {
            PlayerHealths[i] = Players[i].GetComponent<PlayerHealth>();
        }
        
        EnemyHealth = GetComponent<EnemyHealth>();
        Nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update() {
        if (!IsAlive()) {
            Nav.enabled = false;
            return;
        } else if (CanMove) {
            Move();
        }
    }

    /**
     * Returns the NavMeshAgent of the Enemy
     */
    public UnityEngine.AI.NavMeshAgent GetNav() {
        return Nav;
    }

    /**
     * Move towards closest player
     */
    public virtual void Move() {
        GameObject ClosestPlayer = GetClosestPlayer();
        if (ClosestPlayer != null) {
            Nav.SetDestination (ClosestPlayer.transform.position);
        } else {
            Nav.enabled = false;
            if (EnemyAnimator != null) {
                EnemyAnimator.SetTrigger("Idle");
            }
            DisableMove();
        }
    }

    /**
     * Enables movement of Enemy
     */
    public void EnableMove() {
        CanMove = true;
    }

    /**
     * Disables movement of Enemy
     */
    public void DisableMove() {
        if (Nav.hasPath) {
            Nav.ResetPath();
        }
        CanMove = false;
    }

    /**
     * Returns true if Enemy is alive, false otherwise
     */
    public bool IsAlive() {
        return EnemyHealth.currentHealth > 0;
    }

    /**
     * Returns the closest alive player GameObject
     */
    public GameObject GetClosestPlayer() {
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
