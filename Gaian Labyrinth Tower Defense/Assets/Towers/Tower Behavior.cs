using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    public Transform target;
    public float range = 5f;
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 5f;

    // Call the targeting function twice a second to scan for enemies
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        // Creates a list of all game objects with the enemy tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        // Iterate through the list and find the enemy with the shortest distance from the tower ("Close" targeting)
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
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
    }

    // Tower range visualization via gizmos 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
