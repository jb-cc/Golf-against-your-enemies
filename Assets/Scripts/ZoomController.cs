using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float maxFOV = 50f;
    public float minFOV = 10f;
    public float zoomSpeed = 10f;

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            AdjustFOV(scrollInput);
        }
    }

    void AdjustFOV(float scrollInput)
    {
        float currentFOV = freeLookCamera.m_Lens.FieldOfView;
        float newFOV = Mathf.Clamp(currentFOV - scrollInput * zoomSpeed, minFOV, maxFOV);
        freeLookCamera.m_Lens.FieldOfView = newFOV;
    }
}