﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public const int numItemSlots = 3;
    public InventoryUI inventoryUI;
    public GameObject itemSpawn;
    public int playerNumber = 1;

    private GameObject[] storedObjects = new GameObject[numItemSlots];
    private int selectedItem = 0;
    private PlayerMoney playerMoney;

    void Start() {
        playerMoney = GetComponent<PlayerMoney>();
        if (inventoryUI == null) {
            inventoryUI = GameObject.Find("Inventory").GetComponent<InventoryUI>();
        }
        SelectItem(selectedItem);
    }

    void Update() {
        if (Input.GetAxis("Mouse ScrollWheel") == -0.1f) {
            if (selectedItem == numItemSlots - 1) {
                SelectItem(0);
            } else {
                SelectItem(selectedItem + 1);
            }
        } else if (Input.GetAxis("Mouse ScrollWheel") == 0.1f) {
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
        inventoryUI.Select(selectedItem);

        if (storedObjects[selectedItem] != null) {
            storedObjects[selectedItem].SetActive(true);
        }
    }

    public bool AddItem(GameObject itemGameObject) {
        IEntity entity = itemGameObject.GetComponent<IEntity>();
        if (entity == null) {
            return false;
        }
        ((Entity)entity).playerNumber = playerNumber;
        ((Weapon)entity).playerMoney = playerMoney;

        for (int i = 0; i < storedObjects.Length; i++) {
            if (storedObjects[i] == null) {
                // Store the item and update inventory UI
                storedObjects[i] = itemGameObject;
                inventoryUI.SetImage(i, entity.GetSprite());

                // Set the location and rotation ?
                itemGameObject.transform.parent = itemSpawn.transform;
                itemGameObject.transform.localPosition = new Vector3();
                itemGameObject.transform.rotation = new Quaternion();

                itemGameObject.SetActive(false);
                if (i == selectedItem) {
                    SelectItem(i);
                }

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
                inventoryUI.RemoveImage(i);
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
}
