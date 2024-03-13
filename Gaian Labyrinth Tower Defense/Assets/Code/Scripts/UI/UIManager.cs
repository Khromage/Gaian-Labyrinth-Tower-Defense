using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject TowerUI;
    public GameObject InGameHUD;
    public GameObject CampaignHUD;

    
    // Start is called before the first frame update
    void Start()
    {
        TowerUI.SetActive(false);
    }
    void OnEnable()
    {
        LevelManager.Instance.OnLevelLoaded += SetUIModule;
        TowerBehavior.OnOpenInteractionPanel += enableTowerUI;
    }

    void OnDisable()
    {
        LevelManager.Instance.OnLevelLoaded -= SetUIModule;
        TowerBehavior.OnOpenInteractionPanel -= enableTowerUI;
    }

    private void SetUIModule(LevelInfo level)
    {
        
    }
    
    
    private void ClearUI()
    {
        
    }


    private void enableTowerUI(TowerBehavior tower)
    {
        // Enable UI Elements
        TowerUI.SetActive(true);
        
        // Set Info
        TowerUIManager towerUI = TowerUI.GetComponent<TowerUIManager>();
        towerUI.SetTowerInfo(tower);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
