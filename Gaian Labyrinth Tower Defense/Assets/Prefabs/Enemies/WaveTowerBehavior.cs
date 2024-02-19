using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTowerBehavior : TowerBehavior
{
    //
    public float WaveDistance = 7;
    public GameObject WavePrefab;
    public List<WaveMaster> MasterList = new List<WaveMaster>();
    public void Shoot()
    {
        WaveMaster waveMaster = new WaveMaster();
        MasterList.Add(waveMaster);
        //GridTile targetTile = target.GetComponent<EnemyBehavior>().successorTile.successor.successor;
        GameObject attackWave = Instantiate(WavePrefab, target.GetComponent<EnemyBehavior>().successorTile.successor.successor.transform.position, target.transform.rotation, waveMaster.transform);
        attackWave.GetComponent<WaveWaterBehavior>().tilesLeft = WaveDistance;
        waveMaster.waterWaves.Add(attackWave);
    }
}
