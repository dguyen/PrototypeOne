using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Entity {
    public int perArrowDamage = 10;
    public int moneyPerHit;
    public ParticleSystem arrowDestroyParticle;
    [HideInInspector] public PlayerMoney playerMoney;

    private Rigidbody arrowRb;

    void Start() {
        arrowRb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, 120);
    }

    void FixedUpdate() {
        // Rotate arrow depending on velocity
        if (arrowRb.velocity != Vector3.zero) {
            arrowRb.rotation = Quaternion.LookRotation(arrowRb.velocity);
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            return;
        }
        IDamagable damagable = other.gameObject.GetComponent<IDamagable>();
        if (damagable != null) {
            damagable.TakeDamage(perArrowDamage);
            playerMoney.IncreaseMoney(moneyPerHit);
        }
        ReattachParticle(other.gameObject);
        DestroyArrow();
    }

    /**
     * Removes destroy particle system from gameobject and reattaches to another gameobject
     */
    void ReattachParticle(GameObject other) {
        if (other.gameObject.isStatic) {
            arrowDestroyParticle.gameObject.transform.parent = null;
        } else {
            var emptyObject = new GameObject();
            emptyObject.transform.parent = other.transform;
            arrowDestroyParticle.gameObject.transform.parent = emptyObject.transform;
        }
    }

    void DestroyArrow() {
        if (arrowDestroyParticle != null) {
            arrowDestroyParticle.Play();
            Destroy(arrowDestroyParticle, 1);
        }
        gameObject.SetActive(false);
        Destroy(gameObject, 2);
    }
}
