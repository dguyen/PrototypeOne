using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour, IAction {
    [HideInInspector] public int playerControlScheme = 1;
    [HideInInspector] public PlayerDetails playerDetails;

    [Tooltip("How fast the player moves")]
    public float speed = 6f;
 
    public bool canMove = true;
    public bool canSprint = true;

    [Tooltip("Slider to indicate stamina value")]   
    public Slider staminaSlider;

    [Tooltip("How much extra speed the player gets")]   
    public float staminaSpeedMultiplier = 1.5f;

    [Tooltip("How long the player can use stamina for")]   
    public float staminaLength = 3;

    [Tooltip("How fast stamina recovers")]
    public float staminaRecoverySpeed = 1;

    [Tooltip("How fast stamina depletes")]
    public float staminaDepletionSpeed = 1.5f;

    public Image crosshair; 
    public float crosshairDistance = 2.5f; 
    public Transform firingHeight;

    private Rigidbody playerRigidbody;
    private int floorMask;
    private bool staminaDepleted = false;
    private float camRayLength = 100f;
    private float currentStamina;
    private Camera m_Camera;

    void Awake() {
        floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();
        playerDetails = GetComponent<PlayerDetails>();
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (playerDetails != null) {
            playerControlScheme = playerDetails.PlayerControlScheme;
            staminaSlider = playerDetails.PlayerUI.StaminaSlider;
            crosshair = playerDetails.PlayerUI.CrosshairImage;
            UpdateCrosshair();
        }

        staminaSlider.maxValue = staminaLength;
        currentStamina = staminaLength;
        staminaSlider.value = currentStamina;
    }

    void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal_P" + playerControlScheme);
        float v = Input.GetAxisRaw("Vertical_P" + playerControlScheme);

        Turning();
        if (canMove) {
            Move(h, v);
        }
        if (canSprint) {
            Sprinting();
        }
    }

    void Move(float h, float v) {
        float newSpeed = speed;
        Vector3 movement = new Vector3(h, 0.0f, v);

        if (canSprint && isSprinting() && currentStamina > 0) {
            newSpeed *= staminaSpeedMultiplier;
            DepleteStamina();
        }
        playerRigidbody.MovePosition(transform.position + (movement.normalized * newSpeed * Time.deltaTime));
    }

    void Turning() {
        /**
         * If using mouse, use raycasting to turn
         * Todo: Remove keyboard support in future for console versions
         */
        if (playerControlScheme == 1) {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorhit;

            if(Physics.Raycast(camRay, out floorhit, camRayLength, floorMask)) {
                Vector3 playerToMouse = floorhit.point - transform.position;
                playerToMouse.y = 0f;
                Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

                if (playerToMouse.sqrMagnitude > 0.0f) {
                    transform.rotation = newRotation;
                }
            }
        } else {
            Vector3 playerDirection = Vector3.right * Input.GetAxisRaw("Mouse_X_P" + playerControlScheme) + Vector3.forward * Input.GetAxisRaw("Mouse_Y_P" + playerControlScheme);
            if (playerDirection.sqrMagnitude > 0.0f) {
                transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            }
        }
        UpdateCrosshair();
    }

    void UpdateCrosshair() {
        if (crosshair != null) {
            Vector2 tmp = m_Camera.WorldToScreenPoint(firingHeight.transform.position + gameObject.transform.forward * crosshairDistance);
            crosshair.transform.position = Vector2.Lerp(crosshair.transform.position, tmp, 0.6f);
        }
    }

    void Sprinting() {
        RecoverStamina();
        if (Input.GetButtonDown("Sprint_P" + playerControlScheme)) {
            DepleteStamina();
        }
        if (Input.GetButtonUp("Sprint_P" + playerControlScheme)) {
            staminaDepleted = false;
        }
    }

    void RecoverStamina() {
        if (currentStamina <= staminaLength && !staminaDepleted) {
            currentStamina += Time.deltaTime * staminaRecoverySpeed;
            staminaSlider.value = currentStamina;
        }
    }

    void DepleteStamina() {
        if (currentStamina >= 0) {
            currentStamina -= Time.deltaTime * staminaDepletionSpeed;
            staminaSlider.value = currentStamina;
        } else if (currentStamina == 0) {
            staminaDepleted = true;
        } 
    }

    bool isSprinting() {
        return Input.GetButton("Sprint_P" + playerControlScheme);
    }

    void OnEnable() {
        if (crosshair != null) {
            crosshair.enabled = true;
        }
    }

    void OnDisable() {
        if (crosshair != null) {
            crosshair.enabled = false;
        }
    }

    public bool CanDo() {
        return enabled;
    }

    public void Act() {}
}
