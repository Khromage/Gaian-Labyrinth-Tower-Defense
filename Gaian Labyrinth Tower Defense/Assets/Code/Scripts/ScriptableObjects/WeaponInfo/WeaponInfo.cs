using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/WeaponInfo", fileName = "WeaponInfo")]
public class WeaponInfo : ScriptableObject
{
    [field: SerializeField]
    public GameObject Prefab;

    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }

    [field: SerializeField]
    public WeaponUpgrade[] Upgrades { get; private set; }
}
