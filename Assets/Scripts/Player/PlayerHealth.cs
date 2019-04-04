using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamagable {
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    [HideInInspector] public PlayerDetails playerDetails;

    private PlayerActionManager playerActionManager;
    private bool isDead;
    private bool damaged;

    void Awake () {
        playerActionManager = GetComponent<PlayerActionManager> ();
        playerDetails = GetComponent<PlayerDetails>();

        healthSlider = playerDetails.PlayerUI.HealthSlider;
        healthSlider.maxValue = startingHealth;
        healthSlider.value = startingHealth;
        currentHealth = startingHealth;
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
        healthSlider.value = currentHealth;
        // Todo: Play damage sound
        if(currentHealth <= 0 && !isDead) {
            Death ();
        }
    }

    void Death () {
        isDead = true;
        playerActionManager.DisableAll();
        // Todo: Play death sound
    }
}
