using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAutomaticWeapon : SingleFireWeapon {
    public bool CanSprintFire = false;
    public bool CanMoveFire = true;

    private PlayerMovement PlayerMovement;
    private bool ModifiedMovement = false;

    public override void WeaponActive() {
        if (PlayerMovement == null) {
            PlayerMovement = GetComponentInParent<PlayerMovement>();
        }
        if (Input.GetButton("Fire1_P" + playerControlScheme) && CanFire()) {
            if (!ModifiedMovement) {
                if (!CanSprintFire) {
                    ModifySprint(false);
                }
                if (!CanMoveFire) {
                    ModifyMovement(false);
                }
                ModifiedMovement = true;
            }
            Fire();  
        } else if (Input.GetButtonUp("Fire1_P" + playerControlScheme)) {
            // Remove movement restrictions
            ModifySprint(true);                
            ModifyMovement(true);
            ModifiedMovement = false;
        }
    }

    /**
     * Enable or disable sprint ability
     */
    private void ModifySprint(bool newValue) {
        PlayerMovement.canSprint = newValue;
    }

    /**
     * Enable or disable movement ability
     */
    private void ModifyMovement(bool newValue) {
        PlayerMovement.canMove = newValue;
    }
}
