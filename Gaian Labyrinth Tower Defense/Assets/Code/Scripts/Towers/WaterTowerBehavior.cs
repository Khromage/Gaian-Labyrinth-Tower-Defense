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
        distance = lv1_distance;
    }
    protected override void lv1_Attack()
    {
        /*GameObject waveMaster = new GameObject("wave master"); 
        waveMaster.AddComponent<WaveMaster>();
        MasterList.Add(waveMaster);*/
        GridTile targetTile = target.GetComponent<EnemyBehavior>().currTile;
        GameObject attackWave = Instantiate(projectilePrefab, targetTile.transform.position + new Vector3(0f, .1f, 0f), target.transform.rotation); 
        attackWave.GetComponent<BasicWaveBehavior>().tilesLeft = distance;
        attackWave.GetComponent<BasicWaveBehavior>().damage = damage;
        //waveMaster.GetComponent<WaveMaster>().waterWaves.Add(attackWave);


    }

    protected override void lv2_upgrade()
    {
        base.lv2_upgrade();
        distance = lv2_distance;
    }

    //level 3 branch projectile prefabs
    public GameObject wavePrefab;
    public GameObject riptidePrefab;
    protected override void lv3_1_upgrade()
    {
        projectilePrefab = wavePrefab;
        base.lv3_2_upgrade();
    }

    protected override void lv3_1_Attack()
    {
        GameObject waveMaster = new GameObject("wave master"); 
        waveMaster.AddComponent<WaveMaster>();
        MasterList.Add(waveMaster);
        GridTile targetTile = target.GetComponent<EnemyBehavior>().currTile;
        GameObject attackWave = Instantiate(projectilePrefab, targetTile.transform.position + new Vector3(0f, .1f, 0f), target.transform.rotation, waveMaster.transform); 
        attackWave.GetComponent<WaveWaterBehavior>().tilesLeft = distance;
        attackWave.GetComponent<WaveWaterBehavior>().damage = damage;
        waveMaster.GetComponent<WaveMaster>().waterWaves.Add(attackWave);
    }

    protected override void lv3_2_upgrade()
    {
        projectilePrefab = riptidePrefab;
        base.lv3_2_upgrade();
        //change model
        //change projectile, if necessary
    }

    protected override void lv3_2_Attack()
    {
        Instantiate(projectilePrefab, target.GetComponent<EnemyBehavior>().currTile.transform.position + new Vector3(0f, .1f, 0f), target.transform.rotation); 
    }
}
