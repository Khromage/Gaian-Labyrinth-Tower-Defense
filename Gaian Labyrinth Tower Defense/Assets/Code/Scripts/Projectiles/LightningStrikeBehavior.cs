using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrikeBehavior : LightningProjBehavior
{

    private Camera cam;
    protected override void Start()
    {
        cam = GameObject.Find("PlayerCam").GetComponent<Camera>();
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
            Destroy(gameObject, .3f);
        }
        else
        {
            //shift my position
            transform.position = target.position;
        }

        //transform.LookAt(cam.transform, transform.up);
    }
}
