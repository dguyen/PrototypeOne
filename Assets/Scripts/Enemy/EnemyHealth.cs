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
