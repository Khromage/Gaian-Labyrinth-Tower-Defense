using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ctrl+f SSS for thing to do (saving animation, etc)

//I'm going to stick with SetActive instead of doing prefab instantiation

//can add another Layer to the animator state machine for when Closing > 0 (closing animation in progress),
// to have an invisible sheet cover the panel to disable the buttons

public class UIManager_CMenu : MonoBehaviour
{
    public PlayerInfo savedData;
    private bool changedData = false;

    //need a name for this currency
    public int metaCurrency = 0;
    private string[] activeTowerSet = { "", "", "", "", "", "" };
    private string[] activeWeaponSet = { "", "", ""};

    public GameObject ActivePanel;

    /*
    [SerializeField]
    private GameObject towerListPanel;
    [SerializeField]
    private GameObject weaponListPanel;
    [SerializeField]
    private GameObject encyclopediaListPanel;
    [SerializeField]
    private GameObject levelListPanel;
    [SerializeField]
    private GameObject techListPanel;

    private Dictionary<GameObject, Vector2> panelDictionary = new Dictionary<GameObject, Vector2>();
    */

    [SerializeField] 
    private GameObject activeTowerPanel;

    [SerializeField]
    private GameObject towerInfoPanel;
    private bool towerInfoPanelIsOpen = false;


    // Start is called before the first frame update
    void Start()
    {
        savedData = new PlayerInfo();
        LoadSavedData();

        ActivePanel = null;

        /*
        panelDictionary.Add(towerListPanel, new Vector2(0, -1));
        panelDictionary.Add(towerInfoPanel, new Vector2(-1, 0));
        panelDictionary.Add(weaponListPanel, new Vector2(0, 1));
        panelDictionary.Add(encyclopediaListPanel, new Vector2(1, 0));
        panelDictionary.Add(levelListPanel, new Vector2(-1, 0));
        panelDictionary.Add(techListPanel, new Vector2(0, 1));
        */
    }

    // Update is called once per frame
    void Update()
    {
        /*
        foreach (KeyValuePair<GameObject, Vector2> panelEntry in panelDictionary)
        {
            RectTransform panelTransform = panelEntry.Key.GetComponent<RectTransform>();
            if (panelEntry.Key == ActivePanel)
            {
                //if not already in target position
                //panel for left part of screen
                if (panelEntry.Value.x == -1 && panelTransform.anchoredPosition.x < -Screen.width / 2 + panelTransform.rect.width / 2)
                {
                    panelTransform.anchoredPosition += -1 * panelEntry.Value;
                    //FlyIn(panelEntry);
                }
                //panel for center
                else if (panelTransform.anchoredPosition != new Vector2(0, 0))
                {
                    panelTransform.anchoredPosition += -1 * panelEntry.Value;
                    //FlyIn(panelEntry);
                }
            }
            else
            {
                if (!PanelIsOffscreen(panelEntry, panelTransform.anchoredPosition))
                    FlyOut(panelEntry, panelTransform.anchoredPosition);
            }
        }
        //if ActivePanel == panel && not in target position, move to target position
        //if panel.onScreen && panel.inactive, move it out. if it gets to offscreen position, set panel.onScreen = false;
        */


        if (Input.GetKeyDown(KeyCode.Escape) && ActivePanel != null) 
        {
            ActivePanel.GetComponent<Animator>().SetFloat("Closing", 1f);
        }

    }


