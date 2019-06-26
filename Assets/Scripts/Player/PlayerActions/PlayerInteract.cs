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
    private IInteractable CurrentInteractable;
    private GameObject CurrentObject;
    private bool hasActed = false;
    private float timer = 0f;
    private bool sliderVisible = false;
    private bool IsInteracting = false;

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
        if (Input.GetButtonDown("Interact_P" + playerControlScheme) && CanDo()) {
            IsInteracting = true;
        }

        if (Input.GetButton("Interact_P" + playerControlScheme) && !hasActed && IsInteracting && CanDo()) {
            if (timer == 0f) {
                SetupSlider();
                DisplaySlider(true);
            } else if (timer >= CurrentInteractable.GetInteractDuration()) {
                hasActed = true;
                Act();
                ResetInteraction();
            }
            UpdateSlider();
            timer += Time.deltaTime;
        } else if (Input.GetButtonUp("Interact_P" + playerControlScheme)) {
            hasActed = false;
            ResetInteraction();
        } else if (sliderVisible) {
            ResetInteraction();
        }
    }

    private void ResetInteraction() {
        CurrentInteractable = null;
        CurrentObject = null;
        IsInteracting = false;
        timer = 0f;
        DisplaySlider(false);
        UpdateSlider();
    }

    private void SetupSlider() {
        if (CurrentInteractable != null) {
            interactSlider.maxValue = CurrentInteractable.GetInteractDuration();
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

    /**
     * Returns true if the given transform is within the given distance to the player
     */
    private bool InRange(Transform other, float distance) {
        return (transform.position - other.transform.position).sqrMagnitude <= distance;
    }

    public bool CanDo() {
        // If currently interacting, check if still in range
        if (CurrentInteractable != null && InRange(CurrentObject.transform, interactRadius)) {
            return true;
        } else {
            CurrentInteractable = GetClosestInteractable();
        }
        return CurrentInteractable != null;
    }

    private IInteractable GetClosestInteractable() {
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
                    CurrentObject = colliders[i].gameObject;
                }
            }
        }
        return closestInteractable;
    }

    public void Act() {
        if (CanDo() && CurrentInteractable != null) {
            CurrentInteractable.Interact(gameObject);
            CurrentInteractable = null;
        }
    }
}
