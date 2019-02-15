using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    public int spawnHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 0.25f;

    CapsuleCollider capsuleCollider;
    Animator animator;
    bool isDead;
    bool isSinking = false;

    void Awake()
    {
        animator = GetComponent <Animator> ();
        currentHealth = spawnHealth;
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if(isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage ;

        if (currentHealth <= 0)
        {
            Death();
        }

        //Todo Damage particles
    }

    void Death()
    {
        isDead = true;
        capsuleCollider.isTrigger = true;
        animator.SetTrigger ("Dead");
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;

        foreach (var entity in gameObject.GetComponentsInChildren<Entity>())
        {
            entity.transform.parent = null;
            entity.gameObject.SetActive(true);
            Rigidbody tmpRb = entity.GetComponent<Rigidbody>();
            if (tmpRb) {
                tmpRb.useGravity = true;
            } else {
                entity.gameObject.AddComponent<Rigidbody>();
            }
        }
    }

    public void StartSinking ()
    {
        isSinking = true;
        Destroy (gameObject, 3f);
    }
}
