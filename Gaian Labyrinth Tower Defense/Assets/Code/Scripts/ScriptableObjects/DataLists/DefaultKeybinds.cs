using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GLTD/DefaultKeybinds", fileName = "DefaultKeybinds")]
public class DefaultKeybinds : ScriptableObject
{
    [Header("KeyBinds")]
    //Movement
    public KeyCode jumpKey;
    //Combat
    public KeyCode interactKey;
    public KeyCode nextWeaponKey;
    public KeyCode prevWeaponKey;
    //Build Mode
    public KeyCode modeChangeKey;
    public KeyCode towerSelectionKey;
    public KeyCode[] towerKeys;
    public KeyCode sellKey;
    public KeyCode[] updatePathKeys;
}
