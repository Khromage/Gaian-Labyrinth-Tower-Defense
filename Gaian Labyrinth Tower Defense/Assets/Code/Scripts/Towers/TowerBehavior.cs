using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour, Interactable
{
    public delegate void InteractionPanel(TowerBehavior towerScript);
    public static event InteractionPanel OnOpenInteractionPanel;
    public static event InteractionPanel OnCloseInteractionPanel;

    public delegate void UpgradeOrSell(int currencyChange);
    public static event UpgradeOrSell OnUpgradeOrSell;


    public TowerInfo towerInfo;
    public string towerName;

    public GameObject target;

    [Header("Tower Stats")]
    
    public string targetingMode;
    public float range;
    public float fireRate;
    public float fireCountdown;
    public float damage;

    public float targetCooldown;

    [Header("Unity Fields")]

    public string enemyTag = "Enemy";
    public GameObject InteractionIndicator;

    public Transform partToRotate;
    public float turnSpeed = 5f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    public List<GameObject> enemies = new List<GameObject>();
    public SphereCollider detectionZone;
    public AudioSource placedSFX;

    

    public int cost;
    public int lv2_cost;
    public int lv3_1_cost;
    public int lv3_2_cost;
    public int lv3_3_cost;
    public int totalSpent;
    public float sellRatio;

    public int currentLevel;
    public int currentBranch;

    public GridTile gridLocation;

    // Call the targeting function twice a second to scan for enemies
    public virtual void Start()
    {
        towerName = towerInfo.Name;
        damage = towerInfo.Damage;
        range = towerInfo.Range;
        fireRate = towerInfo.FireRate;
        cost = towerInfo.Cost;
        lv2_cost = towerInfo.Level2.Cost;
        lv3_1_cost = towerInfo.Branches[0].Cost;
        lv3_2_cost = towerInfo.Branches[1].Cost;
        lv3_3_cost = towerInfo.Branches[2].Cost;

        fireCountdown = 0f;
        targetCooldown = 0f;
        placedSFX = GetComponent<AudioSource>();
        placedSFX.Play();
        currentLevel = 1;
        currentBranch = 0;

        totalSpent += cost;
        sellRatio = .7f;
    }

    private void OnEnable()
    {
        targetingMode = "Close";
        detectionZone = GetComponent<SphereCollider>();
        detectionZone.radius = range;

        HideInteractButton();
    }
    private void OnDisable()
    {

    }
    
    public void AddEnemy(EnemyBehavior enemy)
    {
        if(enemy == null)
            return;
        if(enemies.Contains(enemy.gameObject))
            return;

        enemies.Add(enemy.gameObject);
        enemy.OnEnemyDeath += RemoveEnemy;
        enemy.OnEnemyReachedGoal += RemoveEnemy;
    }
    public void RemoveEnemy(EnemyBehavior enemy)
    {
        if(enemy == null)
            return;
        if(!enemies.Contains(enemy.gameObject))
            return;

        //Debug.Log($"removing enemy {enemy.gameObject} from tower's list");
        enemies.Remove(enemy.gameObject);   
        enemy.OnEnemyDeath -= RemoveEnemy;
        enemy.OnEnemyReachedGoal -= RemoveEnemy;
    }

    void OnTriggerEnter(Collider other)
    {
        AddEnemy(other.GetComponent<EnemyBehavior>());
    }
    void OnTriggerExit(Collider other)
    {
        RemoveEnemy(other.GetComponent<EnemyBehavior>());
    }

    void UpdateTarget()
    {
        //Debug.Log("Enemies in list: " + string.Join(", ", enemies));
        // Iterate through the list and find the enemy with the shortest distance from the tower ("Close" targeting)
        if(enemies.Count > 0)
        {
            string printstuff = "";
            foreach(GameObject enemy in enemies)
            {
                printstuff += enemy.name;
                    
            }
            Debug.Log(printstuff);
            switch (targetingMode)
            {
                case "Close":
                    float shortestDistance = Mathf.Infinity;
                    GameObject nearestEnemy = null;
                    foreach (GameObject enemy in enemies)
                    {
                        float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distanceToEnemy < shortestDistance)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestEnemy = enemy;
                        }
                    }
                    // Verify the closest enemy is within the tower range and assign as target if true
                    if(nearestEnemy != null && shortestDistance <= range)
                    {
                        target = nearestEnemy;
                    } else 
                    {
                        target = null;
                    }
                    break;
                case "Strong":
                    float highestHealth = 0;
                    GameObject strongestEnemy = null;
                    foreach (GameObject enemy in enemies)
                    {
                        float currentEnemyHealth = enemy.GetComponent<EnemyBehavior>().currentHealth;
                        if (currentEnemyHealth > highestHealth)
                        {
                            highestHealth = currentEnemyHealth;
                            strongestEnemy = enemy;
                        }
                    }
                    if(strongestEnemy != null)
                    {
                        target = strongestEnemy;
                    }
                    break;
                case "Weak":
                    float lowestHealth = Mathf.Infinity;
                    GameObject weakestEnemy = null;
                    foreach (GameObject enemy in enemies)
                    {
                        float currentEnemyHealth = enemy.GetComponent<EnemyBehavior>().currentHealth;
                        if (currentEnemyHealth < lowestHealth)
                        {
                            lowestHealth = currentEnemyHealth;
                            weakestEnemy = enemy;
                        }
                    }

                    if(weakestEnemy != null)
                    {
                        target = weakestEnemy;
                    }
                    break;
                case "First":
                    int shortDist = int.MaxValue;
                    GameObject firstEnemy = null;
                    foreach (GameObject enemy in enemies) {
                        int currDistance = enemy.GetComponent<EnemyBehavior>().currTile.goalDist;
                        if (currDistance < shortDist) {
                            shortDist = currDistance;
                            firstEnemy = enemy;
                        }
                    }
                    if(firstEnemy != null) {
                        target = firstEnemy;
                    }
                    break;
                case "Last":
                    int farDist = int.MinValue;
                    GameObject lastEnemy = null;
                    foreach (GameObject enemy in enemies) {
                        int distance = enemy.GetComponent<EnemyBehavior>().currTile.goalDist;
                        if (distance > farDist) {
                            shortDist = distance;
                            lastEnemy = enemy;
                        }
                    }
                    if(lastEnemy != null) {
                        target = lastEnemy;
                    }
                break;
            }
        } else {
            //Debug.Log("Enemy list is empty");
        }
    }

    public virtual void Update()
    {
        if (targetCooldown <= 0)
        {
            UpdateTarget();
            targetCooldown = .1f;
        }
        targetCooldown -= Time.deltaTime;


        if (target == null)
            return;

        LookTowardTarget();

        if(fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;

    }

    public virtual string GetDescription()
    {
        return towerInfo.Description.Replace("{Name}", towerName)
                                    .Replace("{Damage}", damage.ToString())
                                    .Replace("{Range}", range.ToString())
                                    .Replace("{FireRate}", fireRate.ToString())
                                    .Replace("{Cost}", cost.ToString());
    }
    
    //rotate head of tower to follow current target. Can be overridden for specific rotation behaviors
    protected virtual void LookTowardTarget()
    {
        // Generate vector pointing from tower towards target enemy and use it to rotate the tower head 
        Vector3 direction = target.transform.position - transform.position;
        Quaternion targetingRotation = Quaternion.LookRotation(direction);

        // Using Lerp to smooth transition between target swaps instead of snapping to new targets
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, targetingRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.localRotation = Quaternion.Euler(partToRotate.rotation.x, rotation.y, partToRotate.rotation.z); //changed this from (0, y, 0), then changed rotation to localRotation, which might mean can change back to (0,y,0)
    }
    protected virtual void Shoot()
    {
        switch (currentBranch)
        {
            case 0:
                if (currentLevel == 1)
                    lv1_Attack();
                else
                    lv2_Attack();
                break;
            case 1:
                lv3_1_Attack();
                break;
            case 2:
                lv3_2_Attack();
                break;
            case 3:
                lv3_3_Attack();
                break;
        }
        

    }

    //Specific attack behaviors based on level/branch. To be overridden by specific tower behaviors. 
    protected virtual void lv1_Attack()
    {
        ProjectileBehavior projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<ProjectileBehavior>();
        projectile.damage = damage;
        Debug.Log($"projectile damage: {damage}");
        if (projectile != null)
            projectile.SetTarget(target.transform);
        projectile.targeting = targetingMode;
    }
    protected virtual void lv2_Attack()
    {
        lv1_Attack();
        //change model
        //change projectile, if necessary
    }
    protected virtual void lv3_1_Attack()
    {
        lv1_Attack();
        //change model
        //change projectile, if necessary
    }
    protected virtual void lv3_2_Attack()
    {
        lv1_Attack();
        //change model
        //change projectile, if necessary
    }
    protected virtual void lv3_3_Attack()
    {
        lv1_Attack();
        //change model
        //change projectile, if necessary
    }


    public void ShowInteractButton()
    {
        if (InteractionIndicator != null)
        {
            InteractionIndicator.SetActive(true); // Show the indicator
            //highlight tile
            if (gridLocation != null)
                gridLocation.highlight(true);
        }
    }
    public void HideInteractButton()
    {
        if (InteractionIndicator != null)
        {
            InteractionIndicator.SetActive(false); // Hide the indicator
            if (gridLocation != null)
                gridLocation.highlight(false);
            //unhighlight tile
        }
    }

    // Interaction opens the Tower UI
    public void Interact()
    {
        OnOpenInteractionPanel?.Invoke(this);
        Debug.Log("Tower UI Opening");
    }

    public void upgradeTower(int option)
    {
        switch (option)
        {
            case 0:
                lv2_upgrade();
                currentLevel = 2;
                totalSpent += lv2_cost;
                OnUpgradeOrSell?.Invoke(-lv2_cost);
                break;

            case 1:
                lv3_1_upgrade();
                currentLevel = 3;
                currentBranch = 1;
                totalSpent += lv3_1_cost;
                OnUpgradeOrSell?.Invoke(-lv3_1_cost);
                break;

            case 2:
                lv3_2_upgrade();
                currentLevel = 3;
                currentBranch = 2;
                totalSpent += lv3_2_cost;
                OnUpgradeOrSell?.Invoke(-lv3_2_cost);
                break;

            case 3:
                lv3_3_upgrade();
                currentLevel = 3;
                currentBranch = 3;
                totalSpent += lv3_3_cost;
                OnUpgradeOrSell?.Invoke(-lv3_3_cost);
                break;

            default:
                Debug.Log("Tower upgrade stage not found");
                break;
        }
        OnCloseInteractionPanel?.Invoke(this);
    }
    protected virtual void lv2_upgrade()
    {
        //generic tower upgrade stats
        Debug.Log("Tower upgraded to stage 2");
        BulletBehavior bulletToUpgrade = projectilePrefab.GetComponent<BulletBehavior>();

        //Placeholder Visual to change model, should actually change model here
        GameObject upgradeSphere = transform.GetChild(0).gameObject;
        upgradeSphere.SetActive(true);


        damage = towerInfo.Level2.Damage;
        range = towerInfo.Level2.Range;
        fireRate = towerInfo.Level2.FireRate;
    }
    protected virtual void lv3_1_upgrade()
    {
        transform.Find("UpgradeSphere").GetComponent<Renderer>().material.SetColor("_Color", new Color(100, 0f, .1f, .1f));
        Debug.Log(gameObject + "lvl 3-1 upgrade");
        damage = towerInfo.Branches[0].Damage;
        range = towerInfo.Branches[0].Range;
        fireRate = towerInfo.Branches[0].FireRate;
    }
    protected virtual void lv3_2_upgrade()
    {
        transform.Find("UpgradeSphere").GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 100, 0, .1f));
        Debug.Log(gameObject + "lvl 3-2 upgrade");
        damage = towerInfo.Branches[1].Damage;
        range = towerInfo.Branches[1].Range;
        fireRate = towerInfo.Branches[1].FireRate;
    }
    protected virtual void lv3_3_upgrade()
    {
        transform.Find("UpgradeSphere").GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0, 100, .1f));
        Debug.Log(gameObject + "lvl 3-3 upgrade");
        damage = towerInfo.Branches[2].Damage;
        range = towerInfo.Branches[2].Range;
        fireRate = towerInfo.Branches[2].FireRate;
    }


    public void sellTower()
    {
        gridLocation.placeable = true;
        gridLocation.walkable = true;
        gridLocation.towerOnTile = false;
        OnUpgradeOrSell?.Invoke((int)(totalSpent * sellRatio));
        Destroy(gameObject);
    }


    // Tower range visualization via gizmos 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
}
