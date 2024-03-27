using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

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

    public delegate void AdjustHealth(float diff, bool animate);
    public static event AdjustHealth OnAdjustHealth;

    public delegate void AdjustMana(float newAmount, float maxMana, bool animate);
    public static event AdjustMana OnAdjustMana;

    public delegate void TowerSelect(int index, GameObject towerObj);
    public static event TowerSelect OnTowerSelect;

    public delegate void EnterCombatMode(int weaponIndex);
    public static event EnterCombatMode OnEnterCombatMode;

    public delegate void SwapWeaponEvent(int newIndex);
    public static event SwapWeaponEvent OnSwapWeapon;

    public delegate void TowerSelection();
    public static event TowerSelection OnTowerSelectionOpened;
    public static event TowerSelection OnTowerSelectionClosed;

    //Variables to control and determine player's jumping abiltiy
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;


    [Header("KeyBinds")]
    // SO with default bindings
    public DefaultKeybinds defaultKeybinds;
    //Movement
    public KeyCode jumpKey;
    //Combat
    public KeyCode interactKey;
    public KeyCode nextWeaponKey;
    public KeyCode prevWeaponKey;
    //Build Mode
    public KeyCode modeChangeKey;
    public KeyCode towerSelectionKey;
    public KeyCode[] towerKeys;
    public KeyCode sellKey;
    public KeyCode[] updatePathKeys;


    [Header("Layer Variables")]
    public LayerMask whatIsGround;
    public LayerMask Grid;

    [Header("Ground Check")]
    public float playerHeight;
    bool grounded;
    public Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    // rb rigidbody is declared in UnitBehavior

    //private Vector3 currGravDir;
    private Coroutine rotateToSurfaceCoroutine;


    [Header("Health")]
    public float maxHealth;
    public float health;
    
    [Header("Mana")]
    public float maxMana;
    public float mana;
    public float manaRegenRate;

    [Header("Player Cam")]
    public GameObject playerCam;
    //public GameObject currentCam;

    [Header("Build mode")]
    public GameObject towerPrefab;
    public GameObject towerDisplayPrefab;
    private GameObject tempDisplayHolder;
    private GridTile highlightedTile;
    public static int currency { get; private set; }

    bool colerable = false;
    private GameObject towerHitByRaycast;
    [Header("Player Interaction")]
    public float InteractRange;
    public Interactable InteractionTarget;
    public GameObject Body;
    public GameObject InteractionPoint;

    [Header("Weapon List")]
    public GameObject weaponHolder;
    public GameObject currentWeapon;
    public WeaponList weaponList;
    public GameObject[] weaponSet;
    private int currentWeaponIndex = 0;

    [Header("Tower List")]
    [SerializeField]
    private TowerList towerList;
    public GameObject currentTower;
    public GameObject[] towerSet;
    public GameObject ctDisplay;

    private bool isTowerKeyPressed = false;
    private bool isTowerWheelOpen = false;
    private float TowerWheelOpenTime;

    private bool goingToBuildMode;
    public Animator armAnimator;

    //The Modes the Player will be in, Combat = with weapons, Build = ability to edit towers
    public playerMode currentMode;
    private playerMode? lastMode;
    public enum playerMode
    {
        Combat,
        Build,
        Sell,
        Menu
    }

    //Method to be checked on first frame of the game
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        UpdateCurrency(200);
        InteractRange = 5f;

        currGravDir = Vector3.Normalize(GetComponent<ConstantForce>().force);
        gameObject.GetComponent<ConstantForce>().force = defaultGravityDir * rb.mass * gravityConstant;


        maxHealth = 100f;
        health = maxHealth;
        maxMana = 100f;
        mana = maxMana;
        manaRegenRate = 5f;
        goingToBuildMode = false;

        towerSet = new GameObject[6];
        FillLoadout();
        InitializeKeybinds();

        currentMode = playerMode.Combat;
        lastMode = playerMode.Build;
    }

    //Method to be checked on every frame of the game
    public void Update() 
     {
        setVelocityComponents();
        regenMana();
        
        getUserKeyMenu();
        updateAnimationState();
        
        // Check if NOT in menu mode

        if(currentMode != playerMode.Menu)
        {
            getUserKey();
            checkInteractable();
            playerSpeedControl();
        }

        //setGravityDir();  // this call was to UnitBehavior function using raycast to determine gravity dir. unused

        //Checking if player is on the ground by sending a Raycast down to see if layer whatIsGround is hit
        //Vector3.down was the 2nd parameter here, originally
        grounded = Physics.Raycast(transform.position + (transform.up * .05f), -transform.up, playerHeight * 0.5f + 0.3f, whatIsGround);
        
        //handling player drag if on the ground
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    public void FixedUpdate()
    {
        movePlayer();
    }
    
    private void OnEnable()
    {
        //EnemyBehavior.OnEnemyDeath += GainCurrency;
        Weapon.OnFire += spentMana;
        LevelModule.OnMenuOpened += EnterMenuMode;
        LevelModule.OnMenuClosed += ExitMenuMode;
        TowerBehavior.OnUpgradeOrSell += UpdateCurrency;
        TowerSelectionWheel.OnTowerSelected += TowerSelected;
    }

    private void OnDisable()
    {
        //EnemyBehavior.OnEnemyDeath -= GainCurrency;
        Weapon.OnFire -= spentMana;
        LevelModule.OnMenuOpened -= EnterMenuMode;
        LevelModule.OnMenuClosed -= ExitMenuMode;
        TowerBehavior.OnUpgradeOrSell -= UpdateCurrency;
        TowerSelectionWheel.OnTowerSelected -= TowerSelected;
    }

    private void EnterMenuMode()
    {
        lastMode = currentMode;
        currentMode = playerMode.Menu;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ExitMenuMode()
    {
        if(goingToBuildMode)
        {
            currentMode = playerMode.Build;
        } else {
            currentMode = playerMode.Combat;
        }

        // set lastMode accordingly to allow Q tap mode switching to work
        if(currentMode == playerMode.Combat)
            lastMode = playerMode.Build;
        if(currentMode == playerMode.Build)
            lastMode = playerMode.Combat;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public playerMode checkCurrentMode()
    {
        return currentMode;
    }

    // Loads default keybinds from DefaultKeybinds SO
    private void InitializeKeybinds()
    {
        // General
        jumpKey = defaultKeybinds.jumpKey;
        interactKey = defaultKeybinds.interactKey;
        // Combat
        nextWeaponKey = defaultKeybinds.nextWeaponKey;
        prevWeaponKey = defaultKeybinds.prevWeaponKey;
        //Build Mode
        modeChangeKey = defaultKeybinds.modeChangeKey;
        towerSelectionKey = defaultKeybinds.towerSelectionKey;
        towerKeys = defaultKeybinds.towerKeys;
        sellKey = defaultKeybinds.sellKey;
        updatePathKeys = defaultKeybinds.updatePathKeys;
    }

    private void SwapMode()
    {
        currentMode = (playerMode)lastMode;

        // set lastMode accordingly to allow Q tap mode switching to work
        if(currentMode == playerMode.Combat)
            lastMode = playerMode.Build;
        if(currentMode == playerMode.Build)
            lastMode = playerMode.Combat;
    }

    //Getting WASD and jump inputs
    private void getUserKey()
    { 
        // Mouse click actions player can do depending on mode they are in
        if (currentMode == playerMode.Combat) 
        {
            attack();
        } 
        else if (currentMode != playerMode.Build) 
        {
            //maybe also display outlines of the grid tiles so the player has some idea of where towers can be placed.
            destroyTempHolder();
        } 
        else if (currentMode == playerMode.Sell) 
        {
            //sellTower();
            changeTowerColor();
        } 
        else 
        {
            placeTowers();
        }
        
        // Player hits WASD
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Player wants to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            jump();
            Invoke(nameof(resetJump), jumpCooldown);
        }

        /***
            Setting player mode (Combat/Build/Menu)
        ***/




        // Change selected tower and set Build Mode
        for (int i = 0; i < towerKeys.Length; i++)
        {
            if (Input.GetKeyDown(towerKeys[i])) 
            {
                if (towerSet[i] != null)
                {
                    currentTower = towerSet[i];
                    OnTowerSelect?.Invoke(i, towerSet[i]);
                    Debug.Log("Tower Slot " + (i+1) + "Chosen");
                    currentMode = playerMode.Build;

                    currentWeapon.SetActive(false);
                    toggleTowerDisplay(currentTower, true);
                } else 
                {
                    Debug.Log("No tower in slot " + (i+1));
                }
            }
        }
        
        if ( (Input.GetKeyDown(sellKey)))
        {
            currentTower = null;
            currentMode = playerMode.Sell;
        }

        // Change chosen weapon and set Combat mode
        if (Input.GetKeyDown(nextWeaponKey))
        {
            currentWeapon.SetActive(true);
            toggleTowerDisplay(currentTower, false);

            SwapWeapon(nextWeaponKey);
            currentMode = playerMode.Combat;
            if (tempDisplayHolder != null)
                Destroy(this.tempDisplayHolder);
            if (highlightedTile != null)
                highlightedTile.highlight(false);
            OnEnterCombatMode?.Invoke(currentWeaponIndex);
        } 
        else if (Input.GetKeyDown(prevWeaponKey))
        {
            currentWeapon.SetActive(true);
            toggleTowerDisplay(currentTower, false);

            SwapWeapon(prevWeaponKey);
            currentMode = playerMode.Combat;
            if (tempDisplayHolder != null)
                Destroy(this.tempDisplayHolder);
            if (highlightedTile != null)
                highlightedTile.highlight(false);
            OnEnterCombatMode?.Invoke(currentWeaponIndex);  
        }

        // Player uses interaction key. If menu opens, set menu mode.
        if(Input.GetKeyDown(interactKey))
        {
            try
            {
                Interact();
            }
            catch (System.Exception)
            {
                throw;
            }
            Debug.Log("Interaction");
        }

    }

    private void getUserKeyMenu()
    {
        // Check if the Q key is pressed down
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isTowerKeyPressed = true;
            isTowerWheelOpen = false;
            TowerWheelOpenTime = Time.time;
        }

        // Check if the key is being held down AND if the Tower Wheel has not opened yet
        if (isTowerKeyPressed && !isTowerWheelOpen && (Time.time - TowerWheelOpenTime) > 0.1f)
        {
            OpenTowerSelectionWheel();
            isTowerWheelOpen = true;
        }

        // Check if the Q key is released
        if (Input.GetKeyUp(KeyCode.Q))
        {
            isTowerKeyPressed = false;

            if (!isTowerWheelOpen)
            {
                // Key was released before 0.05 seconds
                SwapMode();
            } else
            {
                CloseTowerSelectionWheel();
            }
            // Reset the flag when the key is released
            isTowerWheelOpen = false;
        }
    }

    private void TowerSelected(int towerSlotIndex)
    {
        if(towerSlotIndex != -1)
        {
            goingToBuildMode = true;
            
            Debug.Log("Player received towerSlotIndex == " + towerSlotIndex);
            Debug.Log("Current PlayerMode: " + currentMode.ToString());
            Debug.Log("Last PlayerMode: " + lastMode.ToString());


            changeTower(towerSlotIndex);
        }
    }



    private void OpenTowerSelectionWheel()
    {
        // send event to LevelModule to enable TowerSelectionWheel UI Module
        OnTowerSelectionOpened?.Invoke();
    }

    private void CloseTowerSelectionWheel()
    {
        // send event to LevelModule to dsable TowerSelectionWheel UI Module
        OnTowerSelectionClosed?.Invoke();
    }
    private void checkInteractable()
    {
        // Visualization for raycast debugging
        // Debug.DrawLine(InteractionPoint.transform.position, InteractionPoint.transform.position + InteractionPoint.transform.forward * InteractRange, Color.red, 1f);
        
        if(Physics.Raycast(InteractionPoint.transform.position, InteractionPoint.transform.forward, out RaycastHit hit, InteractRange))
        {

            Interactable curr_interactable = hit.collider.gameObject.GetComponentInParent<Interactable>();
            if(curr_interactable != null)
            {
                if(InteractionTarget != null && InteractionTarget != curr_interactable)
                {
                    InteractionTarget.HideInteractButton(); // Hide previous interactable's button
                }
                InteractionTarget = curr_interactable; // Update current interaction target
                InteractionTarget.ShowInteractButton();
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
        if(InteractionTarget != null)
            InteractionTarget.Interact();
    }

    public void changeHealth(float changeAmount, bool animated)
    {
        if (health + changeAmount > maxHealth) //if we'd exceed max hp
        {
            changeAmount = maxHealth - (health - changeAmount);
            health = maxHealth;
        }
        else if (health + changeAmount < 0) //if we'd dip below 0 hp
        {
            changeAmount = health;
            Debug.LogWarning("YOU HAVE DIED");
            health = 0;
        }
        else //normal change
        {
            health += changeAmount;
        }

        if (Mathf.Abs(changeAmount) > 0)
            OnAdjustHealth?.Invoke(changeAmount / maxHealth, animated);
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
            OnAdjustMana?.Invoke(mana, maxMana, animated);
    }

    private void updateAnimationState()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            armAnimator.SetBool("Clicking", true);
        }
        else
        {
            armAnimator.SetBool("Clicking", false);
        }


        armAnimator.SetBool("Arcane Equipped", false);
        armAnimator.SetBool("Lantern Equipped", false);
        armAnimator.SetBool("Heatray Equipped", false);
        armAnimator.SetBool("Arcshatter Equipped", false);


        if (currentMode == playerMode.Build)
        {
            armAnimator.SetBool("Build Mode", true);
        }
        else
        {
            armAnimator.SetBool("Build Mode", false);

            //update this to use weapon ID's from the weaponSet SO's WeaponDataSet
            switch (currentWeaponIndex)
            {
                case 0:
                    armAnimator.SetBool("Arcane Equipped", true);
                    break;
                case 1:
                    armAnimator.SetBool("Lantern Equipped", true);
                    break;
                case 2:
                    armAnimator.SetBool("Heatray Equipped", true);
                    break;
                case 3:
                    armAnimator.SetBool("Arcshatter Equipped", true);
                    break;
            }
        }
    }

    private void changeTower(int slotIndex)
    {
        if (towerSet[slotIndex] != null)
            {
                currentTower = towerSet[slotIndex];
                currentMode = playerMode.Build;

                Debug.Log("Player Mode set to Build");
                Debug.Log("Current PlayerMode: " + currentMode.ToString());
                Debug.Log("Last PlayerMode: " + lastMode.ToString());

                currentWeapon.SetActive(false);
                toggleTowerDisplay(currentTower, true);
            } else 
            {
                Debug.Log("ERROR WHEN ACCESSING towerSet[slotIndex]");
            }
    }
    
    //re-generates the tiny tower model on your hand. called when entering build mode. (or exiting, in which case just sets inactive)
    private void toggleTowerDisplay(GameObject currentTower, bool turnOn)
    {
        if (turnOn)
        {
            ctDisplay.SetActive(true);
            if (ctDisplay.transform.childCount > 1)
                Destroy(ctDisplay.transform.GetChild(1).gameObject);
            GameObject t = Instantiate(currentTower.GetComponent<TowerBehavior>().towerInfo.NonfuncModel, ctDisplay.transform.position, ctDisplay.transform.rotation, ctDisplay.transform);
            t.transform.SetSiblingIndex(1);
            t.transform.localScale = Vector3.one * .01f;
        }
        else
        {
            ctDisplay.SetActive(false);
        }
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

        rb.AddForce(-currGravDir * jumpForce, ForceMode.Impulse);
        Debug.Log("jumping");
    }
    private void resetJump()
    {
        readyToJump = true;
    }


    protected override void beginRotation(Transform gravSource)
    {
        if (rotateToSurfaceCoroutine != null)
            StopCoroutine(rotateToSurfaceCoroutine);
        rotateToSurfaceCoroutine = StartCoroutine(rotateCoroutine(gravSource));
    }

    private IEnumerator rotateCoroutine(Transform gravSource)
    {
        Vector3 axisToRotateAround = Vector3.Cross(-transform.up, currGravDir);


        Debug.Log("began rotation coroutine");
        float initialDiffInRotation = Vector3.Angle(-transform.up, currGravDir);
        Debug.Log("initial diff in rotation: " +  initialDiffInRotation);
        float elapsedTime = 0f;
        Quaternion initRot = transform.rotation;
        //Quaternion goalRot = Quaternion.Euler(currGravDir.x, 0f, currGravDir.z); //THIS LINE IS THE PROBLEM. need currGravDir as an actual rotation, not a normalized direction
        //Quaternion goalRot = Quaternion.LookRotation(Vector3.Cross(Vector3.up, -currGravDir), -currGravDir);
        
        //Vector3 initRotEuler = transform.rotation.eulerAngles;
        //Vector3 goalRotEuler = Quaternion.LookRotation(Vector3.Cross(Vector3.up, currGravDir), currGravDir).eulerAngles;


        while (elapsedTime < 1)
        {
            //transform.rotation = Quaternion.Euler(Vector3.Lerp(initRot.eulerAngles, goalRot.eulerAngles, elapsedTime * 180 / initialDiffInRotation));
            rb.rotation = Quaternion.Slerp(initRot, gravSource.rotation, elapsedTime * 90 / initialDiffInRotation); 
            //transform.eulerAngles = Vector3.Lerp(initRotEuler, goalRotEuler, elapsedTime);

            //transform.RotateAround(transform.position, axisToRotateAround, 1f); //initialDiffInRotation * Time.deltaTime


            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rb.rotation = gravSource.rotation;
        //if elapsed rotation > initialDiffInRotation, set rotation to currGravDir's
        Debug.Log("rot at end of corout: " + transform.rotation.eulerAngles);
    }

    /***
    ****
        Methods for player weapon usage
    ****
    ***/

    private void attack()
    {
        Weapon currentWeaponScript = currentWeapon.GetComponent<Weapon>();
        
        if (currentWeaponScript.Automatic && Input.GetMouseButton(0))
        {
            Ray aimRay = new Ray(playerCam.transform.position, playerCam.transform.forward);
            if (currentWeaponScript.TryToFire(mana, aimRay))
            {
                armAnimator.SetBool("Clicking", true);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Ray aimRay = new Ray(playerCam.transform.position, playerCam.transform.forward);
            if (currentWeaponScript.TryToFire(mana, aimRay))
            {
                armAnimator.SetBool("Clicking", true);
            }
        }
        else
        {
            armAnimator.SetBool("Clicking", false);
        }
    
    }
    private void SwapWeapon(KeyCode input)
    {

        if (input == nextWeaponKey)
        {
            currentWeaponIndex++;
            if (currentWeaponIndex >= (weaponSet.Length))
                currentWeaponIndex = 0;
        }
        if (input == prevWeaponKey)
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
                currentWeaponIndex = weaponSet.Length - 1;
        }

        //in the hierarchy, the 1st weapon was originally at: .411, .121, 0
        Transform cwt = currentWeapon.transform;

        Destroy(currentWeapon);

        currentWeapon = Instantiate(weaponSet[currentWeaponIndex], cwt.position, cwt.rotation, weaponHolder.transform);
        OnSwapWeapon?.Invoke(currentWeaponIndex);
    }

    /***
    ****
        Methods for Build and Upgrade Modes
    ****
    ***/
    
    private void placeTowers()
    {
        //destroying the previous frame's green highlight for potential placement of tower
        destroyTempHolder();
        if (highlightedTile != null)
            highlightedTile.highlight(false);

        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if ((Physics.Raycast(ray, out RaycastHit hit, 30f, Grid)))
        {
            if ((hit.transform.tag.Equals("GridTile")))
            {
                GridTile currTileScript = hit.transform.GetComponent<GridTile>();
                highlightedTile = currTileScript;
                highlightedTile.highlight(true);

                if (currTileScript.placeable && !currTileScript.enemyOnTile)
                {
                    //displaying the potential placement spot for the tower
                    tempDisplayHolder = Instantiate(towerDisplayPrefab, new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z), transform.rotation);

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (currency >= currentTower.GetComponent<TowerBehavior>().towerInfo.Cost)
                        {
                            //Vector3 towerPlacement = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                            GameObject currTower = Instantiate(currentTower, hit.transform.position, hit.transform.rotation);
                            TowerBehavior tower = currTower.GetComponent<TowerBehavior>();
                            tower.gridLocation = currTileScript;

                            //transform.rotation = Quaternion.Euler(30f, 0f, 0f);

                            currTileScript.placeable = false;
                            currTileScript.walkable = false;
                            currTileScript.towerOnTile = true;
                            currTileScript.goalDist = int.MaxValue;

                            //currTileScript.goalDistText.text = $"{currTileScript.goalDist}";
                            //Debug.Log($"CurrTile dist {currTileScript.goalDist}");

                            Debug.Log($"Tower's cost: {tower.towerInfo.Cost}");
                            Debug.Log($"Tower's lv2 cost: {tower.towerInfo.Level2.Cost}");
                            UpdateCurrency(-tower.towerInfo.Cost);

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
    
    /*
     * Taking care of this in TowerUIManager and TowerBehavior
    private void sellTower()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if ((Physics.Raycast(ray, out RaycastHit hit, 30f)) && (hit.transform.tag == "towerbuilding")) {
            Debug.Log("Sell");
            colerable = true;
            towerHitByRaycast = hit.transform.gameObject;
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                TowerBehavior towerBehavior = towerHitByRaycast.GetComponent<TowerBehavior>();

                GridTile towerTile = towerBehavior.gridLocation;
                UpdateCurrency((int)(towerBehavior.cost * .7f));
                towerTile.placeable = true;
                towerTile.walkable = true;
                towerTile.towerOnTile = false;
                
                //invoke this event with the tile the tower was on.
                //OnTowerSold?.Invoke(towerTile);
                
                Destroy(towerHitByRaycast);
                colerable = false;
            }
        } else {
            colerable = false;
        }
    }   
    */

    public void GainCurrency(EnemyBehavior enemyWhoDied)
    {
        UpdateCurrency(enemyWhoDied.worth);
    }

    private void UpdateCurrency(int val)
    {
        currency += val;
        LevelManager.Instance.Currency = currency;
    }


    //set towers + their order, and weapons + their order
    private void FillLoadout()
    {
        int[] towerLoadout = LoadoutManager.Instance.EquippedTowerIDs;
        int[] weaponLoadout = LoadoutManager.Instance.EquippedWeaponIDs;

        Debug.Log("FillLoadout loadoutManager's length: " + towerLoadout.Length);

        //fill in the tower set from save data
        for (int i = 0; i < towerSet.Length; i++)
        {
            if (towerLoadout[i] != -1) // -1 is the default/empty value
                towerSet[i] = towerList.TowerDataSet[towerLoadout[i]].Prefab;
        }

        //fill in the active weapon slots with their respective weapon icons...
        for (int i = 0; i < weaponSet.Length; i++)
        {
            //SSS fill in the weapons from save data
        }
    }


    private void destroyTempHolder() 
    {
        if (tempDisplayHolder != null) {
            Destroy(this.tempDisplayHolder);
        }
    }

//Should highlight the tower that the player is looking at (Only changes tower color rn)
    private void changeTowerColor()
    {
        if (towerHitByRaycast == null) {
            return;
        }
        if (colerable) {
            towerHitByRaycast.transform.Find("Head").GetComponent<Renderer>().material.SetColor("_Color", new Color(.1f, .9f, .1f, .2f));
        } else if (!colerable) {

            towerHitByRaycast.transform.Find("Head").GetComponent<Renderer>().material.SetColor("_Color", new Color(1,1,1,1));
        }
    }

}
