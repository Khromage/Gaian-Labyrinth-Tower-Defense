using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    public Image towerIcon;
    public TMP_Text towerName;
    public TMP_Text towerValue;
    public TMP_Text towerDescription;

    public GameObject[] optionTiles;

    
    
    public void OnEnable()
    {
    }

    public void setTowerInfo(TowerBehavior selectedTower)
    {
        towerName.text = selectedTower.towerName;
        towerIcon.sprite = selectedTower.towerInfo.Icon;
        towerValue.text = "Value: " + selectedTower.cost;
        towerDescription.text = selectedTower.GetDescription();


        for(int i=0; i<optionTiles.Length; i++)
            optionTiles[i].SetActive(false);
        // check tower level and display appropriate # of upgrade options
        if(selectedTower.currentLevel == 1)
        {
            optionTiles[1].SetActive(true);
            
        }
    }
}