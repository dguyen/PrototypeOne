using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour {
    public int initialPlayerMoney = 500;
    public Text moneyText;
    [HideInInspector] public PlayerDetails playerDetails;

    private int playerMoney = 0;

    public void Start() {
        playerDetails = GetComponent<PlayerDetails>();
        if (playerDetails != null) {
            moneyText = playerDetails.PlayerUI.MoneyText;
        }
        playerMoney = initialPlayerMoney;
        UpdateUI();
    }

    public int GetPlayerMoney() {
        return playerMoney;
    }

    public void IncreaseMoney(int money) {
        if (money <= 0) {
            return;
        }
        SetMoney(playerMoney + money);
    }

    public bool DecreaseMoney(int money) {
        if (playerMoney - money < 0 || money <= 0) {
            return false;
        }
        SetMoney(playerMoney - money);
        return true;
    }

    private void SetMoney(int money) {
        playerMoney = money;
        UpdateUI();
    }

    private void UpdateUI() {
        if (moneyText) {
            moneyText.text = playerMoney.ToString();
        }
    }
}
