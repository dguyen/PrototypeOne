using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public GameObject m_PlayerPrefab;
    public PlayerManager[] m_Players;
    public WaveManager m_WaveManager;
    public CameraFollow m_Camera;

    void Start() {
        SpawnPlayers();
        m_WaveManager.StartWaves();
        m_Camera.SetupCamera();
    }

    void SpawnPlayers() {
        for (int i = 0; i < m_Players.Length; i++) {
            m_Players[i].m_PlayerGameObject = Instantiate(m_PlayerPrefab, m_Players[i].m_SpawnPoint.position, m_Players[i].m_SpawnPoint.rotation);
            m_Players[i].m_PlayerNumber = i + 1;
            m_Players[i].Setup();
        }
    }
}
