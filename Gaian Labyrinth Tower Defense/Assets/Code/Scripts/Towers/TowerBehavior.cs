using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour, Interactable
{

    public GameObject target;

    [Header("Tower Stats")]
    
    public string targetingMode;
    public float range = 10f;
    public float fireRate = 1f;
    public float fireCountdown = 0f;

    [Header("Unity Fields")]

    public string enemyTag = "Enemy";
    public GameObject InteractionIndicator;

    public Transform partToRotate;
    public float turnSpeed = 5f;

    public ProjectileBehavior projectilePrefab;
    public Transform firePoint;

    public List<GameObject> enemies = new List<GameObject>();
    public SphereCollider detectionZone;

    public int cost;

    // Call the targeting function twice a second to scan for enemies
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    private void OnEnable()
    {
        targetingMode = "Close";
        detectionZone = GetComponent<SphereCollider>();
        detectionZone.radius = range;
        EnemyBehavior.OnEnemyDeath += removeEnemyFromList;
        EnemyBehavior.OnEnemyReachedGoal += removeEnemyFromList;
        HideInteractButton();
    }
    private void OnDisable()
    {
        EnemyBehavior.OnEnemyDeath -= removeEnemyFromList;
        EnemyBehavior.OnEnemyReachedGoal -= removeEnemyFromList;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            {
                enemies.Add(other.gameObject);
            }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemies.Remove(other.gameObject);
        }
    }
    private void removeEnemyFromList(GameObject enemyToRemove)
    {
        enemies.Remove(enemyToRemove);
    }
    void UpdateTarget()
    {

        // Iterate through the list and find the enemy with the shortest distance from the tower ("Close" targeting)
        try
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
            }
        }
        catch
        {
            Debug.Log("Tower trying to target in empty enemy list. Would have sent a MissingReferenceException regarding the foreach (GameObject enemy in enemies)");
            detectionZone.enabled = false;
            enemies.Clear();
            detectionZone.enabled = true;
        }
    }
    void Update()
    {
        if(target == null)
            return;

        // Generate vector pointing from tower towards target enemy and use it to rotate the tower head 
        Vector3 direction = target.transform.position - transform.position;
        Quaternion targetingRotation = Quaternion.LookRotation(direction);
        // Using Lerp to smooth transition between target swaps instead of snapping to new targets
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, targetingRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }
    void Shoot()
    {
        ProjectileBehavior projectile = Instantiate (projectilePrefab, firePoint.position, firePoint.rotation, gameObject.transform) as ProjectileBehavior;
        if (projectile != null)
            projectile.SetTarget(target.transform);

        projectile.targeting = targetingMode;

    }
    public void Interact()
    {
        OpenTowerUI();
    }
    public void ShowInteractButton()
    {
        if (InteractionIndicator != null)
        {
            InteractionIndicator.SetActive(true); // Show the indicator
        }
    }
    public void HideInteractButton()
    {
        if (InteractionIndicator != null)
        {
            InteractionIndicator.SetActive(false); // Hide the indicator
        }
    }

    void OpenTowerUI()
    {
        // display ui in screen space
    }
    
    // Tower range visualization via gizmos 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
