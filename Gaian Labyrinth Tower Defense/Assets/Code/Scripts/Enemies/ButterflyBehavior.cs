using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyBehavior : EnemyBehavior
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        getGroundOrientation();
    }

    void getGroundOrientation()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f))
        {
            transform.up = hit.normal;
        }
    }
}
