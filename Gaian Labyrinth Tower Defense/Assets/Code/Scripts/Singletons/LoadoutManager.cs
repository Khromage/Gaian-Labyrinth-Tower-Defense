using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LoadoutManager : ScriptableSingleton<LoadoutManager>
{

    public int[] EquippedTowerIDs;
    public int[] EquippedWeaponIDs;

    public bool[] EquippedTechNodeIDs;


    // Start is called before the first frame update
    void Start()
    {
        EquippedTowerIDs = SaveManager.Instance.EquippedTowerIDs;
        EquippedWeaponIDs = SaveManager.Instance.EquippedWeaponIDs;
        EquippedTechNodeIDs = SaveManager.Instance.EquippedTechNodeIDs;
    }

}
