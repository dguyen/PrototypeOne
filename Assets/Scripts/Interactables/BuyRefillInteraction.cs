using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyRefillInteraction : MonoBehaviour, IInteractable {
    public GameObject Item;
    public int BuyPrice;
    public int RefillPrice;

    private IEntity Entity;

    void Start() {
        Entity = Item.GetComponent<IEntity>();
        if (Entity == null) {
            Debug.LogError("GameObject Item must have an IEntity type script");
        }
    }

    public void Interact(GameObject Player) {
        Inventory PlayerInventory = Player.GetComponent<Inventory>();
        PlayerMoney PlayerMoney = Player.GetComponent<PlayerMoney>();
        IEntity FoundPlayerItem = PlayerInventory.GetItemEntity(Entity.GetName());

        if(FoundPlayerItem != null) {
            if (FoundPlayerItem.HasCapability(Capability.REFILLABLE)) {
                RangedWeapon tmpCast = (RangedWeapon)FoundPlayerItem;
                if (tmpCast.ammoCapacity == tmpCast.GetAmmoCount()) {
                    Debug.Log("Ammo already full");
                    // Todo: Inform player "Ammo is already full"
                } else if (PlayerMoney.GetPlayerMoney() >= RefillPrice) {
                    Debug.Log("Refilled AMMO");
                    // Todo: Indicate to player that ammo has been refilled
                    tmpCast.RefillAmmo();
                    PlayerMoney.DecreaseMoney(RefillPrice);
                } else {
                    // Todo: Inform player "Funds lacking"
                    Debug.Log("No Money to refill");
                }
            }
        } else if(PlayerMoney.GetPlayerMoney() < BuyPrice) {
            // Todo: Inform player "Funds lacking"
        } else {
            // Todo: Indicate to player that item has been purchased
            Instantiate(Item, transform.position, Quaternion.identity);
            PlayerMoney.DecreaseMoney(BuyPrice);
        }
    }
}
