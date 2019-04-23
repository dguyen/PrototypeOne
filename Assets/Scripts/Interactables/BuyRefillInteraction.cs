using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyRefillInteraction : MonoBehaviour, IInteractable {
    public GameObject Item;
    public int BuyPrice;
    public int RefillPrice;
    public Indicator PIndicator;

    private IEntity Entity;

    void Start() {
        Entity = Item.GetComponent<IEntity>();
        if (Entity == null) {
            Debug.LogError("GameObject Item must have an IEntity type script");
        }
        if (PIndicator != null) {
            PIndicator.IndicatorText.text = "$" + BuyPrice.ToString() + "/" + RefillPrice.ToString();
            if (Entity.GetSprite() != null) {
                PIndicator.SpriteImage.sprite = Entity.GetSprite();
            }
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
                    // Todo: Inform player "Ammo is already full"
                    // Flash ammo UI?
                } else if (PlayerMoney.GetPlayerMoney() >= RefillPrice) {
                    // Todo: Indicate to player that ammo has been refilled
                    // Flash ammo UI?
                    tmpCast.RefillAmmo();
                    PlayerMoney.DecreaseMoney(RefillPrice);
                } else {
                    // Todo: Inform player "Funds lacking"
                    // Flash money red? or indicator red?
                }
            }
        } else if(PlayerMoney.GetPlayerMoney() < BuyPrice) {
            // Todo: Inform player "Funds lacking"
            // Flash money red? or indicator red?
        } else {
            // Todo: Indicate to player that item has been purchased
            PlayerInventory.AddItem(Instantiate(Item, transform.position, Quaternion.identity));
            PlayerMoney.DecreaseMoney(BuyPrice);
        }
    }
}
