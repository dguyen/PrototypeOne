using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile {
    public int MoneyPerHit;
    [HideInInspector] public PlayerMoney PlayerMoney;

    private Rigidbody ArrowRb;

    void Awake() {
        ArrowRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        // Rotate arrow depending on velocity
        if (ArrowRb.velocity != Vector3.zero) {
            ArrowRb.rotation = Quaternion.LookRotation(ArrowRb.velocity);
        }
    }

    public override void Hit(GameObject other) {
        if (other.gameObject.CompareTag("Player")) {
            return;
        }
        IDamagable Damagable = other.gameObject.GetComponent<IDamagable>();
        if (Damagable != null) {
            Damagable.TakeDamage(Damage);
            PlayerMoney.IncreaseMoney(MoneyPerHit);
        }
        ReattachParticle(other);
        DestroyProjectile();
    }
}
