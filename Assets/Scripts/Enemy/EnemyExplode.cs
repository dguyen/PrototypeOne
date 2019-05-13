using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplode : MonoBehaviour
{
    [Tooltip("The range that damage will be applied in")]
    public float explosionRadius;

    public LayerMask playerMask;
    public ParticleSystem explosionParticles;
    public int explosionDamage;
    public bool explodeOnDeath = false;

    private EnemyHealth health;

    void Awake() {
        health = GetComponent<EnemyHealth>();
    }

    void Update() {
        if (explodeOnDeath && health.currentHealth <= 0) {
            Explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || health.currentHealth <= 0 || other.GetComponent<PlayerHealth>().currentHealth <= 0)
        {
            return;
        }
        Explode();
    }

    /**
     * Damages all entities within a certain radius around this GameObject
     */
    private void Explode() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, playerMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if (!targetRigidbody)
            {
                continue;
            }

            IDamagable damagable = targetRigidbody.GetComponent<IDamagable>();
            if (damagable == null)
            {
                continue;
            }

            damagable.TakeDamage(explosionDamage);
        }

        explosionParticles.transform.parent = null;
        explosionParticles.Play();
        Destroy(explosionParticles.gameObject, explosionParticles.main.duration);
        Destroy(gameObject);
    }
}
