using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Indicator : MonoBehaviour {
    public Animator IndicatorAC;
    public TextMeshProUGUI IndicatorText;
    public Image SpriteImage;

    private int NumViewers = 0;
    private bool IsShown = false;

    void Update() {
        if (NumViewers > 0 && !IsShown) {
            ShowIndicator();
        } else if (NumViewers <= 0 && IsShown) {
            HideIndicator();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            NumViewers++;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            NumViewers--;
        }
    }

    void ShowIndicator() {
        IsShown = true;
        IndicatorAC.SetTrigger("FadeIn");
    }

    void HideIndicator() {
        IsShown = false;
        IndicatorAC.SetTrigger("FadeOut");
    }
}
