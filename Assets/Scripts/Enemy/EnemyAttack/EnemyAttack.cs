using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    public float AttackDelay = 0.5f;
    public int AttackDamage = 30;
    public float AttackRange = 1f;

    private GameObject[] Players;
    private Animator EnemyAnimator;
    private PlayerHealth[] PlayerHealths;
    private EnemyHealth EnemyHealth;
    private float Timer;

    void Awake() {
        Players = GameObject.FindGameObjectsWithTag("Player");
        PlayerHealths = new PlayerHealth[Players.Length];
        EnemyAnimator = GetComponent<Animator>();
        EnemyHealth = GetComponent<EnemyHealth>();

        for (int i = 0; i < Players.Length; i++) {
            PlayerHealths[i] = Players[i].GetComponent<PlayerHealth>();
        }
    }

    void Update() {
        Timer += Time.deltaTime;
        GameObject ClosestPlayer = GetClosestPlayer();
        if (Timer >= AttackDelay && PlayerInRange(ClosestPlayer) && EnemyHealth.currentHealth > 0) {
            Attack(ClosestPlayer);
            Timer = 0f;
            if (EnemyAnimator != null) {
                EnemyAnimator.SetTrigger("Attack");
            }
        }
    }

    /**
     * Returns true if the player is within attack range
     */
    public virtual bool PlayerInRange(GameObject other) {
        if (other == null) {
            return false;
        }
        Vector3 DirectionToTarget = other.transform.position - transform.position;
        return DirectionToTarget.sqrMagnitude <= (AttackRange * AttackRange);
    }

    /**
     * Attack the given player
     */
    public virtual void Attack(GameObject player) {
        PlayerHealth PHealth = GetPlayerHealth(player);
        if (PHealth != null) {
            PHealth.TakeDamage(AttackDamage);
        }
    }

    /**
     * Returns PlayerHealth of a GameObject if it exists
     */
    public PlayerHealth GetPlayerHealth(GameObject player) {
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i] == player && PlayerHealths[i] != null) {
                return PlayerHealths[i];
            }
        }
        return null;
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
