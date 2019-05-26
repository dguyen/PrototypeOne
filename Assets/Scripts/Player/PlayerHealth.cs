using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamagable {
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    [HideInInspector] public PlayerDetails playerDetails;

    private PlayerActionManager playerActionManager;
    private bool isDead;
    private bool damaged;

    void Awake () {
        playerActionManager = GetComponent<PlayerActionManager> ();
        playerDetails = GetComponent<PlayerDetails>();
        currentHealth = startingHealth;

        if (playerDetails != null) {
            healthSlider = playerDetails.PlayerUI.HealthSlider;
        }
        UpdateHealthSlider();
    }

    void Update () {
        if (damaged) {
            // Todo: Indicate damage
        }
        damaged = false;
    }

    public void TakeDamage (int amount) {
        if (isDead) return;
        damaged = true;
        currentHealth -= amount;
        UpdateHealthSlider();
        // Todo: Play damage sound
        if(currentHealth <= 0 && !isDead) {
            Death ();
        }
    }

    /**
     * Update the Player's health slider
     */
    public void UpdateHealthSlider() {
        if (healthSlider != null) {
            healthSlider.maxValue = startingHealth;
            healthSlider.value = currentHealth;
        }
    }

    void Death () {
        isDead = true;
        playerActionManager.DisableAll();
        // Todo: Play death sound
    }
}
