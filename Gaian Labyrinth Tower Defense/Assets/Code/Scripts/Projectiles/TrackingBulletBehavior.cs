using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBulletBehavior : BulletBehavior
{
    private Transform target;
    public string targeting;
    public List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        speed = 50f;
        damage = 5f;
    }

    public override void SetTarget(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    public override void Update()
    {

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        
    }

    public void ScanForEnemies()
    {
        enemies.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hitCollider in hitColliders)
        {
            if (other.gameObject.tag == "Enemy")
            {
                enemies.Add(other.gameObject);
            }
        }
    }

    void UpdateTarget()
    {

        // Iterate through the list and find the enemy with the shortest distance from the tower ("Close" targeting)
        try
        {
            switch (targeting)
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

    override void GetTargetInfo()
    {
        ScanForEnemies();
        UpdateTarget();
    }
}
