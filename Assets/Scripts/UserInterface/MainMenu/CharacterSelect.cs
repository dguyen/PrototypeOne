using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterSelect : MonoBehaviour {
    public int PlayerNumber = 0;
    public bool IsReady = false;
    public bool HasJoined = false;
    public GameObject UnjoinedUI;
    public GameObject SelectCharacterUI;
    public GameObject ReadyButton;
    public Transform PlayerSpawn;
    public GameObject[] PlayerModels;

    private int SelectedCharacter = 0;
    private GameObject InstantiatedPlayer;
    private int TmpPlayerNum = 0;

    void Start() {
        if (HasJoined && PlayerNumber > 0) {
            Setup(PlayerNumber);
        }
    }

    void Update() {
        if (HasJoined && PlayerNumber > 0) {
            if (Input.GetButtonDown("Cancel_P" + PlayerNumber) && IsReady) {
                Unready();
            } else if (Input.GetButtonDown("Cancel_P" + PlayerNumber)) {
                Leave();
            } else if (Input.GetButtonDown("Submit_P" + PlayerNumber) && !IsReady) {
                Ready();
            }
        } else if (TmpPlayerNum > 0) { // Stops Submit being pressed twice initially
            PlayerNumber = TmpPlayerNum;
            TmpPlayerNum = 0;
        }
    }

    /**
     * Setup the player number and updates the UI
     */
    public void Setup(int IPlayerNumber) {
        TmpPlayerNum = IPlayerNumber;
        Join();
    }

    /**
     * Updates the UI to indicate the player is ready
     */
    public void Ready() {
        IsReady = true;
        SelectCharacterUI.SetActive(false);
        ReadyButton.SetActive(true);
    }

    /**
     * Removes the ready status and updates the UI
     */
    public void Unready() {
        IsReady = false;
        ReadyButton.SetActive(false);
        SelectCharacterUI.SetActive(true);
    }

    /**
     * Instantiates a character and updates UI
     */
    public void Join() {
        HasJoined = true;
        InstantiatedPlayer = Instantiate(PlayerModels[SelectedCharacter], PlayerSpawn.position, PlayerSpawn.rotation);        
        foreach (MonoBehaviour script in InstantiatedPlayer.GetComponents<MonoBehaviour>()) {
            script.enabled = false;
        }
        InstantiatedPlayer.SetActive(true);
        UnjoinedUI.SetActive(false);
        SelectCharacterUI.SetActive(true);
    }

    /**
     * Completely remove the player from the menu
     */
    public void Leave() {
        HasJoined = false;
        PlayerNumber = 0;
        Destroy(InstantiatedPlayer);
        SelectCharacterUI.SetActive(false);
        UnjoinedUI.SetActive(true);
    }
}
