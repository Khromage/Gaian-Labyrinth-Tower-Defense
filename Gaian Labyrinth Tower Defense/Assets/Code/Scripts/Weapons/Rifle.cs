using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public GameObject Bullet;

    public override void Fire()
    {
        GameObject b = Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
        b.GetComponent<ProjectileBehavior>().damage = Damage;
    }
}
