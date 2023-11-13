using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBehavior
{
    public delegate void TowerPlaced(GridTile tileOn);
    public static event TowerPlaced OnTowerPlaced;

    public delegate void TowerSold(GridTile tileOn);
    public static event TowerSold OnTowerSold;

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
    public KeyCode nextWeapon = KeyCode.E;
    public KeyCode prevWeapon = KeyCode.Q;
    public KeyCode tower1 = KeyCode.Alpha1;
    public KeyCode tower2 = KeyCode.Alpha2;
    public KeyCode tower3 = KeyCode.Alpha3;

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
    public GameObject towerDisplayPrefab;
    private GameObject tempDisplayHolder;

    public int currency;

    [Header("Weapon List")]
    public GameObject currentWeapon;
    public List<GameObject> weaponList;
    private int currentWeaponIndex = 0;

    [Header("Tower List")]
    public GameObject currentTower;
    public List<GameObject> towerList;

    //The Modes the Player will be in, Combat = with weapons, Build = ability to edit towers
    public enum playerMode
    {
        Combat,
        Build,
    }

    //Method to be checked on first frame of the game
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        gameObject.GetComponent<ConstantForce>().force = defaultGravityDir * rb.mass * gravityConstant;

        readyToJump = true;
        currency = 80;
    }

    //Method to be checked on every frame of the game
    public void Update() 
     {
        checkCurrentMode();
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

    //Methods to be executed when user inputs movement keys
    //Executed on a fixed interval
    public void FixedUpdate() 
    {
        movePlayer();
        rotateToSurface();
    }

    //Checking which mode the player is currently in
    private void checkCurrentMode()
    {
        if (enteringBuildMode())
        {
            currentMode = playerMode.Build;

            Debug.Log("Build");
        }
        else if (enteringCombatMode())
        {
            currentMode = playerMode.Combat;
            Debug.Log("Combat");
            if (tempDisplayHolder != null)
            {
                Destroy(this.tempDisplayHolder);
            }
        }
    }

    private bool enteringBuildMode()
    {
        //Sets tower immediatley to whichever key is pressed 
        //then returns true to place player into build mode
        if (Input.GetKeyDown(tower1))
        { 
                return true;
        }
        if (Input.GetKeyDown(tower2))
        {
                return true;
        }
        if (Input.GetKeyDown(tower2))
        {
                return true;
        }
        return false;
    }

    private bool enteringCombatMode()
    {
        if (Input.GetKeyDown(prevWeapon))
        {
            return true;
        }
        if (Input.GetKeyDown(nextWeapon))
        {
            return true;
        }
        return false;
    }

    //Getting WASD and jump inputs
    private void getUserKeyInput()
    { 
        //Player hits WASD
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //Player wants to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }

        //Getting weapon selected
        if (Input.GetKeyDown(nextWeapon))
        {
            SwapWeapon(nextWeapon);
        }
        else if (Input.GetKeyDown(prevWeapon))
        {
            SwapWeapon(prevWeapon);
        }

        //Change current selected tower
        if (Input.GetKeyDown(tower1))
        {
            currentTower = towerList[0];
        }
        if (Input.GetKeyDown(tower2))
        {
            currentTower = towerList[1];
        }
        if (Input.GetKeyDown(tower2))
        {
            currentTower = towerList[2];
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

    private void rotateToSurface()
    {
        //interpolate my rotation to my rotation with the x and z axis replaced by the surface's

        //or rotateToward quaternion formed by Euler of my y and the surface's x and z
        //Vector3 diffInRotation = Vector3.Angle(-transform.up, GetComponent<ConstantForce>().force);
        
        Quaternion goalRotation = Quaternion.FromToRotation(-transform.up, GetComponent<ConstantForce>().force);
        Vector3 newOrientation = Quaternion.Lerp(rb.rotation, goalRotation, Time.deltaTime * 10f).eulerAngles;
        rb.rotation = Quaternion.Euler(newOrientation.x, rb.rotation.y, newOrientation.z);
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
        Weapon currentWeaponScript = currentWeapon.GetComponent<Weapon>();

        //Empty
        
        if (currentWeaponScript.Automatic && Input.GetMouseButton(0))
        {
            currentWeaponScript.TryToFire();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            currentWeaponScript.TryToFire();
        }
    
    }

    private void SwapWeapon(KeyCode input)
    {

        if (input == nextWeapon)
        {
            currentWeaponIndex++;
            if (currentWeaponIndex >= (weaponList.Count))
                currentWeaponIndex = 0;
        }
        if (input == prevWeapon)
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
                currentWeaponIndex = weaponList.Count - 1;
        }


        Transform cwt = currentWeapon.transform;

        Destroy(currentWeapon);

        currentWeapon = Instantiate(weaponList[currentWeaponIndex], cwt.position, cwt.rotation, transform.Find("Body"));
        

    }


    private void placeTowers()
    {
        //destroying the previous frame's green highlight for potential placement of tower
        if (tempDisplayHolder != null)
        {
            Destroy(this.tempDisplayHolder);
        }
        //here is where we should display an outline of the currently selected tower, either a green transparent silhouette if placeable, or red.
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if ((Physics.Raycast(ray, out RaycastHit hit, 100f, Grid)))
        {
            if ((hit.transform.tag.Equals("GridTile")))
            {
                GridTile currTileScript = hit.transform.GetComponent<GridTile>();

                if (currTileScript.placeable && !currTileScript.enemyOnTile)
                {
                    //displaying the potential placement spot for the tower
                    tempDisplayHolder = Instantiate(towerDisplayPrefab, new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z), transform.rotation);

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (currency >= currentTower.GetComponent<TowerBehavior>().cost)
                        {
                            Debug.Log(hit.transform.position.x);
                            Vector3 towerPlacement = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                            GameObject currTower = Instantiate(currentTower, towerPlacement, transform.rotation);
                            TowerBehavior tower = currTower.GetComponent<TowerBehavior>();

                            currTileScript.placeable = false;
                            currTileScript.walkable = false;
                            currTileScript.towerOnTile = true;

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
                else if (!currTileScript.towerOnTile)
                {
                    //displaying the potential placement spot for the tower, red when disallowed
                    tempDisplayHolder = Instantiate(towerDisplayPrefab, new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z), transform.rotation);
                    // Call SetColor using the shader property name "_Color" and setting the color to red
                    tempDisplayHolder.transform.Find("Body").GetComponent<Renderer>().material.SetColor("_Color", new Color(.9f, .1f, .1f, .2f));
                }
            }   
        }
        
    }

    private void sellTower()
    {
        GridTile tileOn = null;
        //invoke this event with the tile the tower was on.
        OnTowerSold?.Invoke(tileOn);
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
