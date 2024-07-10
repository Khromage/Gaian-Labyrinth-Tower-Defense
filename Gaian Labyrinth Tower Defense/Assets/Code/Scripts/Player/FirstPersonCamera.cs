using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class FirstPersonCamera : CinemachineExtension
{
    [SerializeField] private float horizontalSpeed = 10.0f;
    [SerializeField] private float verticalSpeed = 10.0f;
    [SerializeField] private float clampAngle = 70.0f;
    public float mouseSpeedModifier = 0.01f;

    public GameObject player;
    
    public Transform playerBody;
    public Transform playerHead;
    public Transform orientation;
    public Transform combatLookAt;
    public GameObject PlayerCam;

    private Vector3 startingRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                Player playerScript = player.GetComponent<Player>();
                if(playerScript.currentMode != Player.playerMode.Menu)
                {
                    if (startingRotation == Vector3.zero)
                        startingRotation = transform.localRotation.eulerAngles;

                    float mouseX = Input.GetAxis("Mouse X");
                    float mouseY = Input.GetAxis("Mouse Y");
                    // 800 affects sensitivity of mouse movement can change to a variable for settings adjustment later
                    startingRotation.x += mouseX * 800 * Time.deltaTime;
                    startingRotation.y += mouseY * 800 * Time.deltaTime;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                    // Calculate camera rotation based on mouse input while keeping the starting rotation in mind
                    Quaternion rotation = Quaternion.Euler(-mouseY * (mouseSpeedModifier * verticalSpeed) * Time.deltaTime, mouseX * (mouseSpeedModifier * horizontalSpeed) * Time.deltaTime, 0f);

                    // Apply the rotation to the camera's current orientation
                    state.RawOrientation *= rotation;

                    // Rotate the player object
                    playerBody.localRotation = Quaternion.Euler(0f, startingRotation.x, 0f);
                    
                    // Rotate the playerHead to match the camera's orientation while also keeping the player object's rotation in mind
                    playerHead.transform.position = state.RawPosition;

                    // Rotate the orientation object
                    orientation.localRotation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);

                    // Rotate the combatLookAt object
                    Vector3 dirToCombatLook = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
                    combatLookAt.localRotation = Quaternion.LookRotation(dirToCombatLook, Vector3.up); 
                }
            }
        }
    }
}
