using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTower : TowerBehavior
{

    [Header("Tower Stats")]

    public List<GameObject> towers = new List<GameObject>();

    public override void Start()
    {
        base.Start();
        //scan in range for towers and add to tower list
    }

    private void OnEnable()
    {
        targetingMode = "Close";
        detectionZone.radius = range;

        HideInteractButton();
    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("found a something");
        if (other.gameObject.tag == "towerbuilding")
        {
            Debug.Log("its a tower");
            towers.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("lost a something");
        if (other.gameObject.tag == "towerbuilding")
        {
            Debug.Log("its a tower");
            towers.Remove(other.gameObject);
        }
    }

    private void removeTowerFromList(GameObject towerToRemove)
    {
        towers.Remove(towerToRemove);
    }


    public override void Update()
    {
        /*if (fireCountdown <= 0)
        {
            //givew a target the buff status
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;*/
    }


    // Tower range visualization via gizmos 
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
