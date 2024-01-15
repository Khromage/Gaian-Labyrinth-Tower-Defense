using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ArcaneTower
{
    public static string name = "Arcane Tower";
    public static Image icon = Resources.Load<Image>("Icons/ArcaneIcon");

    //the actual tower object, with its behavior attached
    public static GameObject prefab = Resources.Load<GameObject>("Towers/ArcaneTower");

    //lifetime stats across all levels. Loaded from save data. Given to save data on finishing a level (or failing)
    public static int lifetimePlaced = 0;
    public static int lifetimeDamage = 0;

    public static Image[] levelImages =
    {
        Resources.Load<Image>("Icons/ArcaneLvl1"),
        Resources.Load<Image>("Icons/ArcaneLvl2"),
        Resources.Load<Image>("Icons/ArcaneLvl31"),
        Resources.Load<Image>("Icons/ArcaneLvl32"),
        Resources.Load<Image>("Icons/ArcaneLvl33")
};
    public static string[] descriptions =
    {
        "Condenses Arcane energy into projectiles which track and damage their targets.", //General
        "Fires arcane bolts at a moderate rate, consistently damaging a single enemy at a time.", //Lvl 1
        "Fires heavier arcane bolts at a high rate, consistently damaging a single enemy at a time.", // Lvl 2
        "Give them no rest! No quarter!\nFires arcane bolts at an extremely rapid pace, shredding its target.", //Lvl 3-1
        "Magical explosions! What's not to love!\nLobs an arcane explosive at nearby foes, blasting them in a moderate area.", //Lvl 3-2
        "Tear them apart from the inside!\nEmanates waves of arcane energy, destablising enemies, which slows and damages them." //Lvl 3-3
    };

}
