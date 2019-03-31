using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public Image[] InventoryImages;
    public Color32 SelectColor = new Color32(212, 100, 50, 255);
    public Color32 DeselectedColor = new Color32(160, 255, 255, 255);

    public void Select(int index) {
        for (int i = 0; i < InventoryImages.Length; i++) {
            Deselect(i);
        }
        InventoryImages[index].color = SelectColor;
    }

    public void Deselect(int index) {
        InventoryImages[index].color = DeselectedColor;
    }

    public void SetImage(int index, Sprite newImage) {
        InventoryImages[index].sprite = newImage;
        InventoryImages[index].enabled = true;
    }

    public void RemoveImage(int index) {
        if (!ImageExists(index)) {
            return;
        }
        InventoryImages[index].sprite = null;
        InventoryImages[index].enabled = false;
    }

    bool ImageExists(int index) {
        return InventoryImages[index] != null;
    }
}
