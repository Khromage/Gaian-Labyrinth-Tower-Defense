using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTowerBehavior : TowerBehavior
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

    //maybe just override lv1_attack? just need the pierce thing right? just give the projectile the pierceAMT
    protected override void Shoot()
    {
        ProjectileBehavior projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation).GetComponent<ProjectileBehavior>();
        projectile.damage = damage;
        projectile.GetComponent<ArcBehavior>().pierceAMT = 2;
        if (projectile != null)
            projectile.SetTarget(target.transform);
        projectile.targeting = targetingMode;

        Debug.Log($"lightning proj target: {projectile.GetComponent<ArcBehavior>().target}");
    }

    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        damage = 10f;
        fireRate = 1.5f;
        projectilePrefab.GetComponent<TrackingBulletBehavior>().pierceAMT = 3;
    }
    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        damage = 14f;
        fireRate = 2f;
        projectilePrefab.GetComponent<TrackingBulletBehavior>().pierceAMT = 4;
    }
    protected override void lv3_2_upgrade()
    {
        base.lv3_2_upgrade();
        damage = 25f;
    }
    protected override void lv3_3_upgrade()
    {
        base.lv3_3_upgrade();
        damage = 2f;
        fireRate = 4f;
    }

}
