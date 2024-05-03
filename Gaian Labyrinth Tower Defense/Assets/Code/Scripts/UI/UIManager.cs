using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIManager : MonoBehaviour
{
    public delegate void UIManagement();
    public static event UIManagement OnOptionsOpened;
    public static event UIManagement OnOptionsClosed;
    public static event UIManagement OnLevelUILoaded;
    
    
    [SerializeField]

    public GameObject MainMenuModule;
    public GameObject CampaignMenuModule;
    public GameObject LevelModule;

    public GameObject OptionsMenu;
    
    private GameObject currentModule;

    public DefaultKeybinds defaultKeybinds;

    public GameObject endScreen;

    
    // Start is called before the first frame update
    void Start()
    {
        
        // start with main menu being loaded
        SetMainMenuUI();
        
        CampaignMenuModule.SetActive(false);
        LevelModule.SetActive(false);
        //SaveManager.Instance.defaultKeybinds = defaultKeybinds;
        SaveManager.Instance.PassDefaultBindings(defaultKeybinds);
        SaveManager.Instance.saveFileName = "MyProgress";
        SaveManager.Instance.LoadData();
        

    }

    // Update is called once per frame
    void Update()
    {
        GetUserKeyOptions();
    }

    void OnEnable()
    {
        LevelManager.Instance.OnSceneLoaded += SetUIModule;
        Level.OnWinLevel += winCurrLevel;
        Level.OnLoseLevel += loseCurrLevel;
    }

    void OnDisable()
    {
        LevelManager.Instance.OnSceneLoaded -= SetUIModule;
        Level.OnWinLevel -= winCurrLevel;
        Level.OnLoseLevel -= loseCurrLevel;
    }
    
    private void winCurrLevel()
    {
        LevelModule.GetComponent<LevelModule>().EndLevel(true);
    }
    private void loseCurrLevel()
    {
        LevelModule.GetComponent<LevelModule>().EndLevel(false);
    }

    public void GetUserKeyOptions()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // if options menu/module is not open, open it and set menu mode for player
            if(!OptionsMenu.activeInHierarchy)
            {
                OpenOptions();
            }
            // if already open, close and exit menu mode for player
            else if(OptionsMenu.activeInHierarchy)
            {
                CloseOptions();
            }
        }
    }
    

    public void ReturnToCampaignMenu()
    {
        LevelModule.GetComponent<LevelModule>().resetLevel();
        endScreen.SetActive(false);
        LevelManager.Instance.LoadCampaign();
        CloseOptions();
    }
    
    public void OpenOptions()
    {
        Debug.Log("Options Clicked");
        if(LevelModule.activeInHierarchy || CampaignMenuModule.activeInHierarchy)
        {
            PauseGame();
        }
        OptionsMenu.SetActive(true);
        OnOptionsOpened?.Invoke();

    }
    public void CloseOptions()
    {
        OptionsMenu.SetActive(false);
        OnOptionsClosed?.Invoke();
        SaveManager.Instance.SaveData();

        if(LevelModule.activeInHierarchy || CampaignMenuModule.activeInHierarchy)
            {
                ResumeGame();
            }
        
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }

    private void SetUIModule(int ID)
    {
        switch (ID)
        {
            case 0:
                SetMainMenuUI();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case 1:
                SetCampaignUI();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            case 2:
                SetLevelUI();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            default:
                Debug.Log("just cry");
                break;
        }
    }


    private void SetMainMenuUI()
    {
        ClearUI();
        //Debug.Log("UI CLEARED");
        MainMenuModule.SetActive(true);
        currentModule = MainMenuModule;
        //Debug.Log("Main Menu UI SET");

    }
    private void SetCampaignUI()
    {
        ClearUI();
        //Debug.Log("UI CLEARED");
        CampaignMenuModule.SetActive(true);
        currentModule = CampaignMenuModule;
        //Debug.Log("Campaign UI SET");
    }
    private void SetLevelUI()
    {
        ClearUI();
        // Debug.Log("UI CLEARED");
        LevelModule.SetActive(true);
        currentModule = LevelModule;
        // Debug.Log("LEVEL UI SET");
        OnLevelUILoaded?.Invoke();
    }

    private void ClearUI()
    {
        if (currentModule != null)
        {
            currentModule.SetActive(false);
            //Debug.Log("Module Destroyed");
            currentModule = null;
        }
    }


}
