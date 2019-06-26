using UnityEngine;

public class Weapon : Entity {
    public int pointsPerHit;
    public int damagePerHit;
    public float attackDelay;
    [HideInInspector] public PlayerMoney playerMoney;
    [HideInInspector] public PlayerDetails playerDetails;
    [HideInInspector] public Inventory inventory;

    public virtual void Start() {
        base.AddCapability(Capability.WEAPON);
    }

    public virtual void Update() {
        if(CanShoot()) {
            WeaponActive();
        }
    }

    /**
     * Returns true if weapon is selected within inventory
     */
    public bool WeaponSelected() {
        return inventory.enabled && inventory.GetSelectedItem() == gameObject;
    }

    /**
     * Returns true if weapon can be fired
     */
    public virtual bool CanShoot() {
        return WeaponSelected();
    }

    /**
     * Will run while weapon can be fired
     */
    public virtual void WeaponActive() {}
}
