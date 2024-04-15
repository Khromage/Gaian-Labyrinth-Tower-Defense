using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBulletBehavior : TrackingBulletBehavior
{
    public float blastRadius;

    [Header("Unity Fields")]
    public GameObject explosionPrefab;


    protected override void Start()
    {
        speed = 40f;
        //damage = 3f;
        blastRadius = 3f;
        turnSpeed = 180f;
    }
    public override void HitTarget(GameObject hitEnemy)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                EnemyBehavior e = collider.gameObject.GetComponent<EnemyBehavior>();
                //e.ApplyStatusEffect(0, new Burn(2f, 1f));
                e.takeDamage(damage, gameObject);
            }
        }
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        ParticleSystem explosionVFX = explosion.GetComponent<ParticleSystem>();
        explosionVFX.Play(true);
        Destroy(explosion, 1);
        Destroy(gameObject);
    }
}
