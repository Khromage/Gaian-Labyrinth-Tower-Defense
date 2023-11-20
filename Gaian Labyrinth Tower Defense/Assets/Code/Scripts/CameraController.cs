using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("KeyBinds")]
    public KeyCode ADS_button = KeyCode.Mouse1;

    public float speedH;
    public float speedV;

    private float yaw = 0f;
    private float pitch = 0f;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
    }

    private Transform cam;

    private Transform player;
    private Transform playerBody;

    private float bodyRotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        speedH = 3f;
        speedV = 3f;

        cam = transform.Find("Main Camera");

        player = transform.parent;
        playerBody = player.Find("Body");
        bodyRotationSpeed = 7f;
    }

    // Update is called once per frame
    void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0f);


        if (Input.GetKey(ADS_button))
        {
            switchCameraStyle(CameraStyle.Combat);
            if (cam.localPosition.x > 1.5f)
            {
                //iterate closer to that position
                cam.localPosition -= new Vector3(.05f, .15f, -.25f);
            }
        }
        else if (Input.GetKeyUp(ADS_button))
        {
            switchCameraStyle(CameraStyle.Basic); 
        }
        else if (cam.localPosition.x < 2f)
        {
            //iterate closer to that position
            cam.localPosition += new Vector3(.05f, .15f, -.25f);
        }


        //if collider (in 2nd child) is hitting ground or towers, (maybe as a trigger instead of collider),
        //move it inward or outward from the player to avoid noclip issues
        //then have it continuously try to return to the default radius

        //Method to call Camera Style Operations
        cameraStyleMethods();
    }


    private void switchCameraStyle(CameraStyle newStyle)
    {
        currentStyle = newStyle;
    }


    private void cameraStyleMethods()
    {
        //rotate player object
        if (currentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = Vector3.Cross(transform.right, playerBody.up) * verticalInput + transform.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                //Debug.Log($"body forward: {playerBody.forward}");
                playerBody.forward = Vector3.Slerp(playerBody.forward, inputDir.normalized, bodyRotationSpeed * Time.deltaTime);
            }

        }
        else if (currentStyle == CameraStyle.Combat)
        {
            //Vector3 dirToCombatLook = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            //orientation.forward = dirToCombatLook.normalized;

            //this should rotate the body around the y axis, as it does now because the rigidbody's rotation is frozen on the x and z axes
            //To add: should also rotate the weapon + head of the player around the y and x? axes, to follow the exact direction of the camera.
            //playerObj.forward = dirToCombatLook.normalized;
        }

    }
}
