using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireWeapon : RangedWeapon {
    public Transform gunPoint;
    public ParticleSystem gunParticles;
    public float effectsDisplayTime = 0.1f;

    private float timer;
    private Ray shootRay = new Ray();
    private RaycastHit shootHit;
    private int shootableMask;
    private Light gunLight;
    private LineRenderer gunLine;

    void Awake() {
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
        if (attackDelay < effectsDisplayTime) {
            effectsDisplayTime = attackDelay;
        }
    }

    public override void Update() {
        base.Update();
        timer += Time.deltaTime;
        if(timer >= effectsDisplayTime) {
            DisableEffects();
        }
    }

    public override void WeaponActive() {
        if (Input.GetButtonDown("Fire1_P" + playerControlScheme) && CanFire()) {
            Fire();
        }
    }

    public bool CanFire() {
        return timer >= attackDelay;
    }

    public void DisableEffects () {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    public void Fire() {
        timer = 0f;
        gunLight.enabled = true;
        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, gunPoint.position);
        
        shootRay.origin = gunPoint.position;
        shootRay.direction = gunPoint.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask)) {
            IDamagable damagable = shootHit.collider.GetComponent<IDamagable>();
            if (damagable != null) {
                damagable.TakeDamage(damagePerHit);
                playerMoney.IncreaseMoney(pointsPerHit);
            }
            gunLine.SetPosition(1, shootHit.point);
        } else {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
        DecreaseAmmo(1);
    }
}
