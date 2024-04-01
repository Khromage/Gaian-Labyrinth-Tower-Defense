using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.ShaderGraph.Internal;
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

    public DefaultKeybinds defaultKeybinds;
    //Movement
    public KeyCode jumpKey;
    //Combat
    public KeyCode interactKey;
    public KeyCode nextWeaponKey;
    public KeyCode prevWeaponKey;
    //Build Mode
    public KeyCode modeChangeKey;
    public KeyCode towerSelectionKey;
    public KeyCode[] weaponKeys;
    public KeyCode[] updatePathKeys;


    private void Start()
    {
    }


    //initializes the data to be saved for a new file.
    //change these #s as we change the total weapons and towers and levels, etc. maybe make some constants for the totals.
    private void InitializeFreshSave()
    {

        BloodOfGaia = 0;
        levelScores = new int[18];

        EquippedTowerIDs = new int[6] { -1, -1, -1, -1, -1, -1 };
        EquippedWeaponIDs = new int[3] { -1, -1, -1 };

        EquippedTechNodes = new bool[27];

        UnlockedTowers = new bool[12];
        UnlockedWeapons = new bool[8];

        lifetimeTowerDamage = new int[12];
        lifetimeTowerPlacement = new int[12];

        jumpKey = defaultKeybinds.jumpKey;
        interactKey = defaultKeybinds.interactKey;
        // Combat
        nextWeaponKey = defaultKeybinds.nextWeaponKey;
        prevWeaponKey = defaultKeybinds.prevWeaponKey;
        weaponKeys = defaultKeybinds.weaponKeys;
        //Build Mode
        modeChangeKey = defaultKeybinds.modeChangeKey;
        towerSelectionKey = defaultKeybinds.towerSelectionKey;
        updatePathKeys = defaultKeybinds.updatePathKeys;
        string jsonData = JsonUtility.ToJson(Instance);
        PlayerPrefs.SetString(saveFileName, jsonData);
        PlayerPrefs.Save();

    }

    //should take string parameter for the save file's name to save to (and use for PlayerPrefs.GetString's parameter)
    public void SaveData()
    {
        Debug.Log("Saving Data...");
        //grab current loadout from LoadoutManager
        EquippedTowerIDs = LoadoutManager.Instance.EquippedTowerIDs;
        EquippedWeaponIDs = LoadoutManager.Instance.EquippedWeaponIDs;
        EquippedTechNodes = LoadoutManager.Instance.EquippedTechNodes;
        jumpKey =  LoadoutManager.Instance.jumpKey;
        interactKey = LoadoutManager.Instance.interactKey;
        nextWeaponKey = LoadoutManager.Instance.nextWeaponKey;
        prevWeaponKey = LoadoutManager.Instance.prevWeaponKey;
        modeChangeKey = LoadoutManager.Instance.modeChangeKey;
        towerSelectionKey = LoadoutManager.Instance.towerSelectionKey ;
        weaponKeys = LoadoutManager.Instance.weaponKeys ;
        updatePathKeys = LoadoutManager.Instance.updatePathKeys;

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
        /*Debug.Log("Json");
        Debug.Log(EquippedTowerIDs);
        Debug.Log(EquippedTowerIDs.Length);*/
        //if didn't load a previous save
        if (EquippedTowerIDs == null)
        {
            InitializeFreshSave();
        }
        

        //pass saved loadout to LoadoutManager
        LoadoutManager.Instance.EquippedTowerIDs = EquippedTowerIDs;
        LoadoutManager.Instance.EquippedWeaponIDs = EquippedWeaponIDs;
        LoadoutManager.Instance.EquippedTechNodes = EquippedTechNodes;
        SetToDefaults();
        LoadoutManager.Instance.jumpKey = jumpKey;
        LoadoutManager.Instance.interactKey = interactKey;
        LoadoutManager.Instance.nextWeaponKey = nextWeaponKey;
        LoadoutManager.Instance.prevWeaponKey = prevWeaponKey;
        LoadoutManager.Instance.modeChangeKey = modeChangeKey;
        LoadoutManager.Instance.towerSelectionKey = towerSelectionKey;
        LoadoutManager.Instance.weaponKeys = weaponKeys;
        LoadoutManager.Instance.updatePathKeys = updatePathKeys;




        //SOME DEBUG.LOG PRINTING
        string printStr = "";
        for (int i = 0; i < 6; i++)
        {
            if (EquippedTowerIDs[i] != -1)
                printStr += EquippedTowerIDs[i] + ", ";
            else
                printStr += "__, ";
        }

        printStr = "";
        int[] printArr = LoadoutManager.Instance.EquippedTowerIDs;
        for (int i = 0; i < 6; i++)
        {
            if (printArr[i] != -1)
                printStr += printArr[i] + ", ";
            else
                printStr += "__, ";
        }


        OnSaveFileLoaded?.Invoke();
    }

    private void SetToDefaults() {
        Debug.Log("setting Defaults");
        if(jumpKey == KeyCode.None) {
            Debug.Log("setting Jump");
            jumpKey = defaultKeybinds.jumpKey;
        }
        if(interactKey == KeyCode.None) {
            interactKey = defaultKeybinds.interactKey;
        }
        if(nextWeaponKey == KeyCode.None) {
            nextWeaponKey = defaultKeybinds.nextWeaponKey;
        }
        if(prevWeaponKey == KeyCode.None) {
            prevWeaponKey = defaultKeybinds.prevWeaponKey;
        }
        if(modeChangeKey == KeyCode.None){
            modeChangeKey = defaultKeybinds.modeChangeKey;
        }
        if(towerSelectionKey == KeyCode.None) {
            towerSelectionKey = defaultKeybinds.towerSelectionKey;
        }
        
    }

}
