using UnityEngine;

public class RangedWeapon : Weapon {
    public int ammoCapacity = 20;
    public float range = 100f;

    private int ammoCount;

    public override void Start() {
        base.Start();
        base.AddCapability(Capability.REFILLABLE);
        ammoCount = ammoCapacity;
        UpdateAmmoCount();
    }

    public override void Update() {
        base.Update();
        if (Input.GetButtonDown("Fire1_P" + playerControlScheme) && GetAmmoCount() <= 0) {
            playerDetails.PlayerUI.NoAmmoAnim();
        }
    }

    public virtual void OnEnable() {
        UpdateAmmoCount();
    }

    public virtual void OnDisable() {
        if (playerDetails != null) {
            playerDetails.PlayerUI.UpdateAmmo(0);
        }
    }

    public override bool CanShoot() {
        return WeaponSelected() && ammoCount > 0;
    }

    public void UpdateAmmoCount() {
        if (gameObject.activeSelf && playerDetails != null) {
            playerDetails.PlayerUI.UpdateAmmo(ammoCount);
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

    public void RefillAmmo() {
        SetAmmo(ammoCapacity);
        UpdateAmmoCount();
    }
}
