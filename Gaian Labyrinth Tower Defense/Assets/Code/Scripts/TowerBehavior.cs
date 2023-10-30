using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    public Transform target;

    [Header("Tower Stats")]
    
    public float range = 10f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Unity Fields")]

    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 5f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    public List<GameObject> enemies = new List<GameObject>();
    SphereCollider detectionZone;

    public int cost;

    // Call the targeting function twice a second to scan for enemies
    void Start()
    {
        detectionZone = GetComponent<SphereCollider>();
        detectionZone.radius = range;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

    }

    private void OnEnable()
    {
        EnemyBehavior.OnEnemyDeath += removeEnemyFromList;
    }
    private void onDisable()
    {
        EnemyBehavior.OnEnemyDeath -= removeEnemyFromList;
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
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        // Iterate through the list and find the enemy with the shortest distance from the tower ("Close" targeting)
        try
        {
            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
        }
        catch
        {
            Debug.Log("Tower trying to target in empty enemy list. Would have sent a MissingReferenceException regarding the foreach (GameObject enemy in enemies)");
        }
        // Verify the closest enemy is within the tower range and assign as target if true
        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        } else 
        {
            target = null;
        }
    }

    void Update()
    {
        if(target == null)
            return;

        // Generate vector pointing from tower towards target enemy and use it to rotate the tower head 
        Vector3 direction = target.position - transform.position;
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
        GameObject bulletGO = Instantiate (bulletPrefab, firePoint.position, firePoint.rotation, gameObject.transform);
        BulletBehavior bullet = bulletGO.GetComponent<BulletBehavior>();

        if (bullet != null)
            bullet.Seek(target);
    }

    // Tower range visualization via gizmos 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
