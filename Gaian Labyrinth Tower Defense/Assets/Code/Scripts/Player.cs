using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void TowerPlaced(GridTile tileOn);
    public static event TowerPlaced OnTowerPlaced;

    public playerMode currentMode;

    //Variables to control and determine player's jumping abiltiy
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    //Variables to be used to check if player is on the ground
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask Grid;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    [Header("Player Cam")]
    public GameObject playerCam;

    [Header("Build mode")]
    public GameObject towerPrefab;

    public int currency;

    //The Modes the Player will be in, Combat = with weapons, Build = ability to edit towers
    public enum playerMode
    {
        Combat,
        Build,
    }

    //Method to be checked on first frame of the game
    public void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        currency = 80;
    }

    //Method to be checked on every frame of the game
    public void Update() 
     {
        //Checking which mode the player is currently in
        if ((Input.GetKeyDown(KeyCode.Tab)) && (currentMode != playerMode.Build))
        {
            currentMode = playerMode.Build;

            Debug.Log("Build");
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        { 
            currentMode = playerMode.Combat;
            Debug.Log("Combat");
        }

        getUserKeyInput();
        playerSpeedControl();

        //Checking if player is on the ground by sending a Raycast down to see if layer whatIsGround is hit
        grounded = Physics.Raycast(transform.position + new Vector3(0, 0.05f, 0), Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        //handling player drag if on the ground
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        //Actions player can do depending on mode they are in
        if (currentMode == playerMode.Combat)
        {
            attack();
        }
        else if (currentMode == playerMode.Build)
        {
            placeTowers();
            //maybe also display outlines of the grid tiles so the player has some idea of where towers can be placed.
        }
    }

    //Methods to be executed when user inputs keys
    public void FixedUpdate() 
    {
        movePlayer();
    }

    //Getting WASD and jump inputs
    private void getUserKeyInput() 
    {
        //Player hits WASD
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Player wants to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
        } 
    }

    //Method to move the player on ground and in air 
    private void movePlayer() 
    {
        //calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //if on the ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    //Method to set a limit to the players velocity
    private void playerSpeedControl() 
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit player's movement velocity when reaching max speed
        if(flatVel.magnitude > moveSpeed) 
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed; 
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void jump() 
    {
        //reset Y velocity to prepare for new jump
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void resetJump()
    {
        //Reset readytoJump to True so player can jump again
        readyToJump = true;
    }

    private void attack()
    {
        //Empty
    }

    private void placeTowers()
    {
        //here is where we should display an outline of the currently selected tower, either a green transparent silhouette if placeable, or red.

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
            if ((Physics.Raycast(ray, out RaycastHit hit, 100f, Grid))) 
            {
                if ((hit.transform.tag.Equals("GridTile")) )
                {
                    GridTile currTileScript = hit.transform.GetComponent<GridTile>();
                    if (currTileScript.placeable) {
                        if (currency >= towerPrefab.GetComponent<TowerBehavior>().cost)
                        {
                            Debug.Log(hit.transform.position.x);
                            Vector3 towerPlacement = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                            GameObject currTower = Instantiate(towerPrefab, towerPlacement, transform.rotation);
                            TowerBehavior tower = currTower.GetComponent<TowerBehavior>();

                            currTileScript.placeable = false;
                            currTileScript.walkable = false;

                            Debug.Log($"Currency before tower placement: {currency}");
                            currency -= tower.cost;
                            Debug.Log($"Currency after tower placement: {currency}");

                            OnTowerPlaced?.Invoke(currTileScript);
                        }
                        else
                        {
                            Debug.Log("We require more Vespene Gas");
                        }
                    }

                }
            }   
        }
    }

    private void GainCurrency(GameObject enemyWhoDied)
    {
        currency += enemyWhoDied.GetComponent<EnemyBehavior>().worth;
    }

    private void OnEnable()
    {
        EnemyBehavior.OnEnemyDeath += GainCurrency;
    }
    private void OnDisable()
    {
        EnemyBehavior.OnEnemyDeath -= GainCurrency;
    }
}
