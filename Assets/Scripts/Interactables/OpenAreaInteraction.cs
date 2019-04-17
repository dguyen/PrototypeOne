using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAreaInteraction : MonoBehaviour, IInteractable {
    public Area AreaToOpen;
    public int CostToOpen;

    public void Interact(GameObject Player) {
        PlayerMoney PMoney = Player.GetComponent<PlayerMoney>();

        if (PMoney.DecreaseMoney(CostToOpen)) {
            AreaToOpen.Open();
            RemoveBlockage();
        } else {
            // Todo: Inform player "Funds lacking"
        }
    }

    void RemoveBlockage() {
        gameObject.SetActive(false);
    }
}