    public void SetActivePanel(GameObject panel)
    {
        //ActivePanel is my last clicked panel

        //is it the same panel?
        //is it already closing?
        //
        //is another panel closing already?

        panel.SetActive(true);
        
        //if (ActivePanel != null)
        //{
            //close already-active panel
        if (ActivePanel != null && ActivePanel != panel)
        {
            ActivePanel.GetComponent<Animator>().SetFloat("Closing", 1f);
            if (changedData)
            {
                //SSS begin saving animation
                //SaveData();
                changedData = false;
            }
        }

        //if not closing, start closing, else stop closing.
        if (panel.GetComponent<Animator>().GetFloat("Closing") < .5f)
        {
            Debug.Log($"closing panel: {panel}");
            panel.GetComponent<Animator>().SetFloat("Closing", 1f);
            if (changedData)
            {
                //SSS begin saving animation
                //SaveData();
                changedData = false;
            }
        }
        else
        {
            Debug.Log($"opening panel: {panel}");
            panel.GetComponent<Animator>().SetFloat("Closing", 0f);
            panel.transform.SetAsLastSibling(); //moves to front
        }
        //}

        ActivePanel = panel;

        //this only needs to happen when tower info panel was open, so when the tower list was the active panel
        if (towerInfoPanel.activeSelf)
            CloseTowerInfoPanel();

        /*

        RemoveActivePanel();
        if (panelName != activePanelName) //if we selected a different button. (if pressed same button twice, just closes it)
        {
            activePanelName = panelName;
            switch (panelName)
            {
                case "TowerList":
                    ActivePanel = towerListPanel;
                    break;
                case "WeaponList":
                    ActivePanel = weaponListPanel;
                    break;
                case "LevelList":
                    ActivePanel = levelListPanel;
                    break;
                case "TechTree":
                    ActivePanel = techTreePanel;
                    break;
                case "EncyclopediaList":
                    ActivePanel = encyclopediaListPanel;
                    break;
                default:
                    break;
            }
            OpenActivePanel();
        }
        else
        {
            activePanelName = "";
        }

        */
    }

    /*
    private void OpenActivePanel()
    {
        //instantiate ActivePanel?
        ActivePanel.SetActive(true);
    }

    private void RemoveActivePanel()
    {
        if (ActivePanel != null)
        {
            //Destroy(ActivePanel, 1);
            //activePanelAnimator.SetBool("activated", false);
            //ActivePanel = null;

            //if (ExitCoroutine != null)
                //StopCoroutine(ExitCoroutine);
            //ExitCoroutine = StartCoroutine(ExitAnimCoroutine());

            //disable the interactive bits
            //play animation for removal
            //then set active false
            ActivePanel.SetActive(false);
            ActivePanel = null;
        }
    }
    private void RemoveActivePanel(int lvlNum)
    {
        if (ActivePanel != null)
        {
            //disable the interactive bits
            //play animation for removal
            //then set active false
            //ActivePanel.SetActive(false);
            ActivePanel.GetComponent<Animator>().SetFloat("Closing", 1f);
            ActivePanel = null;
        }
    }

    private IEnumerator ExitAnimCoroutine()
    {
        yield return new WaitForSeconds(1);
    }

    */


    public void DisplayTowerInfo(string type)
    {
        if (!towerInfoPanelIsOpen)
        {
            OpenTowerInfoPanel();
        }

        Debug.Log("Fill out tower info panel here");
        switch (type)
        {
            case "arcane":
                Debug.Log("populate info panel with arcane tower class's description and buttons/images");
                towerInfoPanel.transform.GetChild(0).GetComponent<Image>().color = new Color(.2f, 0f, .8f, .8f);
                break;
            case "fire":
                Debug.Log("populate info panel with fire tower class's description and buttons/images");
                towerInfoPanel.transform.GetChild(0).GetComponent<Image>().color = new Color(.6f, 0f, .4f, .8f);
                break;
            default:
                towerInfoPanel.transform.GetChild(0).GetComponent<Image>().color = new Color(.1f, .1f, .8f, .8f);
                break;
        }
        //switch case "fire" "arcane" "ice" etc
        //populate the 5 buttons on the background image, and title+description+picture/gif
        //each button will change the active picture/gif and description (maybe also make the title a button to give default/general description)

        //close panel if we close the tower selection panel, or if we hit exit button on info panel, or if press the same tower button again?
    }

    //called by the buttons on the tower info panel (the pictures of each tower level)
    public void ChangeTowerInfoDescription(int lvl)
    {
        //using an array of strings belonging to the tower type's class, change the text in the info panel
    }
    private void OpenTowerInfoPanel()
    {
        towerInfoPanel.SetActive(true);
        towerInfoPanelIsOpen = true;
        //play open animation
        towerInfoPanel.GetComponent<Animator>().SetFloat("Closing", 0f);
    }
    public void CloseTowerInfoPanel()
    {
        towerInfoPanelIsOpen = false;
        //play close animation
        towerInfoPanel.GetComponent<Animator>().SetFloat("Closing", 1f);
    }


