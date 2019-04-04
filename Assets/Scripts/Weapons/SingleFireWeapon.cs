using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFireWeapon : RangedWeapon {
    public int damagePerShot = 30;
    public float fireDelay = 0.2f;
    public float range = 100f;
    public Transform gunPoint;
    public ParticleSystem gunParticles;

    private float timer;
    private float effectsDisplayTime = 0.2f;
    private Ray shootRay = new Ray();
    private RaycastHit shootHit;
    private int shootableMask;
    private Light gunLight;
    private LineRenderer gunLine;

    void Awake() {
        shootableMask = LayerMask.GetMask("Shootable");
        gunLine = GetComponent<LineRenderer>();
        gunLight = GetComponent<Light>();
        if (fireDelay < effectsDisplayTime) {
            effectsDisplayTime = fireDelay;
        }
    }

    public override void Update() {
        base.Update();
        timer += Time.deltaTime;
        if(timer >= fireDelay * effectsDisplayTime) {
            DisableEffects();
        }
    }

    public override void WeaponActive() {
        if (Input.GetButtonDown("Fire1_P" + playerNumber) && timer >= fireDelay) {
            timer = 0f;
            Fire();    
        }
    }

    public void DisableEffects () {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    private void Fire() {
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
                damagable.TakeDamage(damagePerShot);
                playerMoney.IncreaseMoney(pointsPerHit);
            }
            gunLine.SetPosition(1, shootHit.point);
        } else {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
        DecreaseAmmo(1);
    }
}
