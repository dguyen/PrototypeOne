using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangedWeapon : Weapon {
    public int ammoCount = 20;
    public Text ammoCountText;

    void OnEnable() {
        UpdateAmmoCount();
    }

    void OnDisable() {
        ammoCountText.text = "";
    }

    public override bool CanShoot() {
        return WeaponSelected() && ammoCount > 0;
    }

    public void UpdateAmmoCount() {
        if (ammoCountText) {
            ammoCountText.text = ammoCount.ToString();
        }
    }
}
