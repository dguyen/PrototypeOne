using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable {
    public int spawnHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 0.25f;

    private Collider enemyCollider;
    private Animator animator;
    private bool isDead;
    private bool isSinking = false;
    private bool CanDamage = true;

    void Awake() {
        animator = GetComponent <Animator> ();
        enemyCollider = GetComponent<Collider>();
        currentHealth = spawnHealth;
    }

    void Update() {
        if(isSinking) {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage) {
        if (isDead || !CanDamage) {
            return;
        }

        currentHealth -= damage ;
        if (currentHealth <= 0) {
            Death();
        }
        //Todo Damage particles
    }

    public void CanTakeDamage(bool CanBeDamaged) {
        CanDamage = CanBeDamaged;
    }

    void Death() {
        isDead = true;
        enemyCollider.isTrigger = true;
        animator.SetTrigger ("Dead");
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        gameObject.layer = 0;
    }

    public void StartSinking () {
        isSinking = true;
        Destroy (gameObject, 3f);
    }
}
