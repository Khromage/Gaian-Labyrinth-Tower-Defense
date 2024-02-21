using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/WeaponList", fileName = "WeaponList")]
public class WeaponList : ScriptableObject
{
    [field: SerializeField]
    public WeaponInfo[] WeaponDataSet;

    public GameObject GetWeapon(int ID)
    {
        return WeaponDataSet[ID].Prefab;
    }

    public Sprite GetWeaponIcon(int ID)
    {
        if (ID < 0 || ID >= WeaponDataSet.Length)
        {
            return null;
        }
        return WeaponDataSet[ID].Icon;
    }

}
