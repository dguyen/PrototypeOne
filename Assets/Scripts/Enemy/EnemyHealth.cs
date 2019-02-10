using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    public int spawnHealth = 100;
    public int currentHealth;
    BoxCollider boxCollider;
    bool isDead;

    void Awake()
    {
        currentHealth = spawnHealth;
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if(isDead)
        {
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
            Destroy(gameObject);
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
        boxCollider.isTrigger = true;
        //Todo Death animation
    }
}
