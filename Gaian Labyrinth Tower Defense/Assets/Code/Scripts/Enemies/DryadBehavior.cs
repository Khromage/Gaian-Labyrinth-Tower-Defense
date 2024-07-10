using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DryadBehavior : EnemyBehavior
{
    public String previousMode;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
       base.Update(); 
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "towerBuilding"){
            //Charm Inflicted on tower
            //Saving to other variable rn cause last mode might be different
            other.GetComponent<TowerBehavior>().enemies.Insert(0, gameObject);
            previousMode = other.GetComponent<TowerBehavior>().targetingMode;
            other.GetComponent<TowerBehavior>().targetingMode = "Charmed";
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.tag == "towerBuilding"){
            other.GetComponent<TowerBehavior>().targetingMode = previousMode;
        }
    }
}
