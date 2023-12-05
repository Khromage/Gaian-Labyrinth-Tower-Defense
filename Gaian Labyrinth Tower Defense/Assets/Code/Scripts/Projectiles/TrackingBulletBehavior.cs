using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBulletBehavior : BulletBehavior
{
    private Transform target;

    public override void SetTarget(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    public override void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        
    }
}
