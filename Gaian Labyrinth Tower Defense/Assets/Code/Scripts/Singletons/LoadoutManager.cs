using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : SpawnableSingleton<LoadoutManager>
{

    public int[] EquippedTowerIDs;
    public int[] EquippedWeaponIDs;

    public bool[] EquippedTechNodes;
    
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
    public int[] GetWeaponLoadout()
    {
        if (EquippedWeaponIDs != null)
        {
            return EquippedWeaponIDs;
        }
        else
        {
            Debug.Log("Attempted to grab Weapon Loadout from LoadoutManager, but no loadout initialized, so grabbing SaveManager's instead.");
            return SaveManager.Instance.EquippedWeaponIDs;
        }
    }
     // Add a new public method to change keybindings
    public void RebindKey(string keyAction, KeyCode newKey)
    {
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
        if (keyToRebind == null) return; // No key to rebind, just exit

        RebindKey(keyToRebind, newKey);
        keyToRebind = null; // Reset the key to rebind
    }

    void OnEnable() {
        OptionsMenu optionsMenu = FindObjectOfType<OptionsMenu>();
        if (optionsMenu != null)
        {
            optionsMenu.RegisterOnKeySelectedCallback(OnKeySelected);
        }
    }

    void OnDisable() {
        OptionsMenu optionsMenu = FindObjectOfType<OptionsMenu>();
        if (optionsMenu != null)
        {
            optionsMenu.UnregisterOnKeySelectedCallback(OnKeySelected);
        }
    }
}
