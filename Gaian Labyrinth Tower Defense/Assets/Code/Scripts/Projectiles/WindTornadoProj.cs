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

        //this is the range of scanning for enemies
        range = 5f;
        turnSpeed = 35f;

        //damage = .17f;
        //duration = 3f;
        knockback = .9f;
        dmgInterval = .17f;
        TimetoDMG = dmgInterval;
        initialScale = 1f;
        radius = 3f;
        Destroy(gameObject, duration);
        Debug.Log("i spawned");

    }

    // Update is called once per frame
    protected override void Update()
    {

        TimetoDMG += Time.deltaTime;
        if(target == null)
            GetTargetInfo();
        //Debug.Log("i got this many enemies in my list: " + enemies.Count);

        if (target == null)
        {
            return;
        }

        MoveTowardTarget();

        //Debug.Log("i am targetting" + target.name);

        if (Vector3.SqrMagnitude(transform.position - target.transform.position) < radius)
        {
            speed = 3f;
        }
        else
        {
            speed = 10f;
        }

        if (TimetoDMG > dmgInterval)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider collider in colliders)
            {
                if(collider.tag == "Enemy")
                {
                    EnemyBehavior e = collider.gameObject.GetComponent<EnemyBehavior>();
                    e.takeDamage(damage, gameObject);
                    e.transform.Translate(e.transform.forward * knockback);
                }
            }
            TimetoDMG = 0;
        }

    }

/*
    protected void FixedUpdate()
    {
        if (TimetoDMG > dmgInterval || IHIT)
        {
            TimetoDMG = 0;
 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("im hurting now");
        if (TimetoDMG >= dmgInterval)
        {
            if (other.tag == "Enemy")
            {
                EnemyBehavior e = other.gameObject.GetComponent<EnemyBehavior>();
                e.takeDamage(damage, gameObject);
                e.transform.Translate(e.transform.forward * knockback);
            }
        }

    }*/

    public override void HitTarget(GameObject hitEnemy)
    {
        return;
    }
}
