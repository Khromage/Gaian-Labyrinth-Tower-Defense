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
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

    }

    void HitTarget()
    {
        EnemyBehavior e = target.gameObject.GetComponent<EnemyBehavior>();
        e.takeDamage(damage, gameObject);
        Destroy(gameObject);
    }
}
