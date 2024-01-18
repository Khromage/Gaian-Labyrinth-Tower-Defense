using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTowerBehavior : TowerBehavior
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        cost = 50;
        lv2_cost = 60;
        lv3_1_cost = 100;
        lv3_2_cost = 110;
        lv3_3_cost = 130;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
    }
    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        currentDamage = 5f;
    }
    protected override void lv3_2_upgrade()
    {
        base.lv3_2_upgrade();
        currentDamage = 6f;
    }
    protected override void lv3_3_upgrade()
    {
        base.lv3_3_upgrade();
    }

}