    /*

    private bool PanelIsOffscreen(KeyValuePair<GameObject, Vector2> panelEntry, Vector2 pos)
    {
        bool isOffscreen = false;

        //left and right
        if (panelEntry.Value.x != 0) 
        {
            float panelWidth = panelEntry.Key.GetComponent<RectTransform>().rect.width;
            //if offscreen
            if (panelEntry.Value.x * pos.x + panelWidth / 2 > Screen.width / 2)
            {
                isOffscreen = true;
            }
        }
        //top and bottom
        else
        {
            float panelHeight = panelEntry.Key.GetComponent<RectTransform>().rect.height;
            //if offscreen
            if (panelEntry.Value.y * pos.y + panelHeight / 2 > Screen.height / 2)
            {
                isOffscreen = true;
            }
        }
        return isOffscreen;
    }

    //x and y source direction (-1 or 1 for left and right, and top and bottom, respectively respectively)
    private void FlyIn(KeyValuePair<GameObject, Vector2> panelEntry)
    {

    }
    //x and y source direction (-1 or 1 for left and right, and top and bottom, respectively respectively)
    private void FlyOut(KeyValuePair<GameObject, Vector2> panelEntry, Vector2 pos)
    {
        pos += 10f * panelEntry.Value;
    }


    */

    public void UpdateActiveTowerSet(string type, int indexOfChange)
    {
        //if any slots already have the type of tower being added, then swap slot of the type with the new slot indexOfChange
        for (int i = 0; i < 6; i++)
        {
            if (activeTowerSet[i] == type && type != "")
            {
                activeTowerSet[i] = activeTowerSet[indexOfChange];
                activeTowerPanel.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(1).GetComponent<Image>().sprite = Tower.GetIcon(activeTowerSet[indexOfChange]);
            }
        }
        activeTowerSet[indexOfChange] = type;
        Debug.Log("new tower of type " + type + ": {" + activeTowerSet[indexOfChange] + "} at " + indexOfChange);

        //if type is null/empty (which will be set up for dragging out of the slot), then set the index's string to empty

        string printStr = "";
        for (int i = 0; i < 6; i++)
        {
            if (activeTowerSet[i] != "")
                printStr += activeTowerSet[i] + ", ";
            else
                printStr += "___, ";
        }
        Debug.Log("tower set: " + printStr);

        savedData.ActiveTowers = activeTowerSet;
        changedData = true;
    }

    public void UpdateActiveWeaponSet(string type, int indexOfChange)
    {

        savedData.ActiveWeapons = activeWeaponSet;
        changedData = true;
    }



    public void LoadSavedData()
    {
        string jsonData = PlayerPrefs.GetString("MyProgress");
        //Convert to Class but don't create new Save Object. Re-use loadedData and overwrite old data in it
        JsonUtility.FromJsonOverwrite(jsonData, savedData);

        activeTowerSet = savedData.ActiveTowers;
        activeWeaponSet = savedData.ActiveWeapons;
        metaCurrency = savedData.metaCurrency;


        //fill in the active tower slots with their respective tower icons...
        for (int i = 0; i < activeTowerSet.Length; i++)
        {
            activeTowerPanel.transform.GetChild(0).GetChild(i).GetChild(0).GetChild(1).GetComponent<Image>().sprite = Tower.GetIcon(activeTowerSet[i]);
        }

        //fill in the active weapon slots with their respective weapon icons...
        for(int i = 0; i < activeWeaponSet.Length; i++)
        {
            //SSS fill in the active weapon slots with their respective weapon icons...
        }
    }
    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(savedData);
        //Save Json string
        PlayerPrefs.SetString("MyProgress", jsonData);
        PlayerPrefs.Save();

        //SSS finish saving animation
    }
     

    private void OnEnable()
    {
        //LevelMarker.OnLevelInvestigate += RemoveActivePanel;
        UIDragDrop.OnActiveTowerChange += UpdateActiveTowerSet;

    }
    private void OnDisable()
    {
        //LevelMarker.OnLevelInvestigate -= RemoveActivePanel;
        UIDragDrop.OnActiveTowerChange -= UpdateActiveTowerSet;

        SaveData();
    }
}