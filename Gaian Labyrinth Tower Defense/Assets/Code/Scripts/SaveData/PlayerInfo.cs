using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class with info to write to disk.
//Convert class to Json, then save with PlayerPrefs. Load with PlayerPrefs and un-convert with Json
//Loads on selecting save slot from main menu, and on every subsequent scene
[Serializable]
public class PlayerInfo
{
    //maybe switch some of these to lists that populate based on the actual sets (of towers, weapons, tech nodes, levels) we have

    //completed levels by index. completion status (bool) and their scores (int)
    public (bool, int)[] LevelCompletionStatuses = new (bool, int)[18]; //~3 per zone

    //ints for their IDs, corresponding to dictionary? entries for towers and for weapons
    public int[] ActiveTowers = new int[6];
    public int[] ActiveWeapons = new int[2];

    //indexes correspond to IDs. bool for whether the tower or weapon is unlocked
    public bool[] UnlockedTowers = new bool[12];
    public bool[] UnlockedWeapons = new bool[6];

    //Tech tree nodes. indexes corresponding to IDs, bool for whether the node is active
    public bool[] ActiveTechNodes = new bool[10];
}
