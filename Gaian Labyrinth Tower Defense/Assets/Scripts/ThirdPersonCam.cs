using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCamera;
    public GameObject combatCam;

    public CameraStyle currentStyle;
    public enum CameraStyle {
        Basic,
        Combat,
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Mouse1)) {
            switchCameraStyle(CameraStyle.Combat);
        }
        else { switchCameraStyle(CameraStyle.Basic); }

        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //Method to call Camera Style Operations
        cameraStyleMethods();
    }


    private void switchCameraStyle(CameraStyle newStyle) {
        thirdPersonCamera.SetActive(false);
        combatCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) { thirdPersonCamera.SetActive(true); }
        if (newStyle == CameraStyle.Combat) { combatCam.SetActive(true); }

        currentStyle = newStyle;
    }

    private void cameraStyleMethods() {
        //rotate player object
        if (currentStyle == CameraStyle.Basic) {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero) {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }

        }
        else if (currentStyle == CameraStyle.Combat) {
            Vector3 dirToCombatLook = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLook.normalized;

            playerObj.forward = dirToCombatLook.normalized;
        }

    }
}
