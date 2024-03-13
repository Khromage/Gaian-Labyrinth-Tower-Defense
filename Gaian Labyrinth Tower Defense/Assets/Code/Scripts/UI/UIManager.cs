using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIManager : MonoBehaviour
{
    [SerializeField]

    public GameObject MainMenuModule;
    public GameObject CampaignMenuModule;
    public GameObject LevelModule;

    private GameObject SettingsMenu;
    private GameObject currentModule;

    
    // Start is called before the first frame update
    void Start()
    {
        // start with main menu being loaded
        currentModule = Instantiate(MainMenuModule, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        LevelManager.Instance.OnLevelLoaded += SetLevelUI;
        LevelManager.Instance.OnCampaignLoaded += SetCampaignUI;
    }

    void OnDisable()
    {
        LevelManager.Instance.OnLevelLoaded -= SetLevelUI;
        LevelManager.Instance.OnCampaignLoaded -= SetCampaignUI;
    }
    
    private void SetMainMenuUI()
    {
        ClearUI();
        currentModule = Instantiate(MainMenuModule, gameObject.transform);
    }
    private void SetCampaignUI()
    {
        ClearUI();
        currentModule = Instantiate(CampaignMenuModule, gameObject.transform); 
    }
    private void SetLevelUI(LevelInfo level)
    {
        ClearUI();
        currentModule = Instantiate(LevelModule, gameObject.transform);
    }

    private void ClearUI()
    {
        if (currentModule != null)
        {
            Destroy(currentModule);
            currentModule = null;
        }
    }


}
