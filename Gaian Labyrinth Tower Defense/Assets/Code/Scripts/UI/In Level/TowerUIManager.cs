using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TowerUIManager : MonoBehaviour
{

    public delegate void TowerUIEvent(TowerBehavior towerScript);
    public static event TowerUIEvent OnExitButtonClicked;
    public static event TowerUIEvent OnSellButtonClicked;

    public TowerBehavior tower;
    public Image towerIcon;
    public TMP_Text towerName;
    public TMP_Text towerValue;
    public TMP_Text towerDescription;
    public UpgradeOption[] upgradeOptions;

    public TMP_Text towerSellValue;
    
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

        towerSellValue.text = "Sell\n" + (int)(tower.totalSpent * tower.sellRatio);

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
            case 3:
                Debug.Log("Level 3 - No Upgrade Available");
                break;
        }
    }

    public void OptionClicked(int option)
    {
        switch (tower.currentLevel)
        {
            case 1:
                if (Player.currency >= tower.towerInfo.Level2.Cost)
                {
                    tower.upgradeTower(0);
                    Debug.Log("Tower upgraded to level 2");
                }
                else
                {
                    Debug.Log("We require more vespene gas.");
                }
                break;
            case 2:
                if (Player.currency >= tower.towerInfo.Branches[option-1].Cost)
                {
                    tower.upgradeTower(option);
                    Debug.Log("Tower upgraded to level 3 - Branch " + option);
                }
                else
                {
                    Debug.Log("We require more vespene gas.");
                }
                break;
        }
    }
    public void SellButtonClicked()
    {
        OnSellButtonClicked?.Invoke(tower);
        tower.sellTower();
    }
    public void ExitButtonClicked()
    {
        OnExitButtonClicked?.Invoke(tower);
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