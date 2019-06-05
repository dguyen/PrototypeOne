using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour, IAction {
    [HideInInspector] public int playerControlScheme = 1;
    [HideInInspector] public PlayerDetails playerDetails;
    
    public float interactRadius = 2f;
    public Slider interactSlider;
    public float sliderXOffset = 45f;
    public float sliderYOffset = 80f;

    private Camera m_Camera;
    private Animator sliderAnimator;
    private IInteractable tmpInteractable;
    private bool hasActed = false;
    private float timer = 0f;
    private bool sliderVisible = false;

    void Awake() {
        playerDetails = GetComponent<PlayerDetails>();
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        if (playerDetails != null) {
            playerControlScheme = playerDetails.PlayerControlScheme;
            interactSlider = playerDetails.PlayerUI.InteractSlider;
        }
        if (interactSlider != null) {
            sliderAnimator = interactSlider.GetComponent<Animator>();
        }
    }

    void Update() {
        UpdateSliderPosition();
        if (Input.GetButton("Interact_P" + playerControlScheme) && CanDo() && !hasActed) {
            if (timer == 0f) {
                SetupSlider();
                DisplaySlider(true);
            } else if (timer >= tmpInteractable.GetInteractDuration()) {
                hasActed = true;
                Act();
                timer = 0f;
                DisplaySlider(false);
            }
            UpdateSlider();
            timer += Time.deltaTime;
        } else if (Input.GetButtonUp("Interact_P" + playerControlScheme)) {
            timer = 0f;
            hasActed = false;
            UpdateSlider();
            if (tmpInteractable != null) {
                DisplaySlider(false);
            }
        }
    }

    private void SetupSlider() {
        if (tmpInteractable != null) {
            interactSlider.maxValue = tmpInteractable.GetInteractDuration();
        }
        interactSlider.value = 0;
    }

    private void UpdateSlider() {
        interactSlider.value = timer;
    }

    private void DisplaySlider(bool Show) {
        if (interactSlider == null || sliderVisible == Show) {
            return;
        }
        sliderVisible = Show;  
        sliderAnimator.SetBool("IsVisible", Show);
    }

    private void UpdateSliderPosition() {
        if (interactSlider == null) {
            return;
        }

        Vector2 tmp = m_Camera.WorldToScreenPoint(gameObject.transform.position);
        Vector2 newPos = tmp + new Vector2(sliderXOffset, sliderYOffset); // PlayerPos + Offset
        interactSlider.transform.position = Vector2.Lerp(interactSlider.transform.position, newPos, 0.6f);
    }

    public bool CanDo() {
        Collider[] colliders = Physics.OverlapSphere (transform.position, interactRadius);
        IInteractable closestInteractable = null;
        float minSqrDistance = Mathf.Infinity;

        for(int i = 0; i < colliders.Length; i++) {
            IInteractable foundInteractable = colliders[i].GetComponent<IInteractable>();
            if (foundInteractable != null) {
                float sqrDistanceToCenter = (transform.position - colliders[i].transform.position).sqrMagnitude;
                if (sqrDistanceToCenter < minSqrDistance && foundInteractable.CanInteract(gameObject)) {
                    minSqrDistance = sqrDistanceToCenter;
                    closestInteractable = foundInteractable;
                }
            }
        }
        tmpInteractable = closestInteractable;
        return tmpInteractable != null;
    }

    public void Act() {
        if (CanDo() && tmpInteractable != null) {
            tmpInteractable.Interact(gameObject);
            tmpInteractable = null;
        }
    }
}
