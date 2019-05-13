using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public const int numItemSlots = 3;
    public InventoryUI inventoryUI;
    public GameObject itemSpawn;
    public GameObject startingItem;
    [HideInInspector] public int playerControlScheme = 1;
    [HideInInspector] public PlayerDetails playerDetails;

    private GameObject[] storedObjects = new GameObject[numItemSlots];
    private int selectedItem = 0;
    private PlayerMoney playerMoney;

    void Start() {
        playerMoney = GetComponent<PlayerMoney>();
        playerDetails = GetComponent<PlayerDetails>();
        playerControlScheme = playerDetails.PlayerControlScheme;
        inventoryUI = playerDetails.PlayerUI.InventoryUI;
        if (startingItem != null) {
            AddItem(Instantiate(startingItem));
        } else {
            SelectItem(selectedItem);
        }
    }

    void Update() {
        if (Input.GetButtonDown("Previous_Weapon_P" + playerControlScheme)) {
            SelectPreviousAvailableItem();
        } else if (Input.GetButtonDown("Next_Weapon_P" + playerControlScheme)) {
            SelectNextAvailableItem();
        }
    }

    /**
     * Returns true if the specified item slot is not empty
     */
    public bool ItemExists(int itemSlot) {
        return itemSlot >= 0 && itemSlot < storedObjects.Length && storedObjects[itemSlot] != null;
    }

    /**
     * Select an item based on the given slot parameter
     */
    public void SelectItem(int newSelectedItem) {
        if (!ItemExists(newSelectedItem)) {
            return;
        } else if (storedObjects[selectedItem] != null) {
            storedObjects[selectedItem].SetActive(false);
        }

        selectedItem = newSelectedItem;
        UpdateInventoryUI();

        if (storedObjects[selectedItem] != null) {
            storedObjects[selectedItem].SetActive(true);
        }
    }

    /**
     * Selects the next non-null item in the inventory
     */
    public void SelectNextAvailableItem() {
        for (int i = 1; i < storedObjects.Length; i++) {
            int tmp = selectedItem + i;
            if (tmp >= storedObjects.Length) {
                tmp = storedObjects.Length - tmp;
            }
            if (ItemExists(tmp)) {
                SelectItem(tmp);
                return;
            }
        }
    }

    /**
     * Selects the previous non-null item in the inventory
     */
    public void SelectPreviousAvailableItem() {
        for (int i = 1; i < storedObjects.Length; i++) {
            int tmp = selectedItem - i;
            if (tmp < 0) {
                tmp = storedObjects.Length + tmp - 1;
            }
            if (ItemExists(tmp)) {
                SelectItem(tmp);
                return;
            }
        }
    }

    /**
     * Add a GameObject to the inventory
     */
    public bool AddItem(GameObject itemGameObject) {
        IEntity entity = itemGameObject.GetComponent<IEntity>();
        if (entity == null) {
            return false;
        }
        ((Entity)entity).playerControlScheme = playerControlScheme;
        ((Weapon)entity).playerMoney = playerMoney;
        ((Weapon)entity).ammoCountText = playerDetails.PlayerUI.AmmoCountText;
        ((Weapon)entity).inventory = this;

        for (int i = 0; i < storedObjects.Length; i++) {
            if (storedObjects[i] == null) {
                storedObjects[i] = itemGameObject;
                itemGameObject.transform.parent = itemSpawn.transform;
                itemGameObject.transform.localPosition = new Vector3();
                itemGameObject.transform.rotation = new Quaternion();
                SelectItem(i);
                return true;
            }
        }
        return false;
    }

    /**
     * Remove a GameObject from the inventory
     */
    public bool RemoveItem(GameObject objectToRemove) {
        IEntity entity = objectToRemove.GetComponent<IEntity>();        
        if (entity == null) {
            return false;
        }

        for (int i = 0; i < storedObjects.Length; i++) {
            if (storedObjects[i] == objectToRemove) {
                storedObjects[i] = null;
                return true;
            }
        }
        return false;
    }

    /**
     * Returns the currently selected item
     */
    public GameObject GetSelectedItem() {
        return storedObjects[selectedItem];
    }

    /**
     * Returns the Entity of the currently selected item
     */
    public IEntity GetSelectedEntity() {
        return GetEntity(selectedItem);
    }

    /**
     * Returns the Entity correlating to the given itemName if found
     */
    public IEntity GetItemEntity(string itemName) {
        for (int i = 0; i < storedObjects.Length; i++) {
            IEntity tmpEntity = GetEntity(i);
            if (tmpEntity != null && tmpEntity.GetName() == itemName) {
                return tmpEntity;
            }           
        }
        return null;
    }

    private IEntity GetEntity(int index) {
        GameObject tmpObject = storedObjects[index];
        if (tmpObject != null) {
            return tmpObject.GetComponent<IEntity>();
        }
        return null;
    }

    private void UpdateInventoryUI() {
        if (storedObjects[selectedItem] == null) {
            inventoryUI.RemoveImage();
        } else {
            Sprite sprite = storedObjects[selectedItem].GetComponent<IEntity>().GetSprite();
            if (sprite != null) {
                inventoryUI.SetImage(sprite);
            }
        }
    }
}
