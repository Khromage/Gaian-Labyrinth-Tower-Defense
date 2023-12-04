using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed;
    public float damage;
    public float pierceAMT;

    // Update is called once per frame
    void Start()
    {
        speed = 50f;
<<<<<<< Updated upstream
        damage = 5f;
=======
        damage = 1f;
        pierceAMT = 2;
        Destroy(gameObject, 5);
>>>>>>> Stashed changes
    }

    void Update()
    {
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            HitTarget(other.gameObject);
        }
    }

    void HitTarget(GameObject hitEnemy)
    {
        EnemyBehavior e = hitEnemy.GetComponent<EnemyBehavior>();
        e.takeDamage(damage, gameObject);
        pierceAMT -= 1;
        ScanForEnemies();
        UpdateTarget();
        if (pierceAMT <= 0)
            Destroy(gameObject);
    }
}
