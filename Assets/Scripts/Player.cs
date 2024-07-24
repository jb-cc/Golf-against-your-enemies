using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class Player : MonoBehaviour
{

    private Rigidbody _rigidbody;
    private LineRenderer _lineRenderer;
    public CinemachineVirtualCameraBase PlayerCamera;
    public Camera MainCamera;
    public float maxHitForce = 100f;
    public float forceAcelleration = 100f;
    private float _currentHitForce;
    private float _pingPongTime;
    public bool canShoot;
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // if the current speed of the player is less than 0.1f, then the player can shoot
        canShoot = _rigidbody.velocity.magnitude < 0.1f;

        if (!canShoot) return;
        
        // set velocity and rotation to zero
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
            // deactivate the player camera (no rotation)
            PlayerCamera.gameObject.SetActive(false);
            
            // set the line renderer's start position to the player's position
            _lineRenderer.SetPosition(0, this.transform.position);
            _lineRenderer.enabled = true;
            
            _currentHitForce = 0f;
            _pingPongTime = 0f;
        }
    }
    
    private void ProcessOnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PlayerCamera.gameObject.SetActive(true);
            _lineRenderer.enabled = false;
            
            Vector3 cameraForward = MainCamera.transform.forward;
            Vector3 hitDirection = new Vector3(cameraForward.x, 0, cameraForward.z) * _currentHitForce;
            _rigidbody.AddForce(hitDirection, ForceMode.Impulse);
        }
    }
    
    private void ProcessOnMouseHold()
    {
        if (Input.GetMouseButton(0))
        {
            _pingPongTime += Time.deltaTime;
            _currentHitForce = Mathf.PingPong(_pingPongTime * forceAcelleration, maxHitForce);
            
            // Drawing a line from the player to the direction the camera is facing (set the end of the line to that direction)
            Vector3 cameraForward = MainCamera.transform.forward;
            Vector3 playerPosition = this.transform.position;
            Vector3 hitDirection = playerPosition + new Vector3(cameraForward.x, 0, cameraForward.z) * _currentHitForce;
            _lineRenderer.SetPosition(1, hitDirection);
        }
    }
}
