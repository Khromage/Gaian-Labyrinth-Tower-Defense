using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public GameObject Bullet;
    //public Tech dmgincrease;


    private void Start()
    {
        //if (dmgincrease.invested == true)
          //  Damage += 100;
    }

    public override void Fire()
    {
        GameObject b = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
        b.GetComponent<ProjectileBehavior>().damage = Damage;
    }
}
