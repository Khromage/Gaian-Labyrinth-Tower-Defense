using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when starting a new save file, these values should all get initially set. probably in LoadData? If the GetString didn't pick anything up?

//only calling LoadData() in campaign menu scene's UIManager_CMenu's Start() function.

public class SaveManager : SpawnableSingleton<SaveManager>
{
    public delegate void SaveFileLoaded();
    public event SaveFileLoaded OnSaveFileLoaded;

    //when starting a new save file, these values should all get initially set.

    public string saveFileName;

    //meta currency (stars? earned from completing levels. something less generic?)
    public int BloodOfGaia;

    //completed levels and scores
    public int[] levelScores;

    public int[] EquippedTowerIDs;
    public int[] EquippedWeaponIDs;

    public bool[] EquippedTechNodes;

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

        //this needs to get called after the specific save file name has been set, so it loads the correct file, so maybe don't call this in Start()?
        LoadData();
    }


    //initializes the data to be saved for a new file.
    //change these #s as we change the total weapons and towers and levels, etc. maybe make some constants for the totals.
    private void InitializeFreshSave()
    {
        Debug.Log("Initializing fresh save in SaveManager.");

        BloodOfGaia = 0;
        levelScores = new int[18];

        EquippedTowerIDs = new int[6] { -1, -1, -1, -1, -1, -1 };
        EquippedWeaponIDs = new int[3] { -1, -1, -1 };

        EquippedTechNodes = new bool[27];

        UnlockedTowers = new bool[12];
        UnlockedWeapons = new bool[8];

        lifetimeTowerDamage = new int[12];
        lifetimeTowerPlacement = new int[12];
    }

    //should take string parameter for the save file's name to save to (and use for PlayerPrefs.GetString's parameter)
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        //grab current loadout from LoadoutManager
        EquippedTowerIDs = LoadoutManager.Instance.EquippedTowerIDs;
        EquippedWeaponIDs = LoadoutManager.Instance.EquippedWeaponIDs;
        EquippedTechNodes = LoadoutManager.Instance.EquippedTechNodes;

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

        //if didn't load a previous save
        if (EquippedTowerIDs.Length == 0)
        {
            InitializeFreshSave();
        }

        //pass saved loadout to LoadoutManager
        LoadoutManager.Instance.EquippedTowerIDs = EquippedTowerIDs;
        LoadoutManager.Instance.EquippedWeaponIDs = EquippedWeaponIDs;
        LoadoutManager.Instance.EquippedTechNodes = EquippedTechNodes;


        //SOME DEBUG.LOG PRINTING
        string printStr = "";
        for (int i = 0; i < 6; i++)
        {
            if (EquippedTowerIDs[i] != -1)
                printStr += EquippedTowerIDs[i] + ", ";
            else
                printStr += "__, ";
        }
        //Debug.Log("SaveManager LoadData(),     SaveManager's list: " + printStr);

        printStr = "";
        int[] printArr = LoadoutManager.Instance.EquippedTowerIDs;
        for (int i = 0; i < 6; i++)
        {
            if (printArr[i] != -1)
                printStr += printArr[i] + ", ";
            else
                printStr += "__, ";
        }
       // Debug.Log("SaveManager LoadData(), LoadoutManager's list: " + printStr);


        OnSaveFileLoaded?.Invoke();
    }

}
