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
    //control scheme. dictionary?

    //toggle: resume combat mode after placing tower
}
