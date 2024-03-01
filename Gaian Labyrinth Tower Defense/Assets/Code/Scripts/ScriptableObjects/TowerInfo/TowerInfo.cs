using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/TowerInfo", fileName = "TowerInfo")]
public class TowerInfo : ScriptableObject
{
    [field: SerializeField]
    public GameObject Prefab;

    [field: SerializeField]
    public int BaseCost;

    [field: SerializeField]
    public int Lv2Cost;

    [field: SerializeField]
    public int[] Lv3Cost;

    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }
}
