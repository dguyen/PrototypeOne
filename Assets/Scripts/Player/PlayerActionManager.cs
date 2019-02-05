using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionManager : MonoBehaviour {
    private Inventory inventory;

    void Start() {
        inventory = FindObjectOfType<Inventory>();        
    }

    /*
     * Enable specific action on Player
     */
    public void EnableAction(PlayerAction action) {
        IAction tmp = (IAction) gameObject.GetComponent(action.ToString());
        if (tmp != null) {
            tmp.enabled = true;
        }
    }

    /*
     * Disable specific action on Player
     */
    public void DisableAction(PlayerAction action) {
        IAction tmp = (IAction) gameObject.GetComponent(action.ToString());
        if (tmp != null) {
            tmp.enabled = false;
        }
    }

    /*
     * Disable all actions on Player
     */
    public void DisableAll() {
        IAction[] actions = gameObject.GetComponents<IAction>();
        foreach (var action in actions) {
            action.enabled = false;
        }
        inventory.enabled = false;
    }

    /*
     * Enable all actions on Player
     */
    public void EnableAll() {
        IAction[] actions = gameObject.GetComponents<IAction>();
        foreach (var action in actions) {
            action.enabled = true;
        }
        inventory.enabled = true;
    }
}
