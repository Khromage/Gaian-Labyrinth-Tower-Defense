using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TowerUIManager : MonoBehaviour
{
    public TowerBehavior tower;
    public Image towerIcon;
    public TMP_Text towerName;
    public TMP_Text towerValue;
    public TMP_Text towerDescription;
    public UpgradeOption[] upgradeOptions;
    
    public void OnEnable()
    {
    }

    public void SetTowerInfo(TowerBehavior selectedTower)
    {
        tower = selectedTower;
        towerName.text = selectedTower.towerName;
        towerIcon.sprite = selectedTower.towerInfo.Icon;
        towerValue.text = "Value: " + selectedTower.cost;
        towerDescription.text = selectedTower.GetDescription();

        for(int i=0; i<upgradeOptions.Length; i++)
            upgradeOptions[i].Tile.SetActive(false);

        // check tower level and display appropriate # of upgrade options
        switch (tower.currentLevel)
        {
            case 1:
                upgradeOptions[1].Tile.SetActive(true);
                upgradeOptions[1].SetOptionInfo(selectedTower);
                break;
            case 2:
                for(int i=0; i<upgradeOptions.Length; i++)
                {
                    upgradeOptions[i].Tile.SetActive(true);
                    upgradeOptions[i].SetOptionInfo(i, selectedTower);
                }
                break;
        }
    }

    public void OptionClicked(GameObject option)
    {
        switch (tower.currentLevel)
        {
            case 1:
                tower.upgradeTower(0);
                Debug.Log("Tower upgraded to level 2");
                break;
            case 2:
                int chosenOption = Array.IndexOf(upgradeOptions, option) + 1;
                tower.upgradeTower(chosenOption);
                Debug.Log("Tower upgraded to level 3 - Branch " + chosenOption);
                break;
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
        Cost.text = "Cost: " + selectedTower.towerInfo.Branches[branch].Cost.ToString();
        Description.text = selectedTower.towerInfo.Branches[branch].Description;
    }

    public void SetOptionInfo(TowerBehavior selectedTower)
    {
        Name.text = selectedTower.towerInfo.Level2.Name;
        Cost.text = "Cost: " + selectedTower.towerInfo.Level2.Cost.ToString();
        Description.text = selectedTower.towerInfo.Level2.Description;
    }
}