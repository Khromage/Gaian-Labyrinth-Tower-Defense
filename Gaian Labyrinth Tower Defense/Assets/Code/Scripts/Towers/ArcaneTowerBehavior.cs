using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class ArcaneTowerBehavior : TowerBehavior
{

    [SerializeField]
    private GameObject VulnerabilityZone;

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
    
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
        // only if vulnerability branch
        if(other.tag == "Enemy" && (currentBranch == 2))
        {
            other.gameObject.GetComponent<EnemyBehavior>().enterVulnerabilityZone();
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        
        // only if vulnerability branch
        if(other.tag == "Enemy" && (currentBranch == 2))
        {
            other.gameObject.GetComponent<EnemyBehavior>().exitVulnerabilityZone();
        }
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
        VulnerabilityZone.SetActive(true);
        VulnerabilityZone.transform.localScale = new Vector3(range, range, range);
        gameObject.GetComponent<SphereCollider>().radius = range;
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
        // lv1_Attack(); no attack, just zone
    }
    protected override void lv3_3_Attack()
    {
        //PULSE DAMAGE AROUND TOWER
        lv1_Attack();
    }

}
