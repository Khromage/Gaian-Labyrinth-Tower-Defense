using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTowerBehavior : TowerBehavior
{

    public GameObject pulsePrefab;

    [Header("Tower Stats")]

    public float maxDamage = 5f;
    //private float fireCountdown = 0f;

    //[Header("Unity Fields")]

    //SphereCollider detectionZone;


    // Call the targeting function twice a second to scan for enemies
    public override void Start()
    {
        cost = 8;
        fireRate = .3f;

        //detectionZone = GetComponent<SphereCollider>();
        //detectionZone.radius = range;
    }

    private void OnEnable()
    {

    }
    private void onDisable()
    {
		
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


    public override void Update()
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
