using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour, IAction {
    [HideInInspector] public int playerNumber = 1;
    [HideInInspector] public PlayerDetails playerDetails;

    [Tooltip("How fast the player moves")]
    public float speed = 6f;
 
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

    private Rigidbody playerRigidbody;
    private int floorMask;
    private bool staminaDepleted = false;
    private float camRayLength = 100f;
    private float currentStamina;

    void Awake() {
        floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();

        playerDetails = GetComponent<PlayerDetails>();

        if (playerDetails != null) {
            playerNumber = playerDetails.PlayerNumber;
            staminaSlider = playerDetails.PlayerUI.StaminaSlider;

            staminaSlider.maxValue = staminaLength;
            currentStamina = staminaLength;
            staminaSlider.value = currentStamina;
        }
    }

    void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal_P" + playerNumber);
        float v = Input.GetAxisRaw("Vertical_P" + playerNumber);

        Turning();
        Move(h, v);
        Sprinting();
    }

    void Move(float h, float v) {
        float newSpeed = speed;
        Vector3 movement = new Vector3(h, 0.0f, v);

        if (isSprinting() && currentStamina > 0) {
            newSpeed *= staminaSpeedMultiplier;
            DepleteStamina();
        }
        playerRigidbody.MovePosition(transform.position + (movement.normalized * newSpeed * Time.deltaTime));
    }

    void Turning() {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorhit;

        if(Physics.Raycast(camRay, out floorhit, camRayLength, floorMask)) {
            Vector3 playerToMouse = floorhit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Sprinting() {
        RecoverStamina();
        if (Input.GetButtonDown("Sprint_P" + playerNumber)) {
            DepleteStamina();
        }
        if (Input.GetButtonUp("Sprint_P" + playerNumber)) {
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
        return Input.GetButton("Sprint_P" + playerNumber);
    }

    public bool CanDo() {
        return enabled;
    }

    public void Act() {}
}
