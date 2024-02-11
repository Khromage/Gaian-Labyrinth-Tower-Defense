using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when starting a new save file, these values should all get initially set. probably in LoadData? If the GetString didn't pick anything up?

//only calling LoadData() in campaign menu scene's UIManager_CMenu's Start() function.

public class SaveManager : SpawnableSingleton<SaveManager>
{
    //when starting a new save file, these values should all get initially set.

    public string saveFileName;

    //meta currency (stars? earned from completing levels. something less generic?)

    //completed levels and scores

    public int[] EquippedTowerIDs;
    public int[] EquippedWeaponIDs;

    public bool[] EquippedTechNodeIDs;

    //indexes correspond to IDs. bool for whether the tower or weapon is unlocked
    public bool[] UnlockedTowers;
    public bool[] UnlockedWeapons;

    //index corresponding to IDs
    public int[] lifetimeTowerDamage;
    public int[] lifetimeTowerPlacement;


    private void Start()
    {
        //temporary name
        saveFileName = "MyProgress";

        //default values, in case LoadData doesn't assign any. Unsure whether they would actually remain this way?
        EquippedTowerIDs = new int[6] { -1, -1, -1, -1, -1, -1 };
        EquippedWeaponIDs = new int[3] { -1, -1, -1 };

        LoadData();

        //this needs to get called after the specific save file name has been set, so it loads the correct file, so maybe don't call this in Start()?
        //LoadData();
    }

    //should take string parameter for the save file's name to save to (and use for PlayerPrefs.GetString's parameter)
    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(Instance);
        //Save Json string
        PlayerPrefs.SetString(saveFileName, jsonData);
        PlayerPrefs.Save();
    }
    //should take string parameter for the save file's name to load from (and use for PlayerPrefs.GetString's parameter)
    //Calling this in campaign menu scene's UIManager_CMenu's Start() function.
    public void LoadData()
    {
        //Convert to Class but don't create new Save Object. Re-use loadedData and overwrite old data in it
        string jsonData = PlayerPrefs.GetString(saveFileName);
        JsonUtility.FromJsonOverwrite(jsonData, Instance);

        //don't need to set the variable values, because Instance was overwritten by the above line already?
    }

}
