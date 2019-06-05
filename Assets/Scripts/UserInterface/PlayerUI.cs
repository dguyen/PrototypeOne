using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI: MonoBehaviour {
    public int PlayerNumber;
    public Slider HealthSlider;
    public Slider StaminaSlider;
    public Slider InteractSlider;
    public Text MoneyText;
    public Text AmmoCountText;
    public InventoryUI InventoryUI;
    public Image CrosshairImage;

    void Start() {
        AmmoCountText.text = "0/0";
    }
   
}
