using UnityEngine;

public class CameraLocalMultiplayer : MonoBehaviour {
    public float m_DampTime = 0.2f;                 
    public float m_ScreenEdgeBuffer = 4f;           
    public float m_MinSize = 3.5f;                  
    public float m_MaxSize = 5.0f;                  
    [HideInInspector] public GameObject[] m_Players; 

    private Camera m_Camera;                        
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;                 
    private Vector3 m_DesiredPosition;              
    private PlayerHealth[] m_PlayerHealths; 
    private bool IsSetup = false;

    private void Awake() {
        m_Camera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate() {
        if (IsSetup && GetNumberAlive() != 0) {
            Move();
            Zoom();
        }
    }

    /**
     * Setup the camera, m_Players must be filled
     */
    public void Setup() {
        m_PlayerHealths = new PlayerHealth[m_Players.Length];
        for (int i = 0; i < m_Players.Length; i++) {
            m_PlayerHealths[i] = m_Players[i].GetComponent<PlayerHealth>();
        }
        IsSetup = true;
    }

    private void Move() {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }

    private void FindAveragePosition() {
        int focusedPlayer = -1;

        // Obtains the player closest to the center of all players
        if (GetNumberAlive() > 2) {
            GameObject CenterPlayer = GetClosestPlayer(GetAverageVector());
            for (int i = 0; i < m_Players.Length; i++) {
                if (m_Players[i] == CenterPlayer) {
                    focusedPlayer = i;
                    break;
                }
            }
        }

        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Players.Length; i++) {
            if (!IsPlayerAlive(i)) {
                continue;
            } else if (focusedPlayer < 0) {
                focusedPlayer = i;
            } else if (!IsPlayerVisible(i) && i != focusedPlayer) {
                continue;
            }

            averagePos += m_Players[i].transform.position;
            numTargets++;
        }

        if (numTargets > 0) {
            averagePos /= numTargets;
        }

        averagePos.y = transform.position.y;
        m_DesiredPosition = averagePos;
    }

    private Vector3 GetAverageVector() {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Players.Length; i++) {
            /**
             * Can check if player is alive here and exclude them from calculations
             * However may cause camera to shift away from a fight if a player dies mid fight

                if (!IsPlayerAlive(i)) {
                    continue;
                }
             */
            averagePos += m_Players[i].transform.position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        return averagePos;
    }

    /**
     * Returns the number of players alive
     */
    public int GetNumberAlive() {
        int numPlayers = 0;
        for (int i = 0; i < m_PlayerHealths.Length; i++) {
            if (m_PlayerHealths[i].currentHealth > 0) {
                numPlayers++;
            }
        }
        return numPlayers;
    }

    /**
     * Returns the closest alive player from a given transform
     */
    GameObject GetClosestPlayer(Vector3 CurrentPosition) {
        GameObject ClosestPlayer = null;
        float ClosestDistanceSqr = Mathf.Infinity;
        GameObject tmp = null;

        for (int i = 0; i < m_Players.Length; i++) {
            if (!IsPlayerAlive(i)) {
                continue;
            }
            tmp = m_Players[i];

            Transform TmpTransform = m_Players[i].transform;
            Vector3 DirectionToTarget = TmpTransform.position - CurrentPosition;
            float DSqrToTarget = DirectionToTarget.sqrMagnitude;
            if(DSqrToTarget < ClosestDistanceSqr) {
                ClosestDistanceSqr = DSqrToTarget;
                ClosestPlayer = m_Players[i];
            }
        }

        if (GetNumberAlive() <= 2) {
            return tmp;
        }

        return ClosestPlayer;
    }

    private void Zoom() {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }

    private float FindRequiredSize() {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < m_Players.Length; i++) {
            if (!IsPlayerAlive(i)) {
                continue;
            }

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Players[i].transform.position);
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));
            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);
        }
        
        size += m_ScreenEdgeBuffer;
        size = Mathf.Max(size, m_MinSize);
        size = Mathf.Min(size, m_MaxSize);

        return size;
    }

    private bool IsPlayerAlive(int playerRef) {
        return m_PlayerHealths[playerRef].currentHealth > 0;
    }

    private bool IsPlayerVisible(int playerRef) {
        return PlayerVisible(m_Players[playerRef].transform.position);
    }

    private bool PlayerVisible(Vector3 position) {
        Vector3 playerPos = m_Camera.WorldToViewportPoint(position);

        if (playerPos.y < 0 || playerPos.y > 1 || playerPos.x < 0 || playerPos.x > 1 && playerPos.z > 0) {
            return false;
        }
        return true;
    }

    public void SetStartPositionAndSize() {
        FindAveragePosition();

        transform.position = m_DesiredPosition;
        m_Camera.orthographicSize = FindRequiredSize();
    }
}
