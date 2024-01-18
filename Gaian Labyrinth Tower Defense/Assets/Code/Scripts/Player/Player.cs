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

    public delegate void TowerSelect(int index, GameObject towerObj);
    public static event TowerSelect OnTowerSelect;

    public delegate void EnterCombatMode(int weaponIndex);
    public static event EnterCombatMode OnEnterCombatMode;

    public delegate void SwapWeaponEvent(int newIndex);
    public static event SwapWeaponEvent OnSwapWeapon;

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
    //Movement
    public KeyCode jumpKey = KeyCode.Space;
    //Combat
    public KeyCode interactKey = KeyCode.F;
    public KeyCode nextWeapon = KeyCode.E;
    public KeyCode prevWeapon = KeyCode.Q;
    //Build Mode
    public KeyCode buildMode = KeyCode.Tab;

    public KeyCode tower1 = KeyCode.Alpha1;
    public KeyCode tower2 = KeyCode.Alpha2;
    public KeyCode tower3 = KeyCode.Alpha3;
    public KeyCode tower4 = KeyCode.Alpha4;
    public KeyCode tower5 = KeyCode.Alpha5;
    public KeyCode tower6 = KeyCode.Alpha6;

    public KeyCode deleteTower = KeyCode.Alpha0;
    public KeyCode upgradeCurrentTower = KeyCode.V;
    public KeyCode upgradePath1 = KeyCode.J;
    public KeyCode upgradePath2 = KeyCode.K;
    public KeyCode upgradePath3 = KeyCode.L;



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

    bool colerable = false;
    private GameObject towerHitByRaycast;
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
    public GameObject[] towerSet = new GameObject[6];

    //The Modes the Player will be in, Combat = with weapons, Build = ability to edit towers
    public enum playerMode
    {
        Combat,
        Build,
        Upgrade,
        Sell,
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
        getUserKey();
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
        if (currentMode == playerMode.Combat) {
            attack();
        } else {
            if (currentMode != playerMode.Build) {
                //maybe also display outlines of the grid tiles so the player has some idea of where towers can be placed.
                destoryTempHolder();
                if (currentMode == playerMode.Sell) {
                    sellTower();
                    changeTowerColor();
                } else if (currentMode == playerMode.Upgrade) {
                    upgradeTower();
                    changeTowerColor();
                }
            } else {
                placeTowers();
            }
        }
    }

    public void FixedUpdate()
    {
        regenMana();
        movePlayer();
    }

    private void checkCurrentMode()
    {
        if (Input.GetKeyDown(prevWeapon) ||
           Input.GetKeyDown(nextWeapon)) {
            currentMode = playerMode.Combat;
            if (tempDisplayHolder != null)
            {
                Destroy(this.tempDisplayHolder);
            }
            OnEnterCombatMode?.Invoke(currentWeaponIndex);
        }

        if (Input.GetKeyDown(tower1) ||
           Input.GetKeyDown(tower2) ||
           Input.GetKeyDown(tower3)) {
            currentMode = playerMode.Build;
        }

        if (Input.GetKeyDown(deleteTower)) {
            currentMode = playerMode.Sell;
        }

        if (Input.GetKeyDown(upgradeCurrentTower) ||
            Input.GetKeyDown(buildMode)) {
            currentMode = playerMode.Upgrade;
        }
    }
    private bool enteringBuildMode()
    {
        //Sets tower immediatley to whichever key is pressed 
        //then returns true to place player into build mode
        if (Input.GetKeyDown(tower1) || Input.GetKeyDown(tower2) || Input.GetKeyDown(tower3) 
            || Input.GetKeyDown(tower4) || Input.GetKeyDown(tower5) || Input.GetKeyDown(tower6))
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

        if (Input.GetKeyDown(upgradeCurrentTower) ||
            Input.GetKeyDown(buildMode)) {
            currentMode = playerMode.Upgrade;
        }
        return false;
    }

    //Getting WASD and jump inputs
    private void getUserKey()
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
            if (towerSet[0] != null)
                currentTower = towerSet[0];
            OnTowerSelect?.Invoke(0, towerSet[0]);
        }
        if (Input.GetKeyDown(tower2))
        {
            if (towerSet[1] != null)
                currentTower = towerSet[1];
            OnTowerSelect?.Invoke(1, towerSet[1]);
        }
        if (Input.GetKeyDown(tower3))
        {
            if (towerSet[2] != null)
                currentTower = towerSet[2];
            OnTowerSelect?.Invoke(2, towerSet[2]);
        }
        if (Input.GetKeyDown(tower4))
        {
            if (towerSet[3] != null)
                currentTower = towerSet[3];
            OnTowerSelect?.Invoke(3, towerSet[3]);
        }
        if (Input.GetKeyDown(tower5))
        {
            if (towerSet[4] != null)
                currentTower = towerSet[4];
            OnTowerSelect?.Invoke(4, towerSet[4]);
        }
        if (Input.GetKeyDown(tower6))
        {
            if (towerSet[5] != null)
            currentTower = towerSet[5];
            OnTowerSelect?.Invoke(5, towerSet[5]);
        }
        if ((Input.GetKeyDown(deleteTower))
            || (Input.GetKeyDown(buildMode)))
        {
            currentTower = null;
        }

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
        readyToJump = true;
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
        destoryTempHolder();
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
                            Vector3 towerPlacement = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                            GameObject currTower = Instantiate(currentTower, towerPlacement, transform.rotation);
                            TowerBehavior tower = currTower.GetComponent<TowerBehavior>();
                            tower.gridLocation = currTileScript;

                            currTileScript.placeable = false;
                            currTileScript.walkable = false;
                            currTileScript.towerOnTile = true;
                            currTileScript.goalDist = int.MaxValue;

                            //currTileScript.goalDistText.text = $"{currTileScript.goalDist}";
                            //Debug.Log($"CurrTile dist {currTileScript.goalDist}");

                            currency -= tower.cost;

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
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if ((Physics.Raycast(ray, out RaycastHit hit, 30f)) && (hit.transform.tag == "towerbuilding")) {
            Debug.Log("Sell");
            colerable = true;
            towerHitByRaycast = hit.transform.gameObject;
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                TowerBehavior towerBehavior = towerHitByRaycast.GetComponent<TowerBehavior>();

                GridTile towerTile = towerBehavior.gridLocation;
                currency += towerBehavior.cost;
                towerTile.placeable = true;
                towerTile.walkable = true;
                towerTile.towerOnTile = false;
                /*
                //invoke this event with the tile the tower was on.
                OnTowerSold?.Invoke(towerTile);
                */
                Destroy(towerHitByRaycast);
                colerable = false;
            }
        } else {
            colerable = false;
        }
    }   

    private void upgradeTower()
    {
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if ((Physics.Raycast(ray, out RaycastHit hit, 30f)) && (hit.transform.tag == "towerbuilding")) {
            colerable = true;
            towerHitByRaycast = hit.transform.gameObject;

            TowerBehavior towerBehavior = towerHitByRaycast.GetComponent<TowerBehavior>();
            bool upgradeMultiPath = towerBehavior.multiPathUpgrade;
            int upgradeStage = towerBehavior.upgradeStage;
            int towerCost = towerBehavior.cost;

            if ((towerBehavior.isUpgradable) && (currency > towerCost)){
                Debug.Log("Upgradeable");
                if (upgradeMultiPath == false) {
                    if (Input.GetKeyDown(upgradeCurrentTower)) {
                        Debug.Log("Upgrade");
                        goToUpgrade(upgradeStage, towerHitByRaycast, towerBehavior, towerCost);
                    }
                }else if (upgradeMultiPath == true){
                    if (Input.GetKeyDown(upgradePath1)){
                        upgradeStage = 9;
                        goToUpgrade(upgradeStage, towerHitByRaycast, towerBehavior, towerCost);
                    }else if (Input.GetKeyDown(upgradePath2)){
                        upgradeStage = 19;
                        goToUpgrade(upgradeStage, towerHitByRaycast, towerBehavior, towerCost);
                    }else if (Input.GetKeyDown(upgradePath3)){
                        upgradeStage = 29;
                        goToUpgrade(upgradeStage, towerHitByRaycast, towerBehavior, towerCost);
                    }
                }
            }
        } else {
        colerable = false;
        }
    }

    private void goToUpgrade(int upgradeStage, GameObject towerHitByRaycast, TowerBehavior towerBehavior, int towerCost){
        colerable = false;
        upgradeStage++;
        towerBehavior.upgradeTower(upgradeStage, towerHitByRaycast);
        currency -= towerCost;
    }
    public void rotateToSurface()
    {
        currGravDir = Vector3.Normalize(GetComponent<ConstantForce>().force);
    }
    private void GainCurrency(GameObject enemyWhoDied)
    {
        currency += enemyWhoDied.GetComponent<EnemyBehavior>().worth;
    }


    //set towers + their order, and weapons + their order
    private void level_LoadData(string[] givenTowerSet, string[] weaponSet)
    {
        //fill in the tower set from save data
        for (int i = 0; i < towerSet.Length; i++)
        {
            towerSet[i] = Tower.GetPrefab(givenTowerSet[i]);
        }

        //fill in the active weapon slots with their respective weapon icons...
        for (int i = 0; i < weaponList.Count; i++)
        {
            //SSS fill in the weapons from save data
        }
    }

    private void OnEnable()
    {
        EnemyBehavior.OnEnemyDeath += GainCurrency;
        Weapon.OnFire += spentMana;
        LevelManager.OnLoadData += level_LoadData;
    }

    private void OnDisable()
    {
        EnemyBehavior.OnEnemyDeath -= GainCurrency;
        Weapon.OnFire -= spentMana;
        LevelManager.OnLoadData -= level_LoadData;
    }

    private void destoryTempHolder() 
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
