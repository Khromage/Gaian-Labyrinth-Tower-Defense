using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_CampaignMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //I definitely don't know if this is the system we want to go with. Probably better to have a class instance for each level
    private void OpenPanel(int levelNum)
    {
        
    }

    private void OnEnable()
    {
        LevelMarker.OnLevelInvestigate += OpenPanel;
    }
    private void OnDisable()
    {
        LevelMarker.OnLevelInvestigate -= OpenPanel;
    }
}
