using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/TowerInfo", fileName = "TowerInfo")]
public class TowerInfo : ScriptableObject
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public float Damage { get; private set; }

    [field: SerializeField]
    public float Range { get; private set; }

    [field: SerializeField]
    public float FireRate { get; private set; }
    
    [field: SerializeField]
    public int Cost { get; private set; }

    [field: SerializeField]
    [field: TextArea(3, 10)]
    public string Description { get; private set; }

    [field: SerializeField]
    public Branch Level2 { get; private set; }

    [field: SerializeField]
    public Branch[] Branches { get; private set; }

    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public GameObject NonfuncModel { get; private set; }

}

[Serializable]
public struct Branch
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public float Damage { get; private set; }

    [field: SerializeField]
    public float Range { get; private set; }

    [field: SerializeField]
    public float FireRate { get; private set; }
    
    [field: SerializeField]
    public int Cost { get; private set; }

    [field: SerializeField]
    [field: TextArea(3, 10)]
    public string Description { get; private set; }


}
