using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrikeBehavior : LightningProjBehavior
{

    protected override void Start()
    {
        //do nothing
    }
    // Update is called once per frame
    protected override void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (target == null)
        {
            Destroy(gameObject, .1f);
        }
        else
        {
            //shift my position
            transform.position = target.position;
        }
    }
}
