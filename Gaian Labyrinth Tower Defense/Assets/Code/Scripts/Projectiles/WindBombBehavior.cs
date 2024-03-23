using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBombBehavior : TrackingBulletBehavior
{
    [Header("Unity Fields")]
    public GameObject WindExplosionPrefab;
    
    public float duration; 

    // Update is called once per frame
    void Update()
    {
        base.Update();
        //transform.Rotate(0, 20, 0, Space.Self);
    }

    public override void HitTarget(GameObject hitEnemy)
    {
        GameObject explosion = Instantiate(WindExplosionPrefab, transform.position, transform.rotation);
        Debug.Log("my duration is :" + duration);
        explosion.GetComponent<WindExplosionBehavior>().duration = this.duration;
        explosion.GetComponent<WindExplosionBehavior>().damage = this.damage;
        Destroy(gameObject);
    }
}
