using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackWave : TowerBehavior
{
    public float WaveDistance = 7;
    public GameObject waveAttack;
    public GameObject waveWater;
    public void Shoot()
    {
        GameObject attackInfo = Instantiate(waveAttack, transform.position, transform.rotation);
        GameObject attackWave = Instantiate(waveWater, target.successorTile.successor.successor.transform, target.rotation, attackInfo.transform);
        attackWave.tilesLeft = WaveDistance;
    }
}
