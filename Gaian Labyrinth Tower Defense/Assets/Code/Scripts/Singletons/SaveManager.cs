using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
//using UnityEditor.ShaderGraph.Internal;
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

    //using this SO variable and trying to set it like a reference during runtime doesn't really work
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

    //DEFAULTS
    public KeyCode defJumpKey;
    //Combat
    public KeyCode defInteractKey;
    public KeyCode defNextWeaponKey;
    public KeyCode defPrevWeaponKey;
    //Build Mode
    public KeyCode defModeChangeKey;
    public KeyCode defTowerSelectionKey;
    public KeyCode[] defWeaponKeys;
    public KeyCode[] defUpdatePathKeys;


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

        jumpKey = defJumpKey;
        interactKey = defInteractKey;
        // Combat
        nextWeaponKey = defNextWeaponKey;
        prevWeaponKey = defPrevWeaponKey;
        weaponKeys = defWeaponKeys;
        //Build Mode
        modeChangeKey = defModeChangeKey;
        towerSelectionKey = defTowerSelectionKey;
        updatePathKeys = defUpdatePathKeys;
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
        //jumpKey =  LoadoutManager.Instance.jumpKey;
        //interactKey = LoadoutManager.Instance.interactKey;
        //nextWeaponKey = LoadoutManager.Instance.nextWeaponKey;
        //prevWeaponKey = LoadoutManager.Instance.prevWeaponKey;
        //modeChangeKey = LoadoutManager.Instance.modeChangeKey;
       // towerSelectionKey = LoadoutManager.Instance.towerSelectionKey ;
        //weaponKeys = LoadoutManager.Instance.weaponKeys ;
        //updatePathKeys = LoadoutManager.Instance.updatePathKeys;

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
        /*
        LoadoutManager.Instance.jumpKey = jumpKey;
        LoadoutManager.Instance.interactKey = interactKey;
        LoadoutManager.Instance.nextWeaponKey = nextWeaponKey;
        LoadoutManager.Instance.prevWeaponKey = prevWeaponKey;
        LoadoutManager.Instance.modeChangeKey = modeChangeKey;
        LoadoutManager.Instance.towerSelectionKey = towerSelectionKey;
        LoadoutManager.Instance.weaponKeys = weaponKeys;
        LoadoutManager.Instance.updatePathKeys = updatePathKeys;*/




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
        //jumpKey = defaultKeybinds.jumpKey;
        if (jumpKey == KeyCode.None) {
            Debug.Log("setting Jump");
            jumpKey = defJumpKey;
        }
        if(interactKey == KeyCode.None) {
            interactKey = defInteractKey;
        }
        if(nextWeaponKey == KeyCode.None) {
            nextWeaponKey = defNextWeaponKey;
        }
        if(prevWeaponKey == KeyCode.None) {
            prevWeaponKey = defPrevWeaponKey;
        }
        if(modeChangeKey == KeyCode.None){
            modeChangeKey = defModeChangeKey;
        }
        if(towerSelectionKey == KeyCode.None) {
            towerSelectionKey = defTowerSelectionKey;
        }
        
    }

    public void PassDefaultBindings(DefaultKeybinds d)
    {
        defJumpKey = d.jumpKey;
        defInteractKey = d.interactKey;
        // Combat
        defNextWeaponKey = d.nextWeaponKey;
        defPrevWeaponKey = d.prevWeaponKey;
        defWeaponKeys = d.weaponKeys;
        //Build Mode
        defModeChangeKey = d.modeChangeKey;
        defTowerSelectionKey = d.towerSelectionKey;
        defUpdatePathKeys = d.updatePathKeys;

        Debug.Log($"SaveManager has been passed: {d}");
    }

    // Add a new public method to change keybindings
    public void RebindKey(string keyAction, KeyCode newKey)
    {
        Debug.Log("about to rebind  " + keyAction + " into " + newKey);
        switch (keyAction)
        {
            case "Jump":
                jumpKey = newKey;
                break;
            case "Interact":
                interactKey = newKey;
                Debug.Log("set interact key");
                break;
            case "NextWeapon":
                nextWeaponKey = newKey;
                break;
            case "PrevWeapon":
                prevWeaponKey = newKey;
                break;
            case "ModeChange":
                modeChangeKey = newKey;
                break;
            case "TowerSelection":
                towerSelectionKey = newKey;
                break;
            // Add cases for each keybind you want to be rebindable
        }
    }
    private string keyToRebind = null;

    public void SetKeyToRebind(string keyName)
    {
        keyToRebind = keyName;
    }

    public void OnKeySelected(KeyCode newKey)
    {
        //Debug.Log("ONKEYSELECTED  = " + newKey);
        if (newKey == null) return; // No key to rebind, just exit

        RebindKey(keyToRebind, newKey);
        keyToRebind = null; // Reset the key to rebind
    }

    void OnEnable() {
        //Debug.Log("IM ENABLED");
        OptionsMenu.onKeySelected += OnKeySelected;

    }

    void OnDisable() {
        //Debug.Log("IM DISABLED");
        OptionsMenu.onKeySelected -= OnKeySelected;

    }
}
