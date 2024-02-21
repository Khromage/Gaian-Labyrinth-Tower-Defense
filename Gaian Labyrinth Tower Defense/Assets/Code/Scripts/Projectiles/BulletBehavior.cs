using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : ProjectileBehavior
{
    public float speed;
    public float pierceAMT;
    public List<EnemyBehavior> HitEnemies = new List<EnemyBehavior>();

    // Update is called once per frame
    protected virtual void Start()
    {
        speed = 50f;
        //how many enemies you wanna pierce
        pierceAMT = 1;
        Destroy(gameObject, 5);

    }

    protected virtual void Update()
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
        //Debug.Log("Hit Enemy for: " + damage);
        e.takeDamage(damage, gameObject);
        pierceAMT -= 1;

        if (pierceAMT < 0)
            Destroy(gameObject);

        HitEnemies.Add(e);
        GetTargetInfo();

        
    }

    public virtual void GetTargetInfo()
    {

    }
}
