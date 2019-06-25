using UnityEngine;

public class PlayerMoney : MonoBehaviour {
    public int initialPlayerMoney = 500;
    [HideInInspector] public PlayerDetails playerDetails;

    private int playerMoney = 0;

    public void Start() {
        playerDetails = GetComponent<PlayerDetails>();
        playerMoney = initialPlayerMoney;
        if (playerDetails != null) {
            playerDetails.PlayerUI.UpdateMoney(playerMoney, false);
        }
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
            playerDetails.PlayerUI.MoneyRedFlashAnim();
            return false;
        }
        SetMoney(playerMoney - money);
        playerDetails.PlayerUI.MoneyDeductAnim();
        return true;
    }

    private void SetMoney(int money) {
        playerMoney = money;
        if (playerDetails != null) {
            playerDetails.PlayerUI.UpdateMoney(playerMoney, true);
        }
    }
}
