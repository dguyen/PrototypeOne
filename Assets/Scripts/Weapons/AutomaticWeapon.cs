using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : SingleFireWeapon {
    public override void WeaponActive() {
        if (Input.GetButton("Fire1_P" + playerControlScheme) && CanFire()) {
            Fire();  
        }
    }
}
