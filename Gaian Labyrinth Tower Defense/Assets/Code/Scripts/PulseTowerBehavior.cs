using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTowerBehavior : MonoBehaviour
{

    public GameObject pulsePrefab;

    [Header("Tower Stats")]

    public float range = 20f;
    public float maxDamage = 5f;
    public float fireRate = .3f;
    private float fireCountdown = 0f;

    [Header("Unity Fields")]

    public string enemyTag = "Enemy";

    public Transform firePoint;

    public List<GameObject> enemies = new List<GameObject>();
    SphereCollider detectionZone;

    public int cost;

    // Call the targeting function twice a second to scan for enemies
    void Start()
    {

    }

    private void OnEnable()
    {
        detectionZone = GetComponent<SphereCollider>();
        detectionZone.radius = range;
        EnemyBehavior.OnEnemyDeath += removeEnemyFromList;
    }
    private void onDisable()
    {
        EnemyBehavior.OnEnemyDeath -= removeEnemyFromList;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hello something. love, pulse tower");
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("hello enemy. love, pulse tower");
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


    void Update()
    {
        if (fireCountdown <= 0)
        {
            Pulse();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Pulse()
    {
        GameObject pulse = Instantiate(pulsePrefab, firePoint.position, firePoint.rotation, gameObject.transform);
        //pulse.GetComponent<PulseBehavior>().maxRadius = range;
        //pulse.GetComponent<PulseBehavior>().maxDamage = maxDamage;
    }

    // Tower range visualization via gizmos 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
