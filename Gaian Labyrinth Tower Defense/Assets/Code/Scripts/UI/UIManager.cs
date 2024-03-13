using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject[] Modules;
    
    public GameObject TowerUI;
    
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnEnable()
    {
        TowerBehavior.OnOpenInteractionPanel += enableTowerUI;
        LevelManager.Instance.OnLevelLoaded += SetLevelUI;
        LevelManager.Instance.OnCampaignLoaded += SetCampaignUI;

    }

    void OnDisable()
    {
        TowerBehavior.OnOpenInteractionPanel -= enableTowerUI;
    }
    
    private void SetCampaignUI()
    {

    }
    private void SetLevelUI(LevelInfo level)
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
