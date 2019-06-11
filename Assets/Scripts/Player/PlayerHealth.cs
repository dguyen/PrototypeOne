using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamagable {
    [HideInInspector] public PlayerDetails playerDetails;
    [HideInInspector] public int currentHealth;

    public int startingHealth = 100;
    public Slider healthSlider;
    public bool CanRevive = true;

    private PlayerActionManager playerActionManager;
    private bool isDead;
    private bool damaged;

    void Awake() {
        playerActionManager = GetComponent<PlayerActionManager> ();
        playerDetails = GetComponent<PlayerDetails>();
        currentHealth = startingHealth;

        if (playerDetails != null) {
            healthSlider = playerDetails.PlayerUI.HealthSlider;
        }
        UpdateHealthSlider();
    }

    void Update() {
        if (damaged) {
            // Todo: Indicate damage
        }
        damaged = false;
    }


    /**
     * Deal damage to the player
     */
    public void TakeDamage(int amount) {
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

    /**
     * Kill the player
     */    
    public void Death () {
        currentHealth = 0;
        isDead = true;
        UpdateHealthSlider();
        playerActionManager.DisableAll();
        if (CanRevive) {
            gameObject.AddComponent<ReviveInteraction>();
        }
        // Todo: Play death sound
    }

    /**
     * Revive Player with a given health
     */
    public void Revive(int revivedHealth) {
        isDead = false;
        currentHealth = revivedHealth;
        UpdateHealthSlider();
        playerActionManager.EnableAll();
    }
}
