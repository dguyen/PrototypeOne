using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : RangedWeapon {
    public Transform ArrowSpawn;
    public Rigidbody Arrow;
    public float ArrowSpeed;
    public Slider ChargeSlider;
    public float MinLaunchForce = 15f;
    public float MaxLaunchForce = 30f;
    public float MaxChargeTime = 0.75f;

    private float CurrentLaunchForce;
    private float ChargeSpeed;
    private bool Fired;

    public override void Start() {
        base.Start();
        ChargeSlider = GameObject.Find("ChargeSlider").GetComponent<Slider>();
        ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / MaxChargeTime;
        ChargeSlider.maxValue = MaxLaunchForce;
        ChargeSlider.minValue = MinLaunchForce;
    }

    public override void OnEnable() {
        base.OnEnable();
        if (ChargeSlider) {
            ChargeSlider.gameObject.SetActive(true);
        }
    }

    public override void OnDisable() {
        base.OnDisable();
        if (ChargeSlider) {
            ChargeSlider.gameObject.SetActive(false);
        }
    }

    public override void WeaponActive() {
        if (Input.GetButtonDown("Fire1_P" + playerNumber)) {
            Fired = false;
            CurrentLaunchForce = MinLaunchForce;

        } else if (Input.GetButton("Fire1_P" + playerNumber) && !Fired && CurrentLaunchForce < MaxLaunchForce) {
            float newLaunchForce = CurrentLaunchForce + ChargeSpeed * Time.deltaTime;
            if (newLaunchForce >= MaxLaunchForce) {
                newLaunchForce = MaxLaunchForce;
            }
            CurrentLaunchForce = newLaunchForce;
            ChargeSlider.value = CurrentLaunchForce;

        } else if (Input.GetButtonUp("Fire1_P" + playerNumber) && !Fired) {
            Fire();
        }
    }

    private void Fire() {
        Fired = true;

        Rigidbody arrowRigidbody = Instantiate(Arrow, ArrowSpawn.position, ArrowSpawn.rotation) as Rigidbody;
        Arrow newArrow = arrowRigidbody.gameObject.GetComponent<Arrow>();
        newArrow.moneyPerHit = pointsPerHit;
        float damageMultiplier = ((CurrentLaunchForce - MinLaunchForce) / 10) + 1;
        newArrow.perArrowDamage = Mathf.RoundToInt(newArrow.perArrowDamage * damageMultiplier);
        arrowRigidbody.velocity = ArrowSpawn.forward * CurrentLaunchForce;

        CurrentLaunchForce = MinLaunchForce;
        ChargeSlider.value = MinLaunchForce;
        DecreaseAmmo(1);
    }
}
