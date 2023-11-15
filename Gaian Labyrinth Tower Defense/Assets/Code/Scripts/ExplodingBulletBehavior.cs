using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBulletBehavior : TrackingBulletBehavior
{
    public float blastRadius;

    [Header("Unity Fields")]
    public GameObject explosionPrefab;


    public override void Start()
    {
        speed = 50f;
        damage = 5f;
        blastRadius = 10f;
    }
    public override void HitTarget(GameObject hitEnemy)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
            foreach (Collider collider in colliders)
            {
                if(collider.tag == "Enemy")
                {
                    EnemyBehavior e = collider.gameObject.GetComponent<EnemyBehavior>();
                    e.takeDamage(damage, gameObject);
                }
            }
            
            Debug.Log("EXUUUPLOOOOSION");
            GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
            explosion.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
}
