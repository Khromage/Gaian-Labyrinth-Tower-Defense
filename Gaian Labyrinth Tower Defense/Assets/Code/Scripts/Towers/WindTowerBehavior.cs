using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTowerBehavior : TowerBehavior
{
    public float lv1_duration;
    public float lv2_duration;
    private float duration;

    public override void Start()
    {
        base.Start();
        duration = lv1_duration;
    }
    protected override void lv1_Attack()
    {
        //copied bc funky stuff
        WindBombBehavior projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<WindBombBehavior>();
        projectile.damage = damage;
        //Debug.Log($"projectile damage: {damage}");
        if (projectile != null)
            projectile.SetTarget(target.transform);
        projectile.targeting = targetingMode;

        projectile.duration = duration;
    }

    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        duration = lv2_duration;
    }

    //level 3 branch projectile prefabs
    public GameObject bladePrefab;
    public GameObject tornadoPrefab;
    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        projectilePrefab = bladePrefab;
        Debug.Log($"New blade stats: {fireRate} per sec, {damage} damage");
        //change model
        //change projectile, if necessary
    }

    protected override void lv3_1_Attack()
    {
        base.lv1_Attack();
    }


    protected override void lv3_2_Attack()
    {
        WindTornadoProj projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<WindTornadoProj>();
        projectile.damage = damage;
        //Debug.Log($"projectile damage: {damage}");
        if (projectile != null)
            projectile.SetTarget(target.transform);
        projectile.targeting = targetingMode;

        projectile.duration = duration;
    }

    protected override void lv3_2_upgrade()
    {
        projectilePrefab = tornadoPrefab;
        base.lv3_2_upgrade();
        //change model
        //change projectile, if necessary
    }
}
