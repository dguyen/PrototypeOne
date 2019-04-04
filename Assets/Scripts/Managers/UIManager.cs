using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public GameObject[] UIPrefab = new GameObject[4];

    private PlayerUI[] PlayerUIs = new PlayerUI[4];
    private GameObject InstantiatedUI;

    public void Setup(int numberOfPlayers) {
        if (numberOfPlayers > 4 || numberOfPlayers < 1) {
            Debug.LogError("Number of players must be within 1-4");
        }
        
        InstantiatedUI = Instantiate(UIPrefab[numberOfPlayers - 1]);
        InstantiatedUI.transform.SetParent(transform, false);
        PlayerUIs = InstantiatedUI.GetComponentsInChildren<PlayerUI>();
    }

    public PlayerUI GetPlayerUI(int playerNumber) {
        if (PlayerUIs[playerNumber] == null) {
            return null;
        }
        return PlayerUIs[playerNumber].GetComponent<PlayerUI>();
    }
}
