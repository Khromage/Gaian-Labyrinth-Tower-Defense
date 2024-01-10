using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FireTower
{
    public static string name { get; private set; } = "Fire Tower";
    public static Image icon = Resources.Load<Image>("Icons/FireIcon");

    //the actual tower object, with its behavior attached
    public static GameObject prefab = Resources.Load<GameObject>("Towers/FireTower");

    //lifetime stats across all levels. Loaded from save data. Given to save data on finishing a level (or failing)
    public static int lifetimePlaced = 0;
    public static int lifetimeDamage = 0;

    public static Image[] levelImages =
    {
        Resources.Load<Image>("Icons/FireLvl1"),
        Resources.Load<Image>("Icons/FireLvl2"),
        Resources.Load<Image>("Icons/FireLvl31"),
        Resources.Load<Image>("Icons/FireLvl32"),
        Resources.Load<Image>("Icons/FireLvl33")
};
    public static string[] descriptions =
    {
        "Harnesses the anima of flame and inflicts it upon your foes.", //General
        "Launches a ball of fire at nearby enemies, inflicting BURNING status in an area.", //Lvl 1
        "Imparts a stronger kinetic component to the fireball, improving impact and BURN damage.", // Lvl 2
        "!!!\nwhatever we decide on", //Lvl 3-1
        "Rain fire on your enemies!\nSlowly lobs a wide-area explosive at massive range, inflicting heavy damage and BURN.", //Lvl 3-2
        "Grasp the power of the sun!\nA scorching sun follows the target, inflicting increasingly heavy continuous damage in an area." //Lvl 3-3
    };
}
