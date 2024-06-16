using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    public GameObject Beam;
    public AudioSource LaserFireSFX;
    //public Tech dmgincrease;

    protected override void Start()
    {
        base.Start();
        LaserFireSFX = GetComponent<AudioSource>();
       // if (dmgincrease.invested == true)
         //   Damage = 100f;
    }

    // Update is called once per frame
    public override void Fire()
    {
        Debug.Log("my FIREFIREFIREdamage is: " + Damage);
        LaserFireSFX.Play();

        GameObject laserInstance = Instantiate(Beam, FirePoint.position, FirePoint.rotation);
        Destroy(laserInstance, FireCooldown);

        Ray gunRay = new Ray(FirePoint.position, FirePoint.forward);
        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, ProjRange))
        {
            if (hitInfo.collider.gameObject.tag == "Enemy")
            {
                EnemyBehavior e = hitInfo.collider.gameObject.GetComponent<EnemyBehavior>();
                e.takeDamage(Damage, gameObject);
                
            }
        }
    }
}
