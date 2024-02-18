using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAttack : MonoBehaviour
{
    public GameObject[] enemiesHit;
    public GridTile[] tilesCovered;
    public bool hadChildren = false;

    public void Update()
    {
        if (transform.childCount != 0)
            hadChildren = true;
        if (transform.childCount == 0 && hadChildren)
            Destroy(Gameobject);
    }
}
