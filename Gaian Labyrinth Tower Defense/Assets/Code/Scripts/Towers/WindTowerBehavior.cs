using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTowerBehavior : TowerBehavior
{
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
    protected override void lv3_2_upgrade()
    {
        projectilePrefab = tornadoPrefab;
        base.lv3_2_upgrade();
        //change model
        //change projectile, if necessary
    }
}
