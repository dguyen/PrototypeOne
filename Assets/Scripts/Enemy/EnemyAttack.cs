using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    public float AttackDelay = 0.5f;
    public int AttackDamage = 30;

    private GameObject[] Players;
    private PlayerHealth[] PlayerHealths;
    private EnemyHealth EnemyHealth;
    private bool InRange;
    private float Timer;

    void Awake() {
        Players = GameObject.FindGameObjectsWithTag("Player");
        PlayerHealths = new PlayerHealth[Players.Length];

        for (int i = 0; i < Players.Length; i++) {
            PlayerHealths[i] = Players[i].GetComponent<PlayerHealth>();
        }
        EnemyHealth = GetComponent<EnemyHealth>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            InRange = true;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            InRange = false;
        }
    }

    void Update() {
        Timer += Time.deltaTime;
        if (Timer >= AttackDelay && InRange && EnemyHealth.currentHealth > 0) {
            Attack();
            //Todo Attack animation
        }
    }

    void Attack() {
        Timer = 0f;
        GameObject Player = GetClosestPlayer();
        if(Player != null) {
            IDamagable Hit = (IDamagable)Player.gameObject.GetComponent(typeof(IDamagable));
            if (Hit != null) {
                Hit.TakeDamage(AttackDamage);
            }
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
