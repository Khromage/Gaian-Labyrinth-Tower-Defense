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

    public GameObject OptionsMenu;
    private GameObject currentModule;

    
    // Start is called before the first frame update
    void Start()
    {
        // start with main menu being loaded
        SetMainMenuUI();
        
        CampaignMenuModule.SetActive(false);
        LevelModule.SetActive(false);
        OptionsMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        LevelManager.Instance.OnSceneLoaded += SetUIModule;

    }

    void OnDisable()
    {
        LevelManager.Instance.OnSceneLoaded -= SetUIModule;

    }
    
    private void SetUIModule(int ID)
    {
        switch (ID)
        {
            case 0:
                SetMainMenuUI();
                break;
            case 1:
                SetCampaignUI();
                break;
            case 2:
                SetLevelUI();
                break;
            default:
                Debug.Log("just cry");
                break;
        }
    }


    private void SetMainMenuUI()
    {
        ClearUI();
        Debug.Log("UI CLEARED");
        MainMenuModule.SetActive(true);
        currentModule = MainMenuModule;
        Debug.Log("Main Menu UI SET");

    }
    private void SetCampaignUI()
    {
        ClearUI();
        Debug.Log("UI CLEARED");
        CampaignMenuModule.SetActive(true);
        currentModule = CampaignMenuModule;
        Debug.Log("Campaign UI SET");
    }
    private void SetLevelUI()
    {
        ClearUI();
        Debug.Log("UI CLEARED");
        LevelModule.SetActive(true);
        currentModule = LevelModule;
        Debug.Log("LEVEL UI SET");
    }

    private void ClearUI()
    {
        if (currentModule != null)
        {
            currentModule.SetActive(false);
            Debug.Log("Module Destroyed");
            currentModule = null;
        }
    }


}
