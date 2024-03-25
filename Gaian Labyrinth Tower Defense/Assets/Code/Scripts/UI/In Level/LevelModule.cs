using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModule : MonoBehaviour
{
    
    public delegate void LevelUIManagement();
    public static event LevelUIManagement OnMenuOpened;
    public static event LevelUIManagement OnMenuClosed;

    
    public GameObject PlayerHUD;
    public GameObject TowerInteractionUI;
    public GameObject TowerSelectionWheel;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerHUD.SetActive(true);
        TowerInteractionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        TowerBehavior.OnOpenInteractionPanel += enableTowerUI;
        TowerBehavior.OnCloseInteractionPanel += disableTowerUI;
        TowerUIManager.OnExitButtonClicked += disableTowerUI;
        TowerUIManager.OnSellButtonClicked += disableTowerUI;
    }

    void OnDisable()
    {
        TowerBehavior.OnOpenInteractionPanel -= enableTowerUI;
        TowerBehavior.OnCloseInteractionPanel -= disableTowerUI;
        TowerUIManager.OnExitButtonClicked -= disableTowerUI;
        TowerUIManager.OnSellButtonClicked -= disableTowerUI;
    }

    
    private void enableTowerUI(TowerBehavior tower)
    {
        // Enable UI Elements
        TowerInteractionUI.SetActive(true);
        
        // Set Info
        TowerUIManager towerUI = TowerInteractionUI.GetComponent<TowerUIManager>();
        towerUI.SetTowerInfo(tower);

        // Send Broadcast to Player to set Menu Mode
        OnMenuOpened?.Invoke();
    }

    private void disableTowerUI(TowerBehavior tower)
    {
        TowerInteractionUI.SetActive(false);
        OnMenuClosed?.Invoke();
        Debug.Log("Closing interaction UI panel for " + tower.name + "tower");
    }

}
