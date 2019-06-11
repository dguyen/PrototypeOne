using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public GameObject Target;
    public int Damage = 30;
    public float Speed = 5f;
    public ParticleSystem HitParticleSystem;

    private bool fired = false;
    private Rigidbody Rb;

    void Awake() {
        Rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, 120);
    }

    void FixedUpdate() {
        if (fired) {
            MoveProjectile();
        }
    }

    /**
     * Move projectile per frame
     */
    public virtual void MoveProjectile() {
        Vector3 NewPos = Vector3.MoveTowards(gameObject.transform.position, Target.transform.position, Speed * Time.deltaTime);
        NewPos.y = transform.position.y;
        gameObject.transform.position = NewPos;
    }

    void OnCollisionEnter(Collision other) {
        Hit(other.gameObject);
    }

    /**
     * Is called when projectile hits another collider
     */
    public virtual void Hit(GameObject other) {
        IDamagable Damagable = other.gameObject.GetComponent<IDamagable>();
        if (Damagable != null) {
            Damagable.TakeDamage(Damage);
        }
        ReattachParticle(other);
        DestroyProjectile();
    }

    /**
     * Removes destroy particle system from gameobject and reattaches to another gameobject
     */
    public void ReattachParticle(GameObject other) {
        if (!HitParticleSystem) {
            return;
        } else if (other.gameObject.isStatic || other.gameObject.activeSelf) {
            HitParticleSystem.gameObject.transform.parent = null;
        } else {
            var emptyObject = new GameObject();
            emptyObject.transform.parent = other.transform;
            HitParticleSystem.gameObject.transform.parent = emptyObject.transform;
        }
    }

    /**
     * Destroy the projectile
     */
    public void DestroyProjectile() {
        if (HitParticleSystem != null) {
            HitParticleSystem.Play();
            Destroy(HitParticleSystem, 2);
        }
        gameObject.SetActive(false);
        Destroy(gameObject, 3);
    }

    /**
     * Fire the projectile
     */
    public virtual void Fire() {
        fired = true;
    }
}
