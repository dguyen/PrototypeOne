using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {
    void Interact(GameObject Player);
    bool CanInteract(GameObject Player);
    float GetInteractDuration();
}
