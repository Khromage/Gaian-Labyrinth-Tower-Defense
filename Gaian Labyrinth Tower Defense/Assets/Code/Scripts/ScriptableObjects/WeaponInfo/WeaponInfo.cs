using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/WeaponInfo", fileName = "WeaponInfo")]
public class WeaponInfo : ScriptableObject
{
    [field: SerializeField]
    public float Damage { get; private set; }

    [field: SerializeField]
    public float FireCooldown { get; private set; }

    [field: SerializeField]
    public float ManaCost { get; private set; }

    [field: SerializeField]
    public bool Automatic { get; private set; }

    [field: SerializeField]
    public float ProjectileRange { get; private set; }

    [field: SerializeField]
    public GameObject Prefab;

    [field: SerializeField]
    public Sprite Icon { get; private set; }

    [field: SerializeField]
    public string Description { get; private set; }

    [field: SerializeField]
    public WeaponUpgrade[] Upgrades { get; private set; }
}
