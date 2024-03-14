using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelModule : MonoBehaviour
{
    
    public delegate void LevelUIManagement();
    public static event LevelUIManagement OnMenuOpened;
    public static event LevelUIManagement OnMenuClosed;

    
    public GameObject PlayerHUD;
    public GameObject TowerUI;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerHUD.SetActive(true);
        TowerUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        TowerBehavior.OnOpenInteractionPanel += enableTowerUI;
    }

    void OnDisable()
    {
        TowerBehavior.OnOpenInteractionPanel -= enableTowerUI;
    }

    
    private void enableTowerUI(TowerBehavior tower)
    {
        // Enable UI Elements
        TowerUI.SetActive(true);
        
        // Set Info
        TowerUIManager towerUI = TowerUI.GetComponent<TowerUIManager>();
        towerUI.SetTowerInfo(tower);

        // Send Broadcast to Player to set Menu Mode
        OnMenuOpened?.Invoke();
    }

}
