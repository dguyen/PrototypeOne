using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuCamera : MonoBehaviour {
    public GameMenuManager gameMenuManager;

    /**
     * Show the character select UI
     */
    public void ShowCharSelectUI() {
        gameMenuManager.ShowCharSelectUI();
    }

    /**
     * Show the Main Menu UI (Start, settings, exit, etc)
     */
    public void ShowMainMenuUI() {
        gameMenuManager.ShowMainMenuUI();
    }
}
