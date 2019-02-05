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
        if (damaged) {
            damageImage.color = flashColour;
        } else {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
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
