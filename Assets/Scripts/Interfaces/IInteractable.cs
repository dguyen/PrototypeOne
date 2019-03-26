using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    // The GameObject that is initiating the interaction
    void Interact(GameObject Player);
}
