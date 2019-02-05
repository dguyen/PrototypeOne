using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;

    PlayerActionManager playerActionManager;
    bool isDead;
    bool damaged;

    void Start() {
        healthSlider.maxValue = startingHealth;
        healthSlider.value = startingHealth;
    }

    void Awake () {
        playerActionManager = GetComponent<PlayerActionManager> ();
        currentHealth = startingHealth;
    }

    void Update () {
        if(damaged) {
            // Todo: Indicate damage
        }
        else {
            // Todo: Clear damage indicator up
        }
        damaged = false;
    }

    public void TakeDamage (int amount) {
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
