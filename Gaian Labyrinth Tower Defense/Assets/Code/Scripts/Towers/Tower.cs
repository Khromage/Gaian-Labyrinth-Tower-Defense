using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//for reference
public static class Tower
{
    //arcane, fire, ice, water, air, lightning, light, dark, geo,   druid, poison, necro

    /*
    
    public static string name = "___ Tower";
    public static Image icon = Resources.Load<Image>("Icons/ArcaneIcon");

    //the actual tower object, with its behavior attached
    public static GameObject prefab = Resources.Load<GameObject>("Towers/ArcaneTower");

    //lifetime stats across all levels. Given to save data on finishing a level (or failing)
    public static int lifetimePlaced;
    public static int lifetimeDamage;

    public static Image[] levelImages = new Image[5];
    public static string[] descriptions =
    {
        "General description",
        "Level 1 description",
        "Level 2 description",
        "Level 3-1 description",
        "Level 3-2 description",
        "Level 3-3 description"
    };

    */

    public static Sprite GetIcon(string type)
    {
        Sprite icon = null;
        switch (type)
        {
            case "arcane":
                Debug.Log("getting arcane icon");
                icon = Resources.Load<Sprite>("Icons/ArcaneIcon");
                Debug.Log(icon);
                break;
            case "fire":
                icon = Resources.Load<Sprite>("Icons/FireIcon");
                break;
            case "ice":
                icon = Resources.Load<Sprite>("Icons/IceIcon");
                break;
            case "water":
                icon = Resources.Load<Sprite>("Icons/WaterIcon");
                break;
            case "air":
                icon = Resources.Load<Sprite>("Icons/AirIcon");
                break;
            case "lightning":
                icon = Resources.Load<Sprite>("Icons/LightningIcon");
                break;
            case "light":
                icon = Resources.Load<Sprite>("Icons/LightIcon");
                break;
            case "dark":
                icon = Resources.Load<Sprite>("Icons/DarkIcon");
                break;
            case "geo":
                icon = Resources.Load<Sprite>("Icons/GeoIcon");
                break;
            default:
                icon = Resources.Load<Sprite>("Icons/BlankIcon");
                Debug.Log($"Icon {type} not found");
                Debug.Log("blank icon: " + icon);
                break;
        }
        return icon;
    }
    public static Sprite GetLvlSprite(string type, int lvl)
    {
        Sprite spr = null;
        switch (type)
        {
            case "arcane":
                spr = Resources.Load<Sprite>($"Icons/ArcaneLvl{lvl}");
                break;
            case "fire":
                spr = Resources.Load<Sprite>($"Icons/FireLvl{lvl}");
                break;
            case "ice":
                spr = Resources.Load<Sprite>($"Icons/IceLvl{lvl}");
                break;
            case "water":
                spr = Resources.Load<Sprite>($"Icons/WaterLvl{lvl}");
                break;
            case "air":
                spr = Resources.Load<Sprite>($"Icons/AirLvl{lvl}");
                break;
            case "lightning":
                spr = Resources.Load<Sprite>($"Icons/LightningLvl{lvl}");
                break;
            case "light":
                spr = Resources.Load<Sprite>($"Icons/LightLvl{lvl}");
                break;
            case "dark":
                spr = Resources.Load<Sprite>($"Icons/DarkLvl{lvl}");
                break;
            case "geo":
                spr = Resources.Load<Sprite>($"Icons/GeoLvl{lvl}");
                break;
            default:
                spr = Resources.Load<Sprite>($"Icons/BlankLvl");
                Debug.Log($"Image {type} lvl {lvl} not found");
                break;
        }
        return spr;
    }

    public static GameObject GetPrefab(string type)
    {
        GameObject prefab = null;
        switch (type)
        {
            case "arcane":
                prefab = Resources.Load<GameObject>("Towers/ArcaneTower");
                break;
            case "fire":
                prefab = Resources.Load<GameObject>("Towers/FireTower");
                break;
            case "ice":
                prefab = Resources.Load<GameObject>("Towers/IceTower");
                break;
            case "water":
                prefab = Resources.Load<GameObject>("Towers/WaterTower");
                break;
            case "air":
                prefab = Resources.Load<GameObject>("Towers/AirTower");
                break;
            case "lightning":
                prefab = Resources.Load<GameObject>("Towers/LightningTower");
                break;
            case "light":
                prefab = Resources.Load<GameObject>("Towers/LightTower");
                break;
            case "dark":
                prefab = Resources.Load<GameObject>("Towers/DarkTower");
                break;
            case "geo":
                prefab = Resources.Load<GameObject>("Towers/GeoTower");
                break;
            default:
                //prefab = Resources.Load<GameObject>("Towers/ArcaneTower");
                Debug.Log($"Prefab for {type} not found.");
                break;
        }
        return prefab;
    }
}