using System;
using UnityEngine;

[Serializable]
public class PlayerManager {
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public GameObject m_PlayerGameObject;

    private PlayerActionManager m_PlayerActionManager;
    private GameObject m_CanvasGameObject;
    private PlayerMovement m_PlayerMovement;
    private PlayerInteract m_PlayerInteract;
    private Inventory m_PlayerInventory;

    public void Setup() {
        m_PlayerActionManager = m_PlayerGameObject.GetComponent<PlayerActionManager>();
        m_PlayerMovement = m_PlayerGameObject.GetComponent<PlayerMovement>();
        m_PlayerInteract = m_PlayerGameObject.GetComponent<PlayerInteract>();
        m_PlayerInventory = m_PlayerGameObject.GetComponent<Inventory>();
        m_PlayerMovement.playerNumber = m_PlayerNumber;
        m_PlayerInteract.playerNumber = m_PlayerNumber;
        m_PlayerInventory.playerNumber = m_PlayerNumber;
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
