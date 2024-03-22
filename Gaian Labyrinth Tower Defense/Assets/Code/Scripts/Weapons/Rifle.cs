using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    public GameObject Bullet;

    public override void Fire()
    {
        Instantiate(Bullet, FirePoint.position, FirePoint.rotation);
    }
}
