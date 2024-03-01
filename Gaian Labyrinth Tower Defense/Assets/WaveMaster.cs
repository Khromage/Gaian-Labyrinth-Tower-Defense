using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMaster : ProjectileBehavior
{
    //this is parent behavior of each overarching attack from the wave tower
    public List<GameObject> waterWaves = new List<GameObject>();
    public List<GameObject> enemiesHit = new List<GameObject>();
    public List<GridTile> tilesCovered = new List<GridTile>();
    //public GridTile[] tilesCovered;
    public bool hadChildren = false;

    public void Update()
    {
        if (transform.childCount != 0)
            hadChildren = true;
        if (transform.childCount == 0 && hadChildren)
            Destroy(gameObject);
    }

}
