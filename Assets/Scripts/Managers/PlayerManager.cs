using System;
using UnityEngine;

[Serializable]
public class PlayerManager {
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public GameObject m_PlayerGameObject;

    private PlayerActionManager m_PlayerActionManager;
    private GameObject m_CanvasGameObject;

    public void Setup() {
        m_PlayerActionManager = m_PlayerGameObject.GetComponent<PlayerActionManager>();
    }

    public void EnableControl() {
        m_PlayerActionManager.EnableAll();
    }

    public void DisableControl() {
        m_PlayerActionManager.DisableAll();
    }

    public void Reset() {
        m_PlayerGameObject.transform.position = m_SpawnPoint.position;
        m_PlayerGameObject.transform.rotation = m_SpawnPoint.rotation;

        m_PlayerGameObject.SetActive(false);
        m_PlayerGameObject.SetActive(true);
    }
}
