using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
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

    private void enableTowerUI(TowerBehavior towerScript)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
