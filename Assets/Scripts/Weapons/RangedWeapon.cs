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
        if (ammoCountText && gameObject.activeSelf) {
            ammoCountText.text = ammoCount.ToString() + "/" + ammoCapacity;
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
