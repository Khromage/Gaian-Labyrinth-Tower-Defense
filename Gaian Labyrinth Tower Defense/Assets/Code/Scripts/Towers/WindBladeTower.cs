using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTower : TowerBehavior
{
    //prefabs 
    public GameObject bladePrefab;
    public GameObject tornadoPrefab;
    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        //change model
        //change projectile, if necessary
    }
    protected override void lv3_2_upgrade()
    {
        base.lv3_2_upgrade();
        //change model
        //change projectile, if necessary
    }
}
