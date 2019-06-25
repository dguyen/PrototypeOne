using UnityEngine;

public class BuyRefillInteraction : MonoBehaviour, IInteractable {
    public GameObject Item;
    public int BuyPrice;
    public int RefillPrice;
    public Indicator PIndicator;
    public float InteractDuration = 0.4f;

    private IEntity Entity;

    void Start() {
        Entity = Item.GetComponent<IEntity>();
        if (Entity == null) {
            Debug.LogError("GameObject Item must have an IEntity type script");
        }
        if (PIndicator != null) {
            PIndicator.IndicatorText.text = BuyPrice.ToString() + "/" + RefillPrice.ToString();
            if (Entity.GetSprite() != null) {
                PIndicator.SpriteImage.sprite = Entity.GetSprite();
            }
        }
    }

    public void Interact(GameObject Player) {
        if (CanInteract(Player)) {
            Inventory PInventory = Player.GetComponent<Inventory>();
            PlayerMoney PMoney = Player.GetComponent<PlayerMoney>();
            IEntity FoundPlayerItem = PInventory.GetItemEntity(Entity.GetName());

            if (FoundPlayerItem != null && FoundPlayerItem.HasCapability(Capability.REFILLABLE)) {
                RangedWeapon tmpCast = (RangedWeapon)FoundPlayerItem;
                tmpCast.RefillAmmo();
                // Todo: Indicate to player that ammo has been refilled
                // Flash ammo UI?
            } else {
                PInventory.AddItem(Instantiate(Item, transform.position, Quaternion.identity));
                PMoney.DecreaseMoney(BuyPrice);
                // Todo: Indicate to player that item has been purchased
            }
        }
    }

    public float GetInteractDuration() {
        return InteractDuration;
    }

    public bool CanInteract(GameObject Player) {
        PlayerDetails PlayerDets = Player.GetComponent<PlayerDetails>();
        Inventory PInventory = Player.GetComponent<Inventory>();
        PlayerMoney PMoney = Player.GetComponent<PlayerMoney>();
        IEntity FoundPlayerItem = PInventory.GetItemEntity(Entity.GetName());

        if (FoundPlayerItem != null && FoundPlayerItem.HasCapability(Capability.REFILLABLE)) {
            RangedWeapon tmpCast = (RangedWeapon)FoundPlayerItem;
            if (tmpCast.ammoCapacity <= tmpCast.GetAmmoCount()) {
                // Todo: Inform player "Ammo is already full"
                // Flash ammo UI?
                return false;
            } else if (PMoney.GetPlayerMoney() >= RefillPrice) {
                // Player has enough money and can refill
                return true;
            } else {
                // Player lacking funds
                PlayerDets.PlayerUI.MoneyRedFlashAnim();
                return false;
            }
        } else if(PMoney.GetPlayerMoney() < BuyPrice) {
            // Player lacking funds
            PlayerDets.PlayerUI.MoneyRedFlashAnim();
            return false;
        } else {
            // Player has enough money and can purchase Entity
            return true;
        }
    }
}
