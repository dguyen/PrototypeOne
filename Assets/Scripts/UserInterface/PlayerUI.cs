using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI: MonoBehaviour {
    public int PlayerNumber;
    public Slider HealthSlider;
    public Slider StaminaSlider;
    public Slider InteractSlider;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI AmmoCountText;
    public InventoryUI InventoryUI;
    public Image CrosshairImage;

    void Start() {
        AmmoCountText.text = "0/0";
    }
}
