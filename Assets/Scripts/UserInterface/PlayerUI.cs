using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI: MonoBehaviour {
    public int PlayerNumber;
    public Slider HealthSlider;
    public Slider InteractSlider;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI AmmoCountText;
    public InventoryUI InventoryUI;
    public Image CrosshairImage;

    // Money related variables
    private Animator moneyAnimator;
    private int oldMoney = 0;
    private int newMoney = 0;
    private bool lerpMoney = false;
    private float lerp = 0f;

    // Ammo related variables
    private Animator ammoAnimator;

    private void Start() {
        AmmoCountText.text = "0";
        moneyAnimator = MoneyText.GetComponent<Animator>();
        ammoAnimator = AmmoCountText.GetComponent<Animator>();
    }

    private void Update() {
        if (lerpMoney) {
            LerpMoney();
        }
    }

    /**
     * Update PlayerMoney, can enable lerping of values
     */
    public void UpdateMoney(int newMoneyValue, bool lerpValues) {
        if (lerpValues) {
            oldMoney = int.Parse(MoneyText.text);
            newMoney = newMoneyValue;   
            lerpMoney = true;
        } else {
            lerpMoney = false;
            MoneyText.text = newMoneyValue.ToString();
        }
    }

    /**
     * Update PlayerAmmo
     */
    public void UpdateAmmo(int newAmmoValue) {
        AmmoCountText.text = newAmmoValue.ToString();
    }

    private void LerpMoney() {
        lerp += Time.deltaTime;
        int lerpedMoney = Mathf.RoundToInt(Mathf.LerpUnclamped(oldMoney, newMoney, lerp / 2));
        MoneyText.text = lerpedMoney.ToString();
        oldMoney = lerpedMoney;
        if (oldMoney == newMoney) {
            lerpMoney = false;
        }   
    }

    /**
     * Animations --------------------------------
     */
    public void MoneyRedFlashAnim() {
        moneyAnimator.SetTrigger("NoFunds");
    }
    
    public void MoneyDeductAnim() {
        moneyAnimator.SetTrigger("NoFunds");
    }

    public void NoAmmoAnim() {
        ammoAnimator.SetTrigger("NoAmmo");
    }
    
    public void RefillAmmoAnim() {
        ammoAnimator.SetTrigger("RefillAmmo");
    }
    /**
     * End Animations ----------------------------
     */
}
