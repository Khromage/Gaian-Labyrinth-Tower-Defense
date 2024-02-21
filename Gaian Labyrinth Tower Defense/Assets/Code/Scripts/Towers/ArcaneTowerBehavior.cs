using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneTowerBehavior : TowerBehavior
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        towerName = "arcane";


        cost = 30;
        lv2_cost = 40;
        lv3_1_cost = 80;
        lv3_2_cost = 70;
        lv3_3_cost = 65;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }


    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        currentDamage = 2f;
        fireRate = 1.5f;
    }
    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        currentDamage = 2f;
        fireRate = 6f;
    }
    protected override void lv3_2_upgrade()
    {
        base.lv3_2_upgrade();
        currentDamage = 7f;
    }
    protected override void lv3_3_upgrade()
    {
        base.lv3_3_upgrade();
        currentDamage = 2f;
        projectilePrefab.GetComponent<TrackingBulletBehavior>().pierceAMT = 2;
    }

}
