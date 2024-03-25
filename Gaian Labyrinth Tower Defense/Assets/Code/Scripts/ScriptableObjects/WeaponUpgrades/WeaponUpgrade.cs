using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/WeaponUpgrade", fileName = "WeaponUpgrade")]
public class WeaponUpgrade : ScriptableObject
{
    [field: SerializeField]
    public WeaponInfo Weapon;

    [field: SerializeField]
    public float DamageIncrease;

    [field: SerializeField]
    public float FireRateIncrease;

    [field: SerializeField]
    public MonoBehaviour UniqueBehaviorScript;
}
