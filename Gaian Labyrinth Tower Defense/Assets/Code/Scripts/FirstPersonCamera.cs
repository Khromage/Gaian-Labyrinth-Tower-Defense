using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Numerics;
using System.Runtime.CompilerServices;

public class FirstPersonCamera : CinemachineExtension
{

    [SerializeField]
    private float horizantalSpeed = 10.0f;
    [SerializeField]
    private float verticalSpeed = 10.0f;

    [SerializeField]
    private float clampAngle = 80.0f;
    public Transform playerObj;
    public Transform head;

    private UnityEngine.Vector3 startingRotation;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow){
            if (stage == CinemachineCore.Stage.Aim)
            {
                if(startingRotation == null )
                    startingRotation = transform.localRotation.eulerAngles;

                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                //800 affects sentivity of mouse movement can change to variable for settings to change later
                startingRotation.x += mouseX * 800 * Time.deltaTime;
                startingRotation.y += mouseY * 800 * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                //Affects the rotation of the camera
                state.RawOrientation = UnityEngine.Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);

                
            }
        }    
    }
}
