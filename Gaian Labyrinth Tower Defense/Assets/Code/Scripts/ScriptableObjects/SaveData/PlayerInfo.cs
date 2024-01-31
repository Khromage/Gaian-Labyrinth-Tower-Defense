using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class with info to write to disk.
//Convert class to Json, then save with PlayerPrefs. Load with PlayerPrefs and un-convert with Json
//Loads on selecting save slot from main menu, and on every subsequent scene
//PlayerPrefs key is "MyProgress"

public class PlayerInfo
{
    //maybe switch some of these to lists that populate based on the actual sets (of towers, weapons, tech nodes, levels) we have

    private const int towerCount = 12;
    //arcane, fire, ice, water, air, lightning, light, dark, geo,   druid, poison, necro

    private const int weaponCount = 9;
    //arcane wave, arcane spear, scorching ray, firefly, (debuff something), (grenade something), (shotgun something), (trap something), (tower buff something)
    
    private const int levelCount = 18;


    public int metaCurrency = 0;

    //completed levels by index. completion status (bool) and their scores (int)
    public (bool, int)[] LevelCompletionStatuses = new (bool, int)[levelCount]; //~3 per zone
    //^ instead do Dictionary with level names as keys and bools?

    //names for towers
    public string[] ActiveTowers = { "arcane", "", "", "", "", "" };
    //names of weapons
    public string[] ActiveWeapons = { "", "", "" };

    //indexes correspond to IDs. bool for whether the tower or weapon is unlocked
    public bool[] UnlockedTowers = new bool[towerCount];
    public bool[] UnlockedWeapons = new bool[weaponCount];

    //index corresponding to IDs
    public int[] lifetimeTowerDamage = new int[towerCount];
    public int[] lifetimeTowerPlacement = new int[towerCount];

    //last completed wave for in-progress levels, to resume saved in-progress levels
    //int[] inProgressLastWave = new int[levelCount];

    //Tech tree nodes. indexes corresponding to IDs, bool for whether the node is active
    public bool[] ActiveTechNodes = new bool[10];
}
