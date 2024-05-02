using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CentaurBehavior : EnemyBehavior
{
    protected float maxSpeed;
    protected float slowestSpeed;
    private Vector3 nextCorner;
    private Vector3 newCorner;
    private Vector3 currentPosition;

    public override void Start(){
        base.Start();
        maxSpeed = 5f;
        slowestSpeed = 1f;
        getNextCorner();
    }

    // Update is called once per frame
    public override void Update()
    {   
        base.Update();
        if (Vector3.Distance(transform.position, newCorner) > 5f){
            speedUp();
        }else{
            slowDown();
            getNextCorner();
            currentPosition = new Vector3(transform.position.x, 0f, transform.position.z);
            newCorner = new Vector3(nextCorner.x, 0f, nextCorner.z);
        }
    }

    private void getNextCorner(){
        nextCorner = GetComponent<NavMeshAgent>().steeringTarget;
        Debug.Log(nextCorner);
    }

    private void speedUp(){
        if (GetComponent<NavMeshAgent>().speed < 1f){
            GetComponent<NavMeshAgent>().speed = maxSpeed;
        }
    }

    private void slowDown(){
        if (GetComponent<NavMeshAgent>().speed > 1f){
            GetComponent<NavMeshAgent>().speed = slowestSpeed;
        }
    }

}

