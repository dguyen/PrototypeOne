using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject m_PlayerPrefab;
    public GameObject[] m_SpawnPoints;
    public PlayerManager[] m_Players;
    public WaveManager m_WaveManager;
    public GameObject m_LocalMultiCamera;
    public GameObject m_SinglePlayerCamera;

    public UIManager m_UIManager;

    void Start() {
        Player[] Players = PlayerInformation.GetPlayers();
        int DynamicPlayersNum = 0;
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i] != null) {
                DynamicPlayersNum++;
            }
        }

        // Checks if player data is loaded from another scene
        if (DynamicPlayersNum > 0) {
            m_Players = new PlayerManager[DynamicPlayersNum];
            m_UIManager.Setup(DynamicPlayersNum);
            SpawnGlobalPlayers(Players, DynamicPlayersNum);
        } else {
            m_UIManager.Setup(m_Players.Length);
            SpawnScenePlayers();
        }

        // Setup single player camera or local multiplayer camera
        if (m_Players.Length > 1) {
            m_SinglePlayerCamera.SetActive(false);
            m_LocalMultiCamera.SetActive(true);
            SetupLocalMultiplayerCamera();
        } else {
            m_LocalMultiCamera.SetActive(false);
            m_SinglePlayerCamera.SetActive(true);
            SetupSinglePlayerCamera();
        }

        // Setup wave manager
        if (m_WaveManager != null) {
            m_WaveManager.players = m_Players;
            m_WaveManager.StartWaves();
        }
    }

    /**
     * Setup the single player camera
     */
    void SetupSinglePlayerCamera() {
        CameraFollow m_Camera = m_SinglePlayerCamera.GetComponentInChildren<CameraFollow>();
        m_Camera.target = m_Players[0].m_PlayerGameObject.transform;
        m_Camera.SetupCamera();
    }

    /**
     * Setup local multiplayer camera
     */
    void SetupLocalMultiplayerCamera() {
        CameraLocalMultiplayer m_Camera = m_LocalMultiCamera.GetComponentInChildren<CameraLocalMultiplayer>();

        GameObject[] Players = new GameObject[m_Players.Length];
        for (int i = 0; i < m_Players.Length; i++) {
            Players[i] = m_Players[i].m_PlayerGameObject;
        }
        m_Camera.m_Players = Players;
        m_Camera.Setup();
    }

    /**
     * Spawns players with data obtained from another scene
     */
    void SpawnGlobalPlayers(Player[] NewPlayers, int NumPlayers) {
        m_PlayerPrefab.SetActive(false);

        for (int i = 0; i < NumPlayers; i++) {
            SpawnPlayer(NewPlayers[i].PlayerNumber, NewPlayers[i].PlayerControlNumber);
        }
    }

    /**
     * Used for spawning players straight from scene
     */
    void SpawnScenePlayers() {
        m_PlayerPrefab.SetActive(false);
        for (int i = 0; i < m_Players.Length; i++) {
            SpawnPlayer(i + 1, i + 1);
        }
    }

    /**
     * Spawn individual player
     */
    void SpawnPlayer(int i_PlayerNumber, int i_ControlScheme) {
        int index = i_PlayerNumber - 1;
        PlayerManager tmp = new PlayerManager();
        tmp.m_PlayerGameObject = Instantiate(m_PlayerPrefab, m_SpawnPoints[index].transform.position, m_SpawnPoints[index].transform.rotation);
        tmp.m_PlayerNumber = i_PlayerNumber;
        tmp.m_ControlScheme = i_ControlScheme;
        tmp.m_PlayerUI = m_UIManager.GetPlayerUI(index);
        tmp.Setup();
        m_Players[index] = tmp;
    }
}
