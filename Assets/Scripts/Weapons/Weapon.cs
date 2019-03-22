using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Entity {
    public int pointsPerHit;

    private Inventory inventory;

    public virtual void Start() {
        base.AddCapability(Capability.PICKABLE);
        base.AddCapability(Capability.DROPABLE);
        base.AddCapability(Capability.WEAPON);
        inventory = FindObjectOfType<Inventory>();
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
