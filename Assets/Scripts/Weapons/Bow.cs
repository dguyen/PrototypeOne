﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : RangedWeapon {
    public Transform ArrowSpawn;
    public Rigidbody Arrow;
    public float ArrowSpeed;
    public float MinLaunchForce = 15f;
    public float MaxLaunchForce = 30f;
    public float MaxChargeTime = 0.75f;

    private float CurrentLaunchForce;
    private float ChargeSpeed;
    private bool Fired;

    public override void Start() {
        base.Start();
        ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / MaxChargeTime;
    }

    public override void WeaponActive() {
        if (Input.GetButtonDown("Fire1_P" + playerControlScheme)) {
            Fired = false;
            CurrentLaunchForce = MinLaunchForce;

        } else if (Input.GetButton("Fire1_P" + playerControlScheme) && !Fired && CurrentLaunchForce < MaxLaunchForce) {
            float newLaunchForce = CurrentLaunchForce + ChargeSpeed * Time.deltaTime;
            if (newLaunchForce >= MaxLaunchForce) {
                newLaunchForce = MaxLaunchForce;
            }
            CurrentLaunchForce = newLaunchForce;

        } else if (Input.GetButtonUp("Fire1_P" + playerControlScheme) && !Fired) {
            Fire();
        }
    }

    private void Fire() {
        Fired = true;

        Rigidbody arrowRigidbody = Instantiate(Arrow, ArrowSpawn.position, ArrowSpawn.rotation) as Rigidbody;
        Arrow newArrow = arrowRigidbody.gameObject.GetComponent<Arrow>();
        newArrow.MoneyPerHit = pointsPerHit;
        newArrow.PlayerMoney = playerMoney;
        float damageMultiplier = ((CurrentLaunchForce - MinLaunchForce) / 10) + 1;
        newArrow.Damage = Mathf.RoundToInt(newArrow.Damage * damageMultiplier);
        arrowRigidbody.velocity = ArrowSpawn.forward * CurrentLaunchForce;

        CurrentLaunchForce = MinLaunchForce;
        DecreaseAmmo(1);
    }
}
