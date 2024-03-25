using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class with info to write to disk.
//Convert class to Json, then save with PlayerPrefs. Load with PlayerPrefs and un-convert with Json
//Loads on main menu and every subsequent scene
[Serializable]
public class PlayerSettings
{
    Player p = new Player();
    //VOLUME
    //set up the conversion between log stuff -80 - 0  to  0 - 100
    //master volume
    public float MasterVolume = 0f;
    //music volume
    public float MusicVolume = 0f;
    //sfx volume
    public float SFXVolume = 0f;

    //GRAPHICS
    //particle/VFX level
    //resolution
    //(overall?) quality
    //antialiasing
    //shadows

    //KEYBINDINGS
    [Serializable]
    public class KeyBinding
    {
        public KeyCode jumpKey = KeyCode.Space; // Default value
        public KeyCode interactKey = KeyCode.E; // Default value
        /*public KeyCode nextWeaponKey = 
        public KeyCode prevWeaponKey;
    //Build Mode
        public KeyCode modeChangeKey;
        public KeyCode[] towerKeys;
        public KeyCode sellKey;
        public KeyCode[] updatePathKeys;*/
    }

    public KeyBinding keyBindings = new KeyBinding();

    // Method to update Player class key bindings
    public void ApplyKeyBindings(Player player)
    {
        player.jumpKey = keyBindings.jumpKey;
        player.interactKey = keyBindings.interactKey;
        // Apply other key bindings here...
    }

    // Method to save settings to disk
    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(this, true);
        PlayerPrefs.SetString("PlayerSettings", json);
        PlayerPrefs.Save();
    }

    // Method to load settings from disk
    public static PlayerSettings LoadSettings()
    {
        if (PlayerPrefs.HasKey("PlayerSettings"))
        {
            string json = PlayerPrefs.GetString("PlayerSettings");
            return JsonUtility.FromJson<PlayerSettings>(json);
        }
        return new PlayerSettings(); // Return default settings if not found
    }

    // Example usage:
    // PlayerSettings settings = PlayerSettings.LoadSettings();
    // settings.ApplyKeyBindings(playerInstance);
    // settings.SaveSettings();
}
    //control scheme. dictionary?

    //toggle: resume combat mode after placing tower

