using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : RangedWeapon {
    public Transform ArrowSpawn;
    public Rigidbody Arrow;
    public float ArrowSpeed;

    public override void WeaponActive() {
        if(Input.GetButtonDown("Fire1")) {
            Rigidbody arrowRigidbody = Instantiate(Arrow, ArrowSpawn.position, ArrowSpawn.rotation) as Rigidbody;
            arrowRigidbody.AddForce(ArrowSpawn.forward * ArrowSpeed);
            DecreaseAmmo(1);
        }
    }
}
