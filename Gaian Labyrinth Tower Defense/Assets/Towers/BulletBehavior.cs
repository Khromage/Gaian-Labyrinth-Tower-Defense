using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Transform target;

    public float speed = 50f;
    public float damage  = 5f;

    public void Seek (Transform _target)
    {
        target = _target;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet Reached Target");
        if(other.gameObject.tag == "Enemy")
        {
            HitTarget();
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

       /* 
        if(direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        */

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        EnemyScript e = target.gameObject.GetComponent<EnemyScript>();
        e.takeDamage(damage, gameObject);
        Destroy(gameObject);
    }

}
