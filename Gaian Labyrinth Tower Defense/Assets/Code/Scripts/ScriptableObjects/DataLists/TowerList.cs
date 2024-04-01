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

    public string GetTowerName(int ID)
    {
        return TowerDataSet[ID].Name;
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

    public string GetTowerDescription(int ID)
    {
        return TowerDataSet[ID].Description.Replace("{Name}", TowerDataSet[ID].Name)
                                    .Replace("{Damage}", TowerDataSet[ID].Damage.ToString())
                                    .Replace("{Range}", TowerDataSet[ID].Range.ToString())
                                    .Replace("{FireRate}", TowerDataSet[ID].FireRate.ToString())
                                    .Replace("{Cost}", TowerDataSet[ID].Cost.ToString());
    }
}