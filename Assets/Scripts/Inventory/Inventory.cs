using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public const int numItemSlots = 3;
    public Image[] inventoryImages = new Image[numItemSlots];

    private IEntity[] entities = new IEntity[numItemSlots];

    void Update() {
        // Show the selected weapon
    }

    public bool AddItem(IEntity entityToAdd) {
        if (!entityToAdd.HasCapability(Capability.PICKABLE)) {
            return false;
        }
        for (int i = 0; i < entities.Length; i++) {
            if (entities[i] == null) {
                entities[i] = entityToAdd;
                inventoryImages[i].sprite = entityToAdd.GetSprite();
                inventoryImages[i].enabled = true;
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(IEntity entitiyToRemove) {
        if (!entitiyToRemove.HasCapability(Capability.DROPABLE)) {
            return;
        }
        for (int i = 0; i < entities.Length; i++) {
            if (entities[i] == entitiyToRemove) {
                entities[i] = null;
                inventoryImages[i].sprite = null;
                inventoryImages[i].enabled = false;
                return;
            }
        }
    }
}
