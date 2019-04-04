using System;
using UnityEngine;

[Serializable]
public class PlayerManager {
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public PlayerUI m_PlayerUI;
    [HideInInspector] public GameObject m_PlayerGameObject;

    private PlayerActionManager m_PlayerActionManager;
    private PlayerDetails m_PlayerDetails;
    private GameObject m_CanvasGameObject;
    private PlayerMovement m_PlayerMovement;
    private PlayerInteract m_PlayerInteract;
    private Inventory m_PlayerInventory;

    public void Setup() {
        m_PlayerActionManager = m_PlayerGameObject.GetComponent<PlayerActionManager>();
        m_PlayerDetails = m_PlayerGameObject.AddComponent<PlayerDetails>();
        m_PlayerDetails.PlayerNumber = m_PlayerNumber;
        m_PlayerDetails.PlayerUI = m_PlayerUI;
        m_PlayerGameObject.SetActive(true);
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
