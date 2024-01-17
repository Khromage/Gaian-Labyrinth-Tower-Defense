using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int levelNum;

    private bool complete;

    //set of corruption deals

    public Canvas levelPanel;

    //idk if this is the way we'd want to keep track of the zone
    public enum Zone
    {
        Forest,
        Desert,
        Mountains,
        Islands,
        Ocean,
        Eldoria,
        Caverns
    }

    public Level(int levelNum)
    {
        this.levelNum = levelNum;
    }


}
