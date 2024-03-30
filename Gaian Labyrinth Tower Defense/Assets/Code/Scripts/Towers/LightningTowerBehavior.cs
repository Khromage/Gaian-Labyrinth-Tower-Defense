using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningTowerBehavior : TowerBehavior
{
    [SerializeField]
    private GameObject strikeProj;
    [SerializeField]
    private GameObject pulseProj;

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
    }
    protected override void lv3_3_Attack()
    {
        GameObject pulse = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        pulse.transform.localScale = Vector3.one * range;
        Destroy(pulse, fireRate / 4f);
        
        foreach (Collider e in Physics.OverlapSphere(firePoint.position, range))
        {
            if (e.CompareTag("Enemy"))
            {
                e.GetComponent<EnemyBehavior>().takeDamage(damage, pulse);
            }
        }
    }

    //better arc
    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        chainCount = 3;
    }

    //best arc
    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        chainCount = 4;
    }
    //lightning strike
    protected override void lv3_2_upgrade()
    {
        base.lv3_2_upgrade();
        chainCount = 0;
        projectilePrefab = strikeProj;
    }
    //static/ball lightning 
    protected override void lv3_3_upgrade()
    {
        base.lv3_3_upgrade();
        chainCount = 0;
        projectilePrefab = pulseProj;
    }

}
