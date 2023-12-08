using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : ProjectileBehavior
{
    public float speed;
    public float damage;
    public float pierceAMT;

    // Update is called once per frame
    public virtual void Start()
    {
        speed = 50f;
        damage = 1f;
        pierceAMT = 1;
        Destroy(gameObject, 5);

    }

    public virtual void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HitTarget(other.gameObject);
        }
        
    }

    public virtual void HitTarget(GameObject hitEnemy)
    {
        EnemyBehavior e = hitEnemy.GetComponent<EnemyBehavior>();
        e.takeDamage(damage, gameObject);
        pierceAMT -= 1;

        if (pierceAMT < 0)
            Destroy(gameObject);

        GetTargetInfo();
    }

    virtual void GetTargetInfo()
    {

    }
}
