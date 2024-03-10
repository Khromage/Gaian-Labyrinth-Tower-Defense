using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionBase : ScriptableObject
{
    public string missionName;
    public string description;
    public bool isCompleted = false;

    // Abstract method to check if the mission is completed
    public abstract void CheckMissionCompletion();
}