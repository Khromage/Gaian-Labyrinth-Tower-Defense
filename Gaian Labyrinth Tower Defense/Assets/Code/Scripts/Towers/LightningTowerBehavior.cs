using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTowerBehavior : TowerBehavior
{

    private int chainCount;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        chainCount = 2;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    //This function gets called at lvl1, lvl2, and lvl3_1
    protected override void lv1_Attack()
    {
        LightningProjBehavior projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<LightningProjBehavior>();
        projectile.damage = damage;
        projectile.GetComponent<LightningProjBehavior>().SetPierceAMT(chainCount);
        if (projectile != null)
            projectile.SetTarget(target.transform);
        projectile.targeting = targetingMode;

        Debug.Log($"lightning proj chainCount: {chainCount}");
        Debug.Log($"lightning proj pierceAMT: {projectile.GetComponent<LightningProjBehavior>().pierceAMT}");
        Debug.Log($"lightning proj target: {projectile.GetComponent<LightningProjBehavior>().target}");


        //IN ArcBehavior
        //call GetTargetInfo(); //for scanning and changing target
        //foreach pierce, instantiate a visual toward target, hit the target enemy, if(i>0) find next target
        //Or instead:
        //if pierce > 0, get new target, make a new guy aiming at new target (and pass the HitEnemies List) with -1 pierceAMT
    }

    //better arc
    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        damage = 10f;
        fireRate = 1.5f;
        chainCount = 3;
    }

    //best arc
    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        damage = 14f;
        fireRate = 2f;
        chainCount = 4;
    }
    //lightning strike
    protected override void lv3_2_upgrade()
    {
        base.lv3_2_upgrade();
        damage = 25f;
        chainCount = 0;
    }
    //static/ball lightning 
    protected override void lv3_3_upgrade()
    {
        base.lv3_3_upgrade();
        damage = 2f;
        fireRate = 4f;
        chainCount = 1;
    }

}
