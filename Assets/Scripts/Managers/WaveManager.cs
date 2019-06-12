using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour {
    [Tooltip("The delay before the initial wave starts")]       
    public float initialDelay = 5f;
    
    [Tooltip("The delay before the next wave starts")]       
    public float waveDelay = 5f;

    [Tooltip("The delay before the game over scene plays")]       
    public float gameOverDelay = 1f;

    [Tooltip("Text to indicate the current wave")]   
    public TextMeshProUGUI waveText;

    [Tooltip("Text to indicate the current wave")]   
    public TextMeshProUGUI gameOverText;

    [HideInInspector]
    public PlayerManager[] players;

    [Tooltip("Player health")]   
    public SpawnManager spawnManager;

    private int currentWave = 0;
    private WaitForSeconds initialStartDelay;
    private WaitForSeconds waveEndWait;
    private PlayerActionManager playerActionManager;
    private bool waveInitiated = false;
    private PlayerHealth[] playerHealths;

    void Start() {
        initialStartDelay = new WaitForSeconds(initialDelay);
        waveEndWait = new WaitForSeconds(waveDelay);
    }

    public void StartWaves() {
        if (!waveInitiated) {
            playerHealths = new PlayerHealth[players.Length];
            for (int i = 0; i < playerHealths.Length; i++) {
                playerHealths[i] = players[i].m_PlayerGameObject.GetComponent<PlayerHealth>();
            }

            StartCoroutine(GameLoop());
            waveInitiated = true;
        }
    }

    IEnumerator GameLoop() {
        yield return StartCoroutine(WaveStarting());
        yield return StartCoroutine(WavePlaying());
        yield return StartCoroutine(WaveEnding());

        if (PlayersDead()) {
            GameOver();
        } else {
            StartCoroutine(GameLoop());
        }
    }

    IEnumerator WaveStarting() {
        currentWave++;
        waveText.text = "WAVE " + currentWave;
        yield return null;
    }

    IEnumerator WavePlaying() {
        spawnManager.SpawnNext(currentWave);

        while (!PlayersDead() && spawnManager.EnemiesLeft() > 0) {
            yield return null;
        }
    }

    IEnumerator WaveEnding() {
        if (PlayersDead()) {
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
        return message + "<color=#" + ColorUtility.ToHtmlStringRGB(new Color32(225, 123, 112, 225)) + "><size=20>WAVE: " + currentWave + "</size></color>";
    }

    /**
     * Returns true if all players are dead, false otherwise
     */
    bool PlayersDead() {
        foreach (var pHealth in playerHealths) {
            if (pHealth.currentHealth > 0) {
                return false;
            }
        }
        return true;
    }
}
