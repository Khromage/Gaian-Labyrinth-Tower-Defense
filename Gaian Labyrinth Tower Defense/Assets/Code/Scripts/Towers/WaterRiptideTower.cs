using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRiptideTower : TowerBehavior
{

    public GameObject riptidePrefab;
    // Start is called before the first frame update

    // Update is called once per frame
    protected override void Shoot()
    {
        GridTile targetTile = target.GetComponent<EnemyBehavior>().currTile;
        GameObject attackRiptide = Instantiate(riptidePrefab, targetTile.transform.position, target.transform.rotation);
        //assigning so values can be changed
        //attackRiptide.GetComponent<WaveRiptideBehavior>().tilesLeft = float value;
    }
}
