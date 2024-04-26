using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebuffList
{
    public static int length = 8;

    //visually represented and icons above healthbars
    public static int burnID = 0;
    public static int chillID = 1;
    public static int shockID = 2;
    public static int wetID = 3;

    //healthbar changes (vuln cracks healthbar, fort enhances border)
    public static int vulnerabilityID = 4;
    public static int fortifiedID = 5;

    //visually represented
    public static int frostZoneID = 6;
    public static int mudZoneID = 7;
    public static int whirlpoolZoneID = 8;

    //cooldown for was in a bubble recently. bubble visual effect coming off of enemies immune to being bubbled again
    public static int soaped = 9;
}
