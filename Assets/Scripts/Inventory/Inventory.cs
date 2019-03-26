using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public const int numItemSlots = 3;
    public Image[] inventoryImages = new Image[numItemSlots];
    public GameObject itemSpawn;

    private IEntity[] entities = new IEntity[numItemSlots];
    private GameObject[] storedObjects = new GameObject[numItemSlots];
    private int selectedItem = 0;

    void Start() {
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

        if (inventoryImages[selectedItem]) {
            inventoryImages[selectedItem].color = new Color32(160, 255, 255, 255);
            selectedItem = newSelectedItem;
            inventoryImages[selectedItem].color = new Color32(212, 100, 50, 255);
        }

        if (storedObjects[selectedItem] != null) {
            storedObjects[selectedItem].SetActive(true);
        }
    }

    public bool AddItem(GameObject itemGameObject) {
        IEntity entity = itemGameObject.GetComponent<IEntity>();
        if (entity == null || !entity.HasCapability(Capability.PICKABLE)) {
            return false;
        }
        for (int i = 0; i < entities.Length; i++) {
            if (entities[i] == null) {

                Rigidbody tmp = itemGameObject.GetComponent<Rigidbody>(); 
                if (tmp != null) {
                    Destroy(tmp);
                }

                entities[i] = entity;
                storedObjects[i] = itemGameObject;
                inventoryImages[i].sprite = entity.GetSprite();
                inventoryImages[i].enabled = true;
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

    public bool CanDrop() {
        if (entities[selectedItem] == null || storedObjects[selectedItem] == null) {
            return false;
        }
        return entities[selectedItem].HasCapability(Capability.DROPABLE);
    }

    public void DropSelectedItem() {
        DropItem(selectedItem);
    }

    public void DropItem(int itemToDrop) {
        if (!CanDrop()) {
            return;
        }

        GameObject dropItem = storedObjects[selectedItem];

        if (RemoveItem(entities[itemToDrop])) {
            dropItem.transform.parent = null;
            Rigidbody dropItemRb = dropItem.AddComponent<Rigidbody>();
            dropItemRb.drag = 5;
            dropItemRb.mass = 2.5f;
            dropItemRb.AddRelativeForce(new Vector3(0, 1, 3) * 1000);
        }
    }

    public bool RemoveItem(IEntity entitiyToRemove) {
        if (!entitiyToRemove.HasCapability(Capability.DROPABLE)) {
            return false;
        }
        for (int i = 0; i < entities.Length; i++) {
            if (entities[i] == entitiyToRemove) {
                entities[i] = null;
                storedObjects[i] = null;
                inventoryImages[i].sprite = null;
                inventoryImages[i].enabled = false;
                return true;
            }
        }
        return false;
    }

    public GameObject GetSelectedItem() {
        return storedObjects[selectedItem];
    }

    public IEntity GetSelectedEntity() {
        return entities[selectedItem];
    }

    public IEntity GetItemEntity(string itemName) {
        foreach (var entity in entities) {
            if (entity != null && entity.GetName() == itemName) {
                return entity;
            }           
        }
        return null;
    }
}
