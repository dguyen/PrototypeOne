using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject m_PlayerPrefab;
    public GameObject[] m_SpawnPoints;
    public PlayerManager[] m_Players;
    public WaveManager m_WaveManager;
    public CameraFollow m_Camera;
    public UIManager m_UIManager;

    void Start() {
        Player[] Players = PlayerInformation.GetPlayers();
        int DynamicPlayersNum = 0;
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i] != null) {
                DynamicPlayersNum++;
            }
        }

        if (DynamicPlayersNum > 0) {
            m_Players = new PlayerManager[DynamicPlayersNum];
            m_UIManager.Setup(DynamicPlayersNum);
            SpawnGlobalPlayers(Players, DynamicPlayersNum);

            // Todo: Setup local multiplayer camera

        } else {
            m_UIManager.Setup(m_Players.Length);
            SpawnScenePlayers();
            m_Camera.SetupCamera();
        }

        // No waves for test level

        // Todo: Fix wave manager to obtain health from all players
        if (m_WaveManager != null) {
            m_WaveManager.players = m_Players;
            m_WaveManager.StartWaves();
        }
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
