using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAreaInteraction : MonoBehaviour, IInteractable {
    public Area AreaToOpen;
    public int CostToOpen;
    public Indicator PIndicator;
    public float InteractDuration = 0.5f;

    void Start() {
        if (PIndicator != null) {
            PIndicator.IndicatorText.text = CostToOpen.ToString();
        }
    }

    public void Interact(GameObject Player) {
        PlayerMoney PMoney = Player.GetComponent<PlayerMoney>();

        if (PMoney.DecreaseMoney(CostToOpen)) {
            AreaToOpen.Open();
            RemoveBlockage();
        } else {
            // Todo: Inform player "Funds lacking"
        }
    }

    /**
     * Returns the amount of time required to complete this interaction
     */
    public float GetInteractDuration() {
        return InteractDuration;
    }

    /**
     * Returns true if given player can interact with this
     */
    public bool CanInteract(GameObject Player) {
        PlayerMoney PMoney = Player.GetComponent<PlayerMoney>();
        return PMoney.GetPlayerMoney() >= CostToOpen;
    }

    void RemoveBlockage() {
        gameObject.SetActive(false);
    }
}
