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
        //change model
        //change projectile, if necessary
    }
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
    protected override void lv3_3_upgrade()
    {
        base.lv3_3_upgrade();
        //change model
        //change projectile, if necessary
    }


    protected override void lv3_2_Attack()
    {
        //ZONE OF VULNERABILITY
        lv1_Attack();
    }
    protected override void lv3_3_Attack()
    {
        //PULSE DAMAGE AROUND TOWER
        lv1_Attack();
    }

}
