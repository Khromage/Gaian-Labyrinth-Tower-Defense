using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTowerBehavior : TowerBehavior
{
    public float lv1_distance;
    public float lv2_distance;

    private float distance;
    public List<GameObject> MasterList = new List<GameObject>();

    public override void Start()
    {
        base.Start();

        //temp
        lv1_distance = 4f;
        lv2_distance = 7f;
        


        distance = lv1_distance;
    }
    protected override void lv1_Attack()
    {
        /*GameObject waveMaster = new GameObject("wave master"); 
        waveMaster.AddComponent<WaveMaster>();
        MasterList.Add(waveMaster);*/
        GridTile targetTile = target.GetComponent<EnemyBehavior>().currTile;
        GameObject attackWave = Instantiate(projectilePrefab, targetTile.transform.position /*+ new Vector3(0f, .1f, 0f)*/, target.transform.rotation); 
        attackWave.GetComponent<BasicWaveBehavior>().tilesLeft = distance;
        attackWave.GetComponent<BasicWaveBehavior>().damage = damage;
        attackWave.GetComponent<BasicWaveBehavior>().lowLevel = true;
        //waveMaster.GetComponent<WaveMaster>().waterWaves.Add(attackWave);


    }

    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        distance = lv2_distance;
         
    }

    protected override void lv2_Attack()
    {
        GridTile targetTile = target.GetComponent<EnemyBehavior>().currTile;
        GameObject attackWave = Instantiate(projectilePrefab, targetTile.transform.position /*+ new Vector3(0f, .1f, 0f)*/, target.transform.rotation);
        attackWave.GetComponent<BasicWaveBehavior>().tilesLeft = distance;
        attackWave.GetComponent<BasicWaveBehavior>().damage = damage;
        attackWave.GetComponent<BasicWaveBehavior>().lowLevel = false;
    }



    //level 3 branch projectile prefabs
    public GameObject wavePrefab;
    public GameObject riptidePrefab;
    protected override void lv3_2_upgrade()
    {
        Debug.Log("i have a wave proj");
        projectilePrefab = wavePrefab;
        base.lv3_2_upgrade();
    }

    protected override void lv3_2_Attack()
    {
        Debug.Log("im going to wave attack");
        GameObject waveMaster = new GameObject("wave master"); 
        waveMaster.AddComponent<WaveMaster>();
        MasterList.Add(waveMaster);
        GridTile targetTile = target.GetComponent<EnemyBehavior>().currTile;
        GameObject attackWave = Instantiate(projectilePrefab, targetTile.transform.position + new Vector3(0f, .1f, 0f), target.transform.rotation, waveMaster.transform); 
        attackWave.GetComponent<WaveWaterBehavior>().tilesLeft = distance;
        attackWave.GetComponent<WaveWaterBehavior>().damage = damage;
        waveMaster.GetComponent<WaveMaster>().waterWaves.Add(attackWave);
    }

    protected override void lv3_1_upgrade()
    {
        Debug.Log("i have a riptide proj now");
        //Debug.Log(riptidePrefab);
        projectilePrefab = riptidePrefab;
        Debug.Log(projectilePrefab);
        base.lv3_1_upgrade();
        //change model
        //change projectile, if necessary
    }

    protected override void lv3_1_Attack()
    {
        Debug.Log("im going to riptide attack");
        Instantiate(projectilePrefab, target.GetComponent<EnemyBehavior>().currTile.transform.position, target.transform.rotation); 
    }
}
