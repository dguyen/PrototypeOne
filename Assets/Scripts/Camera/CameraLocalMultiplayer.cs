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
        if (IsSetup) {
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
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Players.Length; i++)   {
            if (!PlayerValid(i)) {
                continue;
            }

            averagePos += m_Players[i].transform.position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;
        m_DesiredPosition = averagePos;
    }

    private void Zoom() {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }

    private float FindRequiredSize() {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < m_Players.Length; i++) {
            if (!PlayerValid(i)) {
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

    /**
     * Checks if player is within camera view and if they are alive
     */
    private bool PlayerValid(int playerRef) {
        if (m_PlayerHealths[playerRef].currentHealth <= 0) {
            return false;
        }
        if (!PlayerVisible(m_Players[playerRef].transform.position)) {
            return false;
        }
        return true;
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
