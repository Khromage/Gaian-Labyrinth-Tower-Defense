using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TowerUIManager : MonoBehaviour
{
    public Image towerIcon;
    public TMP_Text towerName;
    public TMP_Text towerValue;
    public TMP_Text towerDescription;
    public UpgradeOption[] upgradeOptions;
    
    public void OnEnable()
    {
    }

    public void setTowerInfo(TowerBehavior selectedTower)
    {
        towerName.text = selectedTower.towerName;
        towerIcon.sprite = selectedTower.towerInfo.Icon;
        towerValue.text = "Value: " + selectedTower.cost;
        towerDescription.text = selectedTower.GetDescription();

        for(int i=0; i<upgradeOptions.Length; i++)
            upgradeOptions[i].Tile.SetActive(false);

        // check tower level and display appropriate # of upgrade options
        if(selectedTower.currentLevel == 1)
        {
            upgradeOptions[1].Tile.SetActive(true);
            upgradeOptions[1].SetOptionInfo(0, selectedTower);
        } 
        else if(selectedTower.currentLevel == 2)
        {
            for(int i=0; i<upgradeOptions.Length; i++)
            {
                upgradeOptions[i].Tile.SetActive(true);
                upgradeOptions[i].SetOptionInfo(i+1, selectedTower);
            }
        }
    }
}

[Serializable]
public struct UpgradeOption
{
    public GameObject Tile;
    public Image Icon; // placeholder
    public TMP_Text Name;
    public TMP_Text Description;
    public TMP_Text Cost;

    public void SetOptionInfo(int branch, TowerBehavior selectedTower)
    {
        Name.text = selectedTower.towerInfo.Branches[branch].Name;
        Cost.text = selectedTower.towerInfo.Branches[branch].Cost.ToString();
        Description.text = selectedTower.towerInfo.Branches[branch].Description;
    }
}