using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface implemented by any object, tower, npc, etc that the player can interact with
public interface Interactable
{
    public void Interact();
    public void ShowInteractButton();
    public void HideInteractButton();
}
public class Player : UnitBehavior
{
    public delegate void TowerPlaced(GridTile tileOn);
    public static event TowerPlaced OnTowerPlaced;

    public delegate void TowerSold(GridTile tileOn);
    public static event TowerSold OnTowerSold;

    public delegate void AdjustMana(float diff, bool animate);
    public static event AdjustMana OnAdjustMana;

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
    public KeyCode interactKey = KeyCode.F;
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
    // rb rigidbody is declared in UnitBehavior

    private Vector3 currGravDir;

    [Header("Mana")]
    public float maxMana;
    public float mana;
    public float manaRegenRate;

    [Header("Player Cam")]
    public GameObject playerCam;
    public GameObject currentCam;

    [Header("Build mode")]
    public GameObject towerPrefab;
    public GameObject towerDisplayPrefab;
    private GameObject tempDisplayHolder;
    public int currency;

    [Header("Player Interaction")]
    public float InteractRange;
    public Interactable InteractionTarget;
    public GameObject Body;
    public GameObject InteractionPoint;

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
    public void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        currency = 200;
        InteractRange = 5f;

        currGravDir = Vector3.Normalize(GetComponent<ConstantForce>().force);
        gameObject.GetComponent<ConstantForce>().force = defaultGravityDir * rb.mass * gravityConstant;

        currentCam = playerCam.GetComponent<ThirdPersonCam>().currentCam;

        maxMana = 100f;
        mana = maxMana;
        manaRegenRate = 30f;

    }

    //Method to be checked on every frame of the game
    public void Update() 
     {

        setVelocityComponents();

        checkCurrentMode();
        checkInteractable();
        getUserKeyInput();
        playerSpeedControl();

        setGravityDir();
        currGravDir = Vector3.Normalize(GetComponent<ConstantForce>().force);

        //Checking if player is on the ground by sending a Raycast down to see if layer whatIsGround is hit
        //Vector3.down was the 2nd parameter here, originally
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
        regenMana();
        movePlayer();
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
        if (Input.GetKeyDown(tower3))
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

        //Player wants to interact
        if(Input.GetKeyDown(interactKey))
        {
            Interact();
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
        if (Input.GetKeyDown(tower3))
        {
            currentTower = towerList[2];
        } 

    }

    private void checkInteractable()
    {
        
        Ray interactRay = new Ray(InteractionPoint.transform.position, InteractionPoint.transform.forward * InteractRange);
        Debug.DrawLine(InteractionPoint.transform.position, InteractionPoint.transform.position + InteractionPoint.transform.forward * InteractRange, Color.red, 2f);
        
        if(Physics.Raycast(interactRay, out RaycastHit hit, InteractRange))
        {
            Interactable interactable = hit.collider.gameObject.GetComponentInParent<Interactable>();
            if(interactable != null)
            {
                if(InteractionTarget != null && InteractionTarget != interactable)
                {
                    InteractionTarget.HideInteractButton(); // Hide previous interactable's button
                }
                
                InteractionTarget = interactable; // Update current interaction target
                InteractionTarget.ShowInteractButton();
                Debug.Log("Showing Interact Button");
            }
            else if(InteractionTarget != null)
            {
                InteractionTarget.HideInteractButton();
                InteractionTarget = null; // Reset last interactable
            }
        }
        else if(InteractionTarget != null)
        {
                InteractionTarget.HideInteractButton();
                InteractionTarget = null; // Reset last interactable
        }
    }

    private void Interact()
    {
        InteractionTarget.Interact();
    }


    private void regenMana()
    {
        float regenAmount = manaRegenRate * Time.fixedDeltaTime;
        changeMana(regenAmount, false); //not animated regen of mana
    }
    public void spentMana(float amount)
    {
        //amount should be negative. (the call in Weapon has parameter (-manaCost))
        changeMana(amount, true); //animated loss of mana
    }
    public void changeMana(float changeAmount, bool animated)
    {

        if (mana + changeAmount > maxMana) //if we'd exceed max mana
        {
            changeAmount = maxMana - (mana - changeAmount);
            mana = maxMana;
        }
        else if (mana + changeAmount < 0) //if we'd dip below 0 mana
        {
            changeAmount = mana;
            Debug.LogWarning("mana changed to less than 0. something wrong... setting to 0");
            mana = 0;
        }
        else //normal change
        {
            mana += changeAmount;
        }

        if (Mathf.Abs(changeAmount) > 0)
            OnAdjustMana?.Invoke(changeAmount / maxMana, animated);
    }

    //Method to move the player on ground and in air 
    private void movePlayer() 
    {
        //calculate movement direction
        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        //non-cinemachine
        //moveDirection = Vector3.Cross(newCameraHolder.transform.right, -currGravDir) * verticalInput 
        //    + Vector3.Cross(-currGravDir, newCameraHolder.transform.forward) * horizontalInput;

        //cinemachine 3d movement
        moveDirection = Vector3.Cross(orientation.right, -currGravDir) * verticalInput 
            + Vector3.Cross(-currGravDir, orientation.forward) * horizontalInput;

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
        //Limit player's movement velocity when reaching max speed
        if (lateralVelocityComponent.magnitude > moveSpeed)
        {
            Vector3 limitedVel = lateralVelocityComponent.normalized * moveSpeed;
            rb.velocity = limitedVel + verticalVelocityComponent;
        }
    }
    private void jump() 
    {
        //reset Y velocity to prepare for new jump
        rb.velocity = lateralVelocityComponent;

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
            currentWeaponScript.TryToFire(mana);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            currentWeaponScript.TryToFire(mana);
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
        if ((Physics.Raycast(ray, out RaycastHit hit, 30f, Grid)))
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
                            currTileScript.goalDist = int.MaxValue;

                            //currTileScript.goalDistText.text = $"{currTileScript.goalDist}";
                            //Debug.Log($"CurrTile dist {currTileScript.goalDist}");

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
    public void rotateToSurface()
    {
        currGravDir = Vector3.Normalize(GetComponent<ConstantForce>().force);
    }
    private void GainCurrency(GameObject enemyWhoDied)
    {
        currency += enemyWhoDied.GetComponent<EnemyBehavior>().worth;
    }
    private void OnEnable()
    {
        EnemyBehavior.OnEnemyDeath += GainCurrency;
        Weapon.OnFire += spentMana;
    }
    private void OnDisable()
    {
        EnemyBehavior.OnEnemyDeath -= GainCurrency;
        Weapon.OnFire -= spentMana;
    }
}
