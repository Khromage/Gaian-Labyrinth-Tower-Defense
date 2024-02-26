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



    
    
    public void OnEnable()
    {
        
    }

    public void setTowerInfo(TowerBehavior selectedTower)
    {
        towerName.text = selectedTower.towerName;
        towerIcon.sprite = selectedTower.towerInfo.Icon;
        towerValue.text = "Value: " + selectedTower.cost;
        towerDescription.text = selectedTower.GetDescription();
    }
}