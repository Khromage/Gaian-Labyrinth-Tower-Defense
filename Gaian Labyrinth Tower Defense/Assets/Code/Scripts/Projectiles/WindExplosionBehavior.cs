using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindExplosionBehavior : MonoBehaviour
{
    public float duration;
    public float damage;
    //how far back the enemy gets sent back per damage tick
    public float knockback;
    //how often to hit
    public float dmgInterval;
    public float TimetoDMG;

    //this should be taken from wind bomb
    public float initialScale;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        //damage = .17f;
        //duration = 3f;
        knockback = .09f;
        dmgInterval = .2f;
        TimetoDMG = dmgInterval;
        initialScale = 1f;
        radius = 3f;
        Destroy(gameObject, duration);

    }

    // Update is called once per frame
    void Update()
    {
        TimetoDMG += Time.deltaTime;
        if (TimetoDMG >= dmgInterval)
        {
            //making this list each time bc enemies might die/ mvoe out and more might move in
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
}
