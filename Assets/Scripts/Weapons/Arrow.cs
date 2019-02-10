using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Entity {
    public int perArrowDamage = 10;

    private bool stuck;

    void Start() {
        Destroy(gameObject, 120);
    }

    private void OnCollisionEnter(Collision other) {
        if (stuck || other.gameObject.tag == "Player") {
            return;
        }
        StickArrow(other);
        IDamagable[] damagables = other.gameObject.GetComponents<IDamagable>();
        foreach (IDamagable damagable in damagables) {
            damagable.TakeDamage(perArrowDamage);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Bow bow = other.gameObject.GetComponentInChildren<Bow>();
            if (bow) {
                bow.IncreaseAmmo(1);
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
        }
    }

    private void StickArrow(Collision toStick) {
        stuck = true;
        Destroy(gameObject.GetComponent<Rigidbody>());
        var emptyObject = new GameObject();
        if (toStick.gameObject.isStatic) {
            emptyObject.transform.parent = toStick.GetContact(0).thisCollider.transform;
        } else {
            emptyObject.transform.parent = toStick.transform;
        }
        gameObject.transform.parent = emptyObject.transform;
    }
}
