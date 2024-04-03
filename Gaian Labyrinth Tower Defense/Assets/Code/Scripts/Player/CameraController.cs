using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // The NPC's transform
    public float zoomFOV = 40f; // The zoomed-in FOV
    public float normalFOV = 60f; // The normal FOV
    public float zoomSpeed = 5f; // Speed of the zoom effect

    private Camera cam;
    private float targetFOV;
    private bool isZoomed = false; //is this bool even used in anything?

    void Start()
    {
        cam = GetComponent<Camera>();
        targetFOV = cam.fieldOfView;
    }

    void Update()
    {
        // Smoothly interpolate the camera's FOV to the target FOV
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    public void ZoomIn()
    {
        targetFOV = zoomFOV;
        isZoomed = true;
    }

    public void ZoomOut()
    {
        targetFOV = normalFOV;
        isZoomed = false;
    }
}