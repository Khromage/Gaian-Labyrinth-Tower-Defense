using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneTowerBehavior : TowerBehavior
{

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }


    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        damage = 2f;
        fireRate = 1.5f;
    }
    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        damage = 2f;
        fireRate = 6f;
    }
    protected override void lv3_2_upgrade()
    {
        base.lv3_2_upgrade();
        damage = 7f;
    }
    protected override void lv3_3_upgrade()
    {
        base.lv3_3_upgrade();
        damage = 2f;
        projectilePrefab.GetComponent<TrackingBulletBehavior>().pierceAMT = 2;
    }

}
