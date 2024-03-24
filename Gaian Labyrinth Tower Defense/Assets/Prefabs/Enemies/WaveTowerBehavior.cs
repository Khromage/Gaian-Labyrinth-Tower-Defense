using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTowerBehavior : TowerBehavior
{
    //
    public float WaveDistance;
    public GameObject WavePrefab;
    public List<GameObject> MasterList = new List<GameObject>();


    protected override void Shoot()
    {
        GameObject waveMaster = new GameObject("wave master"); 
        waveMaster.AddComponent<WaveMaster>();
        MasterList.Add(waveMaster);
        GridTile targetTile = target.GetComponent<EnemyBehavior>().currTile;
        GameObject attackWave = Instantiate(WavePrefab, targetTile.transform.position + new Vector3(0f, .1f, 0f), target.transform.rotation, waveMaster.transform); 
        attackWave.GetComponent<WaveWaterBehavior>().tilesLeft = WaveDistance;
        // attackWave.GetComponent<WaveWaterBehavior>().damage = currentDamage;
        waveMaster.GetComponent<WaveMaster>().waterWaves.Add(attackWave);
    }

    void Update()
    {
        base.Update();
    }
}
