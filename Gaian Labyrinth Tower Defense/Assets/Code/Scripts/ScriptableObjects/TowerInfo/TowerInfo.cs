using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/TowerInfo", fileName = "TowerInfo")]
public class TowerInfo : ScriptableObject
{
    [field: SerializeField]
    public string Name;

    [field: SerializeField]
    public GameObject Prefab;

    [field: SerializeField]
    public float Damage;

    [field: SerializeField]
    public float Range;

    [field: SerializeField]
    public float FireRate;
    
    [field: SerializeField]
    public int Value;

    [field: SerializeField]
    public string Description = "This tower is {Name} and costs {Price}.";

    public string GetDescription(string towerName, float towerPrice)
    {
        return Description.Replace("{Name}", towerName)
                            .Replace("{Price}", towerPrice.ToString());
    }
    
    [field: SerializeField]
    public int BaseCost;

    [field: SerializeField]
    public int Lv2Cost;

    [field: SerializeField]
    public int[] Lv3Cost;

    [field: SerializeField]
    public Sprite Icon { get; private set; }

}
