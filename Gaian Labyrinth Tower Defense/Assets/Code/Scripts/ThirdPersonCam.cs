using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("KeyBinds")]
    public KeyCode aDSButton = KeyCode.Mouse1;

    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCamera;
    public GameObject combatCam;

    public GameObject currentCam;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentCam = thirdPersonCamera;
    }

    private void Update()
    {
        //Swicth Camera style to ADS mode if right mouse button is held down
        if (Input.GetKey(aDSButton))
        {
            switchCameraStyle(CameraStyle.Combat);
        }
        else if (Input.GetKeyUp(aDSButton)) 
            { switchCameraStyle(CameraStyle.Basic); }

        //rotate orientation
        //Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        //orientation.forward = viewDir.normalized;
        //playerObj.transform.forward = orientation.forward;

        //other attempts
        //player.transform.RotateAround(player.transform.position, player.transform.up, 90 - Vector3.Angle(player.transform.forward, orientation.right)); 
        //orientation.forward = Vector3.forward;

        Vector3 dirToCombatLook = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
        orientation.forward = dirToCombatLook.normalized;

        playerObj.forward = dirToCombatLook.normalized;

        //Method to call Camera Style Operations
        cameraStyleMethods();
    }


    private void switchCameraStyle(CameraStyle newStyle)
    {
        thirdPersonCamera.SetActive(false);
        combatCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) 
        { 
            thirdPersonCamera.SetActive(true);
            currentCam = thirdPersonCamera;
        }
        if (newStyle == CameraStyle.Combat) 
        { 
            combatCam.SetActive(true);
            currentCam = combatCam;
        }

        currentStyle = newStyle;
    }

    private void cameraStyleMethods()
    {
        //rotate player object
        if (currentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            //Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            Vector3 inputDir = Vector3.Cross(thirdPersonCamera.transform.right, player.up) * verticalInput 
                + Vector3.Cross(player.up, thirdPersonCamera.transform.forward) * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                //playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, rotationSpeed * Time.deltaTime);
            }

        }
        else if (currentStyle == CameraStyle.Combat)
        {
            //Vector3 dirToCombatLook = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            //orientation.forward = dirToCombatLook.normalized;

            //playerObj.forward = dirToCombatLook.normalized;
        }

    }
}
