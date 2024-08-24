using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class Player : MonoBehaviour
{
    private GameManager _gameManager;
    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    public CinemachineFreeLook PlayerCamera;
    public Camera MainCamera;
    public float maxHitForce = 1f;
    public float minHitForce = 0.1f;
    public float dragSpeed = 0.1f; // Speed at which the force changes
    private float _currentHitForce;
    private bool canShoot;
    private Vector3 initialFollowPosition;
    private Vector3 initialLookAtPosition;

    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
       // Cursor.lockState = CursorLockMode.Locked;
        Debug.Log("Player Awake, Starting Position: " + transform.position);
    }

    private void Update()
    {
        // if the current speed of the player is less than 0.1f, then the player can shoot
        canShoot = _rigidbody.velocity.magnitude < 0.1f;

        if (!canShoot) return;
        
        // Check if the player has reached the max number of hits, if yes, load next level
        _gameManager.CheckMaxHits();
        
        //else, set velocity and rotation to zero
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;

        ProcessOnMouseDown();
        ProcessOnMouseUp();
        ProcessOnMouseHold();
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        _rigidbody.MovePosition(newPosition);
    }

    private void ProcessOnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Lock the camera Y position
            initialFollowPosition = PlayerCamera.Follow.localPosition;
            initialLookAtPosition = PlayerCamera.LookAt.localPosition;

            // deactivate the player camera (no rotation)
            PlayerCamera.enabled = false;

            // set the line renderer's start position to the player's position
            _lineRenderer.SetPosition(0, this.transform.position);
            _lineRenderer.enabled = true;

            _currentHitForce = minHitForce; // Start with the minimum force
        }
    }

    private void ProcessOnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Enable the player camera
            PlayerCamera.enabled = true;

            // Re-enable camera Y movement
            PlayerCamera.Follow.localPosition = initialFollowPosition;
            PlayerCamera.LookAt.localPosition = initialLookAtPosition;

            _lineRenderer.enabled = false;

            Vector3 cameraForward = MainCamera.transform.forward;
            Vector3 hitDirection = new Vector3(cameraForward.x, 0, cameraForward.z) * _currentHitForce;
            _gameManager.IncreaseHitCounter();
            _rigidbody.AddForce(hitDirection, ForceMode.Impulse);
        }
    }

    private void ProcessOnMouseHold()
    {
        if (Input.GetMouseButton(0))
        {
            // Adjust the hit force based on mouse movement
            _currentHitForce += Input.GetAxis("Mouse Y") * dragSpeed;
            _currentHitForce = Mathf.Clamp(_currentHitForce, minHitForce, maxHitForce);

            // Debug log to check the clamped value
            //Debug.Log($"Current Hit Force: {_currentHitForce}, Clamped: {Mathf.Clamp(_currentHitForce, minHitForce, maxHitForce)}");

            // Drawing a line from the player to the direction the camera is facing (set the end of the line to that direction)
            Vector3 cameraForward = MainCamera.transform.forward;
            Vector3 playerPosition = this.transform.position;
            Vector3 hitDirection = playerPosition + new Vector3(cameraForward.x, 0, cameraForward.z) * _currentHitForce;
            _lineRenderer.SetPosition(1, hitDirection);

            // Lock Y position while allowing horizontal rotation
            PlayerCamera.Follow.localPosition = new Vector3(PlayerCamera.Follow.localPosition.x, initialFollowPosition.y, PlayerCamera.Follow.localPosition.z);
            PlayerCamera.LookAt.localPosition = new Vector3(PlayerCamera.LookAt.localPosition.x, initialLookAtPosition.y, PlayerCamera.LookAt.localPosition.z);
        }
    }
}
