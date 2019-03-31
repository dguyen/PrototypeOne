using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour {
    [Tooltip("The delay before the initial wave starts")]       
    public float initialDelay = 5f;
    
    [Tooltip("The delay before the next wave starts")]       
    public float waveDelay = 5f;

    [Tooltip("The delay before the game over scene plays")]       
    public float gameOverDelay = 1f;

    [Tooltip("Text to indicate the current wave")]   
    public Text waveText;

    [Tooltip("Text to indicate the current wave")]   
    public Text gameOverText;
    
    [Tooltip("Player health")]   
    public PlayerHealth playerHealth;

    [Tooltip("Player health")]   
    public SpawnManager spawnManager;

    private int currentWave = 0;
    private WaitForSeconds initialStartDelay;
    private WaitForSeconds waveEndWait;
    private PlayerActionManager playerActionManager;
    private bool waveInitiated = false;

    void Start() {
        initialStartDelay = new WaitForSeconds(initialDelay);
        waveEndWait = new WaitForSeconds(waveDelay);
    }

    public void StartWaves() {
        if (!waveInitiated) {
            if (playerHealth == null) {
                playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
            }
            StartCoroutine(GameLoop());
            waveInitiated = true;
        }
    }

    IEnumerator GameLoop() {
        yield return StartCoroutine(WaveStarting());
        yield return StartCoroutine(WavePlaying());
        yield return StartCoroutine(WaveEnding());

        if (playerHealth.currentHealth <= 0) {
            GameOver();
        } else {
            StartCoroutine(GameLoop());
        }
    }

    IEnumerator WaveStarting() {
        currentWave++;
        waveText.text = "Wave " + currentWave;
        yield return null;
    }

    IEnumerator WavePlaying() {
        spawnManager.SpawnNext(currentWave);

        while (playerHealth.currentHealth > 0 && spawnManager.EnemiesLeft() > 0) {
            yield return null;
        }
    }

    IEnumerator WaveEnding() {
        if (playerHealth.currentHealth <= 0) {
            yield return new WaitForSeconds(gameOverDelay);
        } else {
            yield return waveEndWait;
        }
    }

    void GameOver() {
        waveText.text = "";
        gameOverText.text = GameOverMessage();
    }

    string GameOverMessage() {
        string message = "GAME OVER\n";
        return message + "<color=#" + ColorUtility.ToHtmlStringRGB(new Color32(225, 123, 112, 225)) + "><size=20>Wave: " + currentWave + "</size></color>";
    }
}
