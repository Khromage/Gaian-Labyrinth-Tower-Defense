using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour, Interactable
{
    public delegate void OpenInteractionPanel(string towerName, int currentLevel);
    public static event OpenInteractionPanel OnOpenInteractionPanel;

    public string towerName = "arcane";

    public GameObject target;

    [Header("Tower Stats")]
    
    public string targetingMode;
    public float range;
    public float fireRate;
    public float fireCountdown;
    public float currentDamage;

    public float targetCooldown;

    [Header("Unity Fields")]

    public string enemyTag = "Enemy";
    public GameObject InteractionIndicator;

    public Transform partToRotate;
    public float turnSpeed = 5f;

    public ProjectileBehavior projectilePrefab;
    public Transform firePoint;

    public List<GameObject> enemies = new List<GameObject>();
    public SphereCollider detectionZone;

    public int cost = 10;
    public int lv2_cost = 20;
    public int lv3_1_cost = 30;
    public int lv3_2_cost = 30;
    public int lv3_3_cost = 30;

    public int currentLevel = 1;

    public GridTile gridLocation;

    // Call the targeting function twice a second to scan for enemies
    public virtual void Start()
    {
        range = 10f;
        fireRate = 1f;
        fireCountdown = 0f;
        currentDamage = 5f;

        targetCooldown = 0f;
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
    void Shoot()
    {
        ProjectileBehavior projectile = Instantiate (projectilePrefab, firePoint.position, firePoint.rotation) as ProjectileBehavior;
        projectile.damage = currentDamage;
        if (projectile != null)
            projectile.SetTarget(target.transform);
        projectile.targeting = targetingMode;

    }
    public void Interact()
    {
        OpenTowerUI();
    }

    // display tower UI in screen space
    void OpenTowerUI()
    {
        OnOpenInteractionPanel?.Invoke(towerName, currentLevel);
        Debug.Log("INTERACTION HAPPINGING");

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

    public void upgradeTower(int newLevel)
    {
        switch (newLevel)
        {
            case 2:
                lv2_upgrade();
                break;

            case 31:
                lv3_1_upgrade();
                break;

            case 32:
                lv3_2_upgrade();
                break;

            case 33:
                lv3_3_upgrade();
                break;

            default:
                Debug.Log("Tower upgrade stage not found");
                break;
        }
    }

    protected virtual void lv2_upgrade()
    {
        //generic tower upgrade stats
        Debug.Log("Tower upgraded to stage 2");
        BulletBehavior bulletToUpgrade = projectilePrefab.GetComponent<BulletBehavior>();

        //Placeholder Visual to change model, should actually change model here
        GameObject upgradeSphere = transform.GetChild(0).gameObject;
        upgradeSphere.SetActive(true);


        currentDamage = 2f;
        range = 10.2f;
        fireRate = 3f;
    }
    protected virtual void lv3_1_upgrade()
    {
        transform.Find("UpgradeSphere").GetComponent<Renderer>().material.SetColor("_Color", new Color(100, 0f, .1f, .1f));
        Debug.Log(gameObject + "lvl 3-1 upgrade");
    }
    protected virtual void lv3_2_upgrade()
    {
        transform.Find("UpgradeSphere").GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 100, 0, .1f));
        Debug.Log(gameObject + "lvl 3-2 upgrade");
    }
    protected virtual void lv3_3_upgrade()
    {
        transform.Find("UpgradeSphere").GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 0, 100, .1f));
        Debug.Log(gameObject + "lvl 3-3 upgrade");
    }




    // Tower range visualization via gizmos 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
}
