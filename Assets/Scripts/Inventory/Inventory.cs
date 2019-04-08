using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public const int numItemSlots = 3;
    public InventoryUI inventoryUI;
    public GameObject itemSpawn;
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
        SelectItem(selectedItem);
    }

    void Update() {
        if (Input.GetButtonDown("Previous_Weapon_P" + playerControlScheme)) {
            if (selectedItem == numItemSlots - 1) {
                SelectItem(0);
            } else {
                SelectItem(selectedItem + 1);
            }
        } else if (Input.GetButtonDown("Next_Weapon_P" + playerControlScheme)) {
            if (selectedItem == 0) {
                SelectItem(numItemSlots - 1);
            } else {
                SelectItem(selectedItem - 1);
            }
        }
    }

    public void SelectItem(int newSelectedItem) {
        if (storedObjects[selectedItem] != null) {
            storedObjects[selectedItem].SetActive(false);
        }

        selectedItem = newSelectedItem;
        UpdateInventoryUI();

        if (storedObjects[selectedItem] != null) {
            storedObjects[selectedItem].SetActive(true);
        }
    }

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

    public GameObject GetSelectedItem() {
        return storedObjects[selectedItem];
    }

    public IEntity GetSelectedEntity() {
        return GetEntity(selectedItem);
    }

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
