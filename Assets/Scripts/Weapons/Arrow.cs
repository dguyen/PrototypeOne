using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Entity {
    public int perArrowDamage = 10;
    public int moneyPerHit;

    private PlayerMoney playerMoney;
    private bool stuck;

    void Start() {
        Destroy(gameObject, 120);
        playerMoney = FindObjectOfType<PlayerMoney>();
    }

    private void OnCollisionEnter(Collision other) {
        if (stuck || other.gameObject.tag == "Player") {
            return;
        }
        StickArrow(other);
        IDamagable[] damagables = other.gameObject.GetComponents<IDamagable>();
        foreach (IDamagable damagable in damagables) {
            damagable.TakeDamage(perArrowDamage);
            playerMoney.IncreaseMoney(moneyPerHit);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Bow bow = other.gameObject.GetComponentInChildren<Bow>(true);
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
