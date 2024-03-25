using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTowerBehavior : TowerBehavior
{
    [SerializeField]
    private GameObject aimSource;

    [SerializeField]
    private GameObject frame;
    [SerializeField]
    private GameObject lens;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        cost = 40;
        lv2_cost = 50;
        lv3_1_cost = 70;
        lv3_2_cost = 80;
        lv3_3_cost = 110;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    protected override void LookTowardTarget()
    {
        //rotate frame around tower's y axis
        //rotate lens around frame's x axis

        // Generate vector pointing from tower towards target enemy and use it to rotate the tower head 
        Vector3 direction = target.transform.position - aimSource.transform.position;
        Quaternion targetingRotation = Quaternion.LookRotation(direction);

        //frame.transform.localRotation = targetingRotation;

        // Using Lerp to smooth transition between target swaps instead of snapping to new targets
        //Vector3 rotation = Quaternion.Lerp(frame.transform.rotation, targetingRotation, Time.deltaTime * turnSpeed).eulerAngles;
        Vector3 rotation = targetingRotation.eulerAngles;
        
        Debug.Log(rotation);
        frame.transform.localRotation = Quaternion.Euler(frame.transform.rotation.x, rotation.y, frame.transform.rotation.z);
        lens.transform.rotation = targetingRotation;
    }


    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        damage = 7f;
    }

    //rainbow
    protected override void lv3_1_upgrade()
    {
        base.lv3_1_upgrade();
        damage = 9f;
        projectilePrefab.GetComponent<ExplodingBulletBehavior>().blastRadius += 2f;
    }
    //laser, ramps in damage
    protected override void lv3_2_upgrade()
    {
        base.lv3_2_upgrade();
        damage = 2f;
        fireRate = 5f;
    }
    //beacon
    protected override void lv3_3_upgrade()
    {
        base.lv3_3_upgrade();
        Debug.Log("Beacon upgrade. not implemented yet");
        damage = 1f;
        fireRate = 4f;
    }
}
