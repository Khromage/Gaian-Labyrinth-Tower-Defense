using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBombBehavior : TrackingBulletBehavior
{
    [Header("Unity Fields")]
    public GameObject WindExplosionPrefab;
    


    // Update is called once per frame
    void Update()
    {
        base.Update();
        //transform.Rotate(0, 20, 0, Space.Self);
    }

    public override void HitTarget(GameObject hitEnemy)
    {
        Instantiate(WindExplosionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
