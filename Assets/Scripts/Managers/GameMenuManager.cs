using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuManager : MonoBehaviour {
    public Animator CameraAnimator;
    public Animator CanvasAnimator;

    /**
     * Shift the camera view towards the main menu area (start, settings, exit, etc)
     */
    public void ShowGameMenu() {
        CanvasAnimator.SetTrigger("HideCharPanel");
        CameraAnimator.SetTrigger("GameMenuSelect");
    }

    /**
     * Shift the camera view towards the character select area
     */
    public void ShowCharacterSelect() {
        CameraAnimator.SetTrigger("CharacterSelect");
        CanvasAnimator.SetTrigger("HideMainPanel");
    }

    /*
     * ShowMainMenuUI will be called when camera animation finishes via animation event
     */
    public void ShowMainMenuUI() {
        CanvasAnimator.SetTrigger("ShowMainPanel");
    }

    /*
     * ShowCharSelectUI will be called when camera animation finishes via animation event
     */
    public void ShowCharSelectUI() {
        CanvasAnimator.SetTrigger("ShowCharPanel");
    }
}
