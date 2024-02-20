using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject TowerUI;
    
    // Start is called before the first frame update
    void Start()
    {
        TowerUI.SetActive(false);
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
        towerUI.setTowerInfo(tower);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
