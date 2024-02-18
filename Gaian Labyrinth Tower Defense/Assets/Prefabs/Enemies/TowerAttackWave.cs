using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackWave : TowerBehavior
{
    public float WaveDistance = 7;
    public void Shoot()
    {
        GameObject attackInfo = Instantiate(waveAttack, transform.position, transform.rotation);
        GameObject attackWave = Instantiate(waveWater, target.successor.successor.successor, target.rotation, attackInfo.transform);
        attackWave.tilesLeft = WaveDistance;
    }
}
