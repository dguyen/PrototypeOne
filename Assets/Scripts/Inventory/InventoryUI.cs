using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public Image InventoryImage;

    public void SetImage(Sprite newImage) {
        InventoryImage.sprite = newImage;
        InventoryImage.enabled = true;
    }

    public void RemoveImage() {
        InventoryImage.sprite = null;
        InventoryImage.enabled = false;
    }
}
