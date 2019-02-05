using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrop : MonoBehaviour
{
    public KeyCode dropKey = KeyCode.C;

    private Inventory inventory;

    void Start() {
        inventory = FindObjectOfType<Inventory>();
    }

    void Update() {
        if (Input.GetKeyDown(dropKey)) {
            Act();
        }
    }

    public bool CanDo() {
        return inventory.CanDrop();
    }

    public void Act() {
        if (CanDo()) {
            inventory.DropSelectedItem();
        }
    }
}
