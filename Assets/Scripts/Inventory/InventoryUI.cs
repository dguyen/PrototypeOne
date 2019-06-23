using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public Image[] inventoryImages;

    [Tooltip("The amount to fade deselected items")]
    public Color32 deselectOpacity = new Color(255, 255, 255, 150);

    private int selectedImage = 0;
    private Color32 fullColor = new Color(255, 255, 255, 255);
    private Color32 noColor = new Color(255, 255, 255, 0);

    /**
     * Highlight the selected item
     */
    public void SelectItem(int position) {
        inventoryImages[selectedImage].color = deselectOpacity;
        inventoryImages[position].color = fullColor;
        selectedImage = position;
    }

    /**
     * Set the image of the item in the given position
     */
    public void SetImage(Sprite newImage, int position) {
        inventoryImages[position].sprite = newImage;
        inventoryImages[position].color = deselectOpacity;
    }

    /**
     * Remove the image of the item in the given position
     */
    public void RemoveImage(int position) {
        inventoryImages[position].color = noColor;
        inventoryImages[position].sprite = null;
    }
}
