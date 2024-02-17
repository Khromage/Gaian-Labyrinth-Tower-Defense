using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : SpawnableSingleton<LoadoutManager>
{

    public int[] EquippedTowerIDs;
    public int[] EquippedWeaponIDs;

    public bool[] EquippedTechNodes;


    public int[] GetTowerLoadout()
    {
        if (EquippedTowerIDs != null)
        {
            return EquippedTowerIDs;
        }
        else
        {
            Debug.Log("Attempted to grab Tower Loadout from LoadoutManager, but no loadout initialized, so grabbing SaveManager's instead.");
            return SaveManager.Instance.EquippedTowerIDs;
        }
    }
}
