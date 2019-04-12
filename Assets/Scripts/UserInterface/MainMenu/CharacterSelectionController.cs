using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectionController : MonoBehaviour {
    public CharacterSelect[] Characters = new CharacterSelect[4];
    public int GameStartCountdown = 3;
    public GameObject LoadingBanner;

    private bool GameStarting = false;
    private Coroutine COCountdown;
    private Animator LoadingBannerAnim;
    private Text LoadingBannerText;

    void Start() {
        LoadingBannerAnim = LoadingBanner.GetComponent<Animator>();
        LoadingBannerText = LoadingBanner.GetComponentInChildren<Text>();
    }

    void Update() {
        bool GameReady = PlayersReady();
        if (GameReady && !GameStarting) {
            GameStarting = true;
            COCountdown = StartCoroutine(StartCountdown());
        } else if (!GameReady && GameStarting) {
            GameStarting = false;
            if (COCountdown != null) {
                StopCoroutine(COCountdown);
            }
            LoadingBannerAnim.SetTrigger("HideBanner");
        }

        for (int i = 0; i < Characters.Length; i++) {
            PlayerJoin(i + 1);
        }
    }

    /**
     * Checks if controller submit button is pressed. If true, create character
     */
    private void PlayerJoin(int IPlayerNumber) {
        if (Input.GetButtonDown("Submit_P" + IPlayerNumber)) {
            int EmptySlot = -1;
            for (int i = 0; i < Characters.Length; i++) {
                if (Characters[i].PlayerNumber == 0 && EmptySlot < 0) {
                    EmptySlot = i;
                }
                if (Characters[i].PlayerNumber == IPlayerNumber) {
                    return;
                }
            }
            if (EmptySlot >= 0) {
                Characters[EmptySlot].Setup(IPlayerNumber);
            }
        }
    }

    /**
     * Returns true if all joined players are ready
     */
    private bool PlayersReady() {
        int numberReady = 0;
        int joinedNotReady = 0;
        foreach (var character in Characters) {
            if (character.HasJoined && character.IsReady) {
                numberReady++;
            }
            if (character.HasJoined && !character.IsReady) {
                joinedNotReady++;
            }
        }
        return numberReady > 0 && joinedNotReady <= 0;
    }

    /**
     * Begins the countdown to start the game
     */
    private IEnumerator StartCountdown() {
        LoadingBannerAnim.SetTrigger("ShowBanner");

        for (int i = GameStartCountdown; 0 < i; i--) {
            LoadingBannerText.text = "Starting in " + i;
            yield return new WaitForSeconds(1);
        }

        Player[] Players = new Player[4];
        int PlayerCount = 0;
        for (int i = 0; i < Characters.Length; i++) {
            Player TmpPlayer = new Player();
            if (Characters[i].PlayerNumber != 0) {
                TmpPlayer.PlayerNumber = i + 1;
                TmpPlayer.PlayerControlNumber = Characters[i].PlayerNumber;
                PlayerInformation.SetPlayer(PlayerCount, TmpPlayer);
                PlayerCount++;
            }
        }

        SceneManager.LoadScene(1);
    }
}
