using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    RaycastHit hit;
    public GameObject Beam;
    private Transform PlayerCamera;

   
    void Start()
    {
        //PlayerCamera = Camera.main.transform;
        FireCooldown = .5f;
        Automatic = true;
        Damage = 20;
        BulletRange = 500;
    }

    void Update()
    {
        CurrentCooldown -= Time.deltaTime;
    }

    
    

    // Update is called once per frame
    public override void Fire()
    {
        Ray gunRay = new Ray(transform.position, transform.forward);
        Transform FirePoint = transform.Find("FirePoint");

        GameObject laserInstance = Instantiate(Beam, FirePoint.position, transform.rotation);
        Destroy(laserInstance, .2f);

        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, BulletRange))
        {
            if (hitInfo.collider.gameObject.tag == "Enemy")
            {
                EnemyBehavior e = hitInfo.collider.gameObject.GetComponent<EnemyBehavior>();
                e.takeDamage(Damage, gameObject);
                
            }
        }
    }
}
