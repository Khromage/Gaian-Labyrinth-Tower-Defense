using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModule : MonoBehaviour
{
    
    public delegate void LevelUIManagement();
    public static event LevelUIManagement OnMenuOpened;
    public static event LevelUIManagement OnMenuClosed;

    
    public GameObject playerHUD;
    public GameObject towerInteractionUI;
    public GameObject towerSelectionWheel;
    
    // Start is called before the first frame update
    void Start()
    {
        playerHUD.SetActive(true);
        towerInteractionUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        Player.OnTowerSelectionOpened += enableTowerSelection;
        Player.OnTowerSelectionClosed += disableTowerSelection;

        TowerBehavior.OnOpenInteractionPanel += enableTowerUI;
        TowerBehavior.OnCloseInteractionPanel += disableTowerUI;
        TowerUIManager.OnExitButtonClicked += disableTowerUI;
        TowerUIManager.OnSellButtonClicked += disableTowerUI;
    }

    void OnDisable()
    {
        Player.OnTowerSelectionOpened -= enableTowerSelection;
        Player.OnTowerSelectionClosed -= disableTowerSelection;

        TowerBehavior.OnOpenInteractionPanel -= enableTowerUI;
        TowerBehavior.OnCloseInteractionPanel -= disableTowerUI;
        TowerUIManager.OnExitButtonClicked -= disableTowerUI;
        TowerUIManager.OnSellButtonClicked -= disableTowerUI;
    }

    
    private void enableTowerUI(TowerBehavior tower)
    {
        // Enable UI Elements
        towerInteractionUI.SetActive(true);
        
        // Set Info
        TowerUIManager towerUI = towerInteractionUI.GetComponent<TowerUIManager>();
        towerUI.SetTowerInfo(tower);

        // Send Broadcast to Player to set Menu Mode
        OnMenuOpened?.Invoke();
    }

    private void disableTowerUI(TowerBehavior tower)
    {
        towerInteractionUI.SetActive(false);
        OnMenuClosed?.Invoke();
        Debug.Log("Closing interaction UI panel for " + tower.name + "tower");
    }

    private void enableTowerSelection()
    {
        // Enable Selection Wheel
        towerSelectionWheel.SetActive(true);

        // Send Broadcast to Player to set Menu Mode
        OnMenuOpened?.Invoke();
    }

    private void disableTowerSelection()
    {
        // Tower Selection Wheel handling hovered slot
        towerSelectionWheel.GetComponent<TowerSelectionWheel>().SlotSelected();
        
        // Disable Selection Wheel
        towerSelectionWheel.SetActive(false);

        // Send Broadcast to Player to exit Menu Mode
        OnMenuClosed?.Invoke();
    }

}
