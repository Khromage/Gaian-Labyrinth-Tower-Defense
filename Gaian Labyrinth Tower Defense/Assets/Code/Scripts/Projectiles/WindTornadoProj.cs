using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTornadoProj : TrackingBulletBehavior
{
    public float duration;
    //public float damage;
    //how far back the enemy gets sent back per damage tick
    public float knockback;
    //how often to hit
    public float dmgInterval;
    public float TimetoDMG;

    //this should be taken from wind bomb
    public float initialScale;
    public float radius;

    // Start is called before the first frame update
    protected override void Start()
    {
        speed = 10f;

        damage = .17f;
        duration = 3f;
        knockback = .09f;
        dmgInterval = .17f;
        TimetoDMG = dmgInterval;
        initialScale = 1f;
        radius = 3f;
        Destroy(gameObject, duration);

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Vector3.SqrMagnitude(transform.position - target.transform.position) < radius)
        {
            speed = 3f;
        }
        else
        {
            speed = 10f;
        }

        TimetoDMG += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (TimetoDMG >= dmgInterval)
        {
            if (other.tag == "Enemy")
            {
                EnemyBehavior e = other.gameObject.GetComponent<EnemyBehavior>();
                e.takeDamage(damage, gameObject);
                e.transform.Translate(e.transform.forward * knockback);
            }
            TimetoDMG = 0;
        }
    }

    public override void HitTarget(GameObject hitEnemy)
    {
        return;
    }
}
