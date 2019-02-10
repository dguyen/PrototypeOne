using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedWeapon : Weapon {
    public int initialAmmoCount = 20;
    public Text ammoCountText;

    private int ammoCount;

    public override void Start() {
        base.Start();
        ammoCount = initialAmmoCount;
    }

    public virtual void OnEnable() {
        UpdateAmmoCount();
    }

    public virtual void OnDisable() {
        if (ammoCountText) {
            ammoCountText.text = "";
        }
    }

    public override bool CanShoot() {
        return WeaponSelected() && ammoCount > 0;
    }

    public void UpdateAmmoCount() {
        if (ammoCountText) {
            ammoCountText.text = ammoCount.ToString();
        }
    }

    public void IncreaseAmmo(int ammo) {
        SetAmmo(ammoCount + ammo);
    }

    public void DecreaseAmmo(int ammo) {
        SetAmmo(ammoCount - ammo);
    }

    public void SetAmmo(int ammo) {
        if (ammo <= 0) {
            ammoCount = 0;
        } else {
            ammoCount = ammo;
        }
        UpdateAmmoCount();
    }

    public int GetAmmoCount() {
        return ammoCount;
    }
}
