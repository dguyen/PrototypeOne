using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : Entity {
    public int pointsPerHit;
    [HideInInspector] public PlayerMoney playerMoney;
    [HideInInspector] public Text ammoCountText;
    [HideInInspector] public Inventory inventory;

    public virtual void Start() {
        base.AddCapability(Capability.WEAPON);
    }

    public virtual void Update() {
        if(CanShoot()) {
            WeaponActive();
        }
    }

    public bool WeaponSelected() {
        return inventory.GetSelectedItem() == gameObject;
    }

    public virtual bool CanShoot() {
        return WeaponSelected();
    }

    public virtual void WeaponActive() {}
}
