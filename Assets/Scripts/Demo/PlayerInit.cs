using UnityEngine;

public class PlayerInit : MonoBehaviour {
    public GameObject Player;
    public CameraFollow CameraFollow;
    public PlayerDetails PlayerDetails;

    void Awake() {
        CameraFollow.SetupCamera();
        if (PlayerDetails != null) {
            PlayerDetails pDetails = Player.AddComponent<PlayerDetails>();
            pDetails.PlayerControlScheme = PlayerDetails.PlayerControlScheme;
            pDetails.PlayerNumber = PlayerDetails.PlayerNumber;
            pDetails.PlayerUI = PlayerDetails.PlayerUI;
        }
    }
}
