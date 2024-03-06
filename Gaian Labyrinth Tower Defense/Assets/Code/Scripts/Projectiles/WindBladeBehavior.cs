using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeBehavior : TrackingBulletBehavior
{
    public float DistTilOrbit = 20f;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //if the bullet is DistTilOrbit far from the target
        while(Mathf.Abs((transform.position - target.transform.position).magnitude) <= DistTilOrbit)
            transform.RotateAround(target.transform.position, Vector3.up, 20 * Time.deltaTime);

    }
}
