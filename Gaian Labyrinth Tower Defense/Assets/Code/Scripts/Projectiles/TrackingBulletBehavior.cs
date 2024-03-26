using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBulletBehavior : BulletBehavior
{
    public Transform target;
    //public string targeting;
    public List<GameObject> enemies = new List<GameObject>();
    public float range;

    public float turnSpeed;


    protected override void Start()
    {
        //damage gets set in tower that instantiates this projectile.
        speed = 25f;
        range = 5f;
        turnSpeed = 35f;
        Destroy(gameObject, 5);

    }

    public override void SetTarget(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (target != null)
        {
            MoveTowardTarget();
        }
        else
        {
            UpdateTarget();
            if (target == null)
                Destroy(gameObject);
        }
    }

    protected void MoveTowardTarget()
    {
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        //transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        //changed Translate direction to forward, and added gradual rotation toward target
        transform.Translate(transform.forward * distanceThisFrame, Space.World);

        Quaternion targetingRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetingRotation, Time.deltaTime * turnSpeed);
    }

    public bool AlreadyHit(GameObject enemy)
    {
        foreach (var e in HitEnemies)
        {
            if (enemy == e.gameObject)
                return true;
        }
        return false;
    }


    public void ScanForEnemies()
    {
        enemies.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Enemy")
            {
                enemies.Add(hitCollider.gameObject);
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
                        if (distanceToEnemy < shortestDistance && !AlreadyHit(enemy))
                        {
                            shortestDistance = distanceToEnemy;
                            nearestEnemy = enemy;
                        }
                    }
                    // Verify the closest enemy is within the tower range and assign as target if true
                    if(nearestEnemy != null && shortestDistance <= range)
                    {
                        target = nearestEnemy.transform;
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
                        if (currentEnemyHealth > highestHealth && !AlreadyHit(enemy))
                        {
                            highestHealth = currentEnemyHealth;
                            strongestEnemy = enemy;
                        }
                    }
                    if(strongestEnemy != null)
                    {
                        target = strongestEnemy.transform;
                    }
                    break;
                case "Weak":
                    float lowestHealth = Mathf.Infinity;
                    GameObject weakestEnemy = null;
                    foreach (GameObject enemy in enemies)
                    {
                        float currentEnemyHealth = enemy.GetComponent<EnemyBehavior>().currentHealth;
                        if (currentEnemyHealth < lowestHealth && !AlreadyHit(enemy))
                        {
                            lowestHealth = currentEnemyHealth;
                            weakestEnemy = enemy;
                        }
                    }
                    if(weakestEnemy != null)
                    {
                        target = weakestEnemy.transform;
                    }
                    break;
            }
        }
        catch
        {

        }
    }

    public override void GetTargetInfo()
    {
        ScanForEnemies();
        UpdateTarget();
    }
}
