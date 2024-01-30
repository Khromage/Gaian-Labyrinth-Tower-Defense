using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    RaycastHit hit;
    public GameObject Bullet;
   
    void Start()
    {
        FireCooldown = .2f;
        Automatic = true;
        Damage = 10;
        ProjRange = 500;
        manaCost = 8;
    }

    void Update()
    {
        CurrentCooldown -= Time.deltaTime;

    }

    // Update is called once per frame
    public override void Fire()
    {
        Transform FirePoint = transform.Find("FirePoint");
        Instantiate(Bullet, FirePoint.position, transform.rotation);
    }
}
