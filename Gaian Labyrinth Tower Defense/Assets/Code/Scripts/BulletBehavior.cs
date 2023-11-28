using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 50f;
    public float damage = 1f;

    // Update is called once per frame
    public virtual void Start()
    {
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
        Debug.Log("Applied damage: " + damage);
        Destroy(gameObject);
    }
}
