using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CentaurBehavior : EnemyBehavior
{

    public float speedUpTime;

    void Start(){
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.acceleration = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void enemySpeedUp()
    {
        
    }
}

