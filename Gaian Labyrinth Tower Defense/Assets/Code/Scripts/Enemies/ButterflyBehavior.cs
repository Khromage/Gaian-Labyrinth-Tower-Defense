using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ButterflyBehavior : EnemyBehavior
{
    private Rigidbody rb;
    private BoxCollider sporeCollider;
    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        sporeCollider = GetComponent<BoxCollider>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"){
            other.gameObject.GetComponent<Player>().setStatus("spored");
        }
    }

}

    
