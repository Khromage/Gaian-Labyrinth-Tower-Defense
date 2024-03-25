using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/TowerList", fileName = "TowerList")]
public class TowerList : ScriptableObject
{
    [field: SerializeField]
    public TowerInfo[] TowerDataSet { get; private set; }

    public GameObject GetTower(int ID)
    {
        return TowerDataSet[ID].Prefab;
    }

    public Sprite GetTowerIcon(int ID)
    {
        if (ID < 0 || ID >= TowerDataSet.Length)
        { 
            return null; 
        }
        return TowerDataSet[ID].Icon;
    }

    public int GetTowerCost(int ID)
    {
        return TowerDataSet[ID].Cost;
    }
}