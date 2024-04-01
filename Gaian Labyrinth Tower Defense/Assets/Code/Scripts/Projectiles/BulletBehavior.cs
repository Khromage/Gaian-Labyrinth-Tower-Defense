using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : ProjectileBehavior
{
    public float speed;
    public int pierceAMT;
    public List<EnemyBehavior> HitEnemies = new List<EnemyBehavior>();

    // Update is called once per frame
    protected virtual void Start()
    {
        speed = 50f;
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

    public void SetPierceAMT(int pierceAMT)
    {
        this.pierceAMT = pierceAMT;
    }

    public virtual void GetTargetInfo()
    {

    }
}
