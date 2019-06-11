using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveInteraction : MonoBehaviour, IInteractable {
    public float InteractDuration = 2f;
    public float RespawnHealthPerecent = 0.50f;

    public void Interact(GameObject Player) {
        PlayerHealth deadPlayer = GetComponent<PlayerHealth>();
        deadPlayer.Revive(Mathf.RoundToInt(deadPlayer.startingHealth * RespawnHealthPerecent));
        Destroy(gameObject.GetComponent<ReviveInteraction>());
    }

    public float GetInteractDuration() {
        return InteractDuration;
    }

    public bool CanInteract(GameObject Player) {
        PlayerHealth PHealth = gameObject.GetComponent<PlayerHealth>();
        return PHealth != null && PHealth.currentHealth <= 0 && PHealth.CanRevive;
    }
}
