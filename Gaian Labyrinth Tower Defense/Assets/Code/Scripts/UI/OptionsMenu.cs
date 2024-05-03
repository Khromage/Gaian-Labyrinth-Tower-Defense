using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    UnityEngine.Resolution[] resolutions;
    public TMP_Dropdown FPSDropdown;
    public GameObject DisplayVGroup;
    public GameObject AudioVGroup;
    public GameObject GraphicsVGroup;
    public GameObject ControlsVGroup;
    public Slider AudioSlider;
    private string currentKeyAction;
    public GameObject waitingForKeyPressUI; // Some UI to show that the game is waiting for a key press
   
    public delegate void changeKey(KeyCode keyToChange);
    public static event changeKey onKeySelected; // Delegate to store the callback when a key is selected
    public TMP_Text jumpButtonText;
    public TMP_Text interactButtonText;
    public TMP_Text nextWeaponButtonText;
    public TMP_Text prevWeaponButtonText;
    public TMP_Text modeChangeButtonText;
    public TMP_Text towerSelectionButtonText;
    public TMP_Text waitingText;




    void Start()
    {
        resolutions = Screen.resolutions;
        PopulateResolutionDropdown();
        SetFPS();
        DisplayVGroup.SetActive(false);
        AudioVGroup.SetActive(false);
        GraphicsVGroup.SetActive(false);
        ControlsVGroup.SetActive(false);
        waitingText.gameObject.SetActive(false);
        
        jumpButtonText.text = SaveManager.Instance.jumpKey.ToString();
        interactButtonText.text = SaveManager.Instance.interactKey.ToString();
        nextWeaponButtonText.text = SaveManager.Instance.nextWeaponKey.ToString();
        prevWeaponButtonText.text = SaveManager.Instance.prevWeaponKey.ToString();
        modeChangeButtonText.text = SaveManager.Instance.modeChangeKey.ToString();
        towerSelectionButtonText.text = SaveManager.Instance.towerSelectionKey.ToString();
    }
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
    }

    private void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume)*20);
    }

    public void SetSFXVolume(float svolume) 
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(svolume)*20);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

     public void SetResolution(int resolutionIndex)
    {
        UnityEngine.Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetFPS() {
        
        if(FPSDropdown.value == 0)
        {
            Application.targetFrameRate = -1;
        } else 
            {
                string stringFPS = FPSDropdown.options[FPSDropdown.value].text;
                Application.targetFrameRate = int.Parse(stringFPS);
            }
    }
// Call this method from the button's OnClick event
    public void StartRebindForKey(string keyAction)
    {
        currentKeyAction = keyAction;
        SaveManager sm = FindObjectOfType<SaveManager>();
        Debug.Log(keyAction);
        sm.SetKeyToRebind(keyAction);
        waitingForKeyPressUI.SetActive(true); // Show the UI that we are waiting for key press
        StartCoroutine(WaitForKeyPress());
    }

    private IEnumerator WaitForKeyPress()
    {
        Debug.Log("started coroutine");
        bool keyFound = false;
        KeyCode newKey = KeyCode.None;
        
        //why not just do    while (newKey == KeyCode.None)
        while (!keyFound)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    newKey = keyCode;
                    keyFound = true;
                    //Debug.Log("want to change to " + keyCode);
                    break;
                }
            }
            yield return null; // Wait until next frame
        }

        //Debug.Log("the key is " + newKey);
        
        // Callback to inform the Player class about the new key
        onKeySelected?.Invoke(newKey);
        // Update UI
        jumpButtonText.text = SaveManager.Instance.jumpKey.ToString();
        interactButtonText.text = SaveManager.Instance.interactKey.ToString();
        nextWeaponButtonText.text = SaveManager.Instance.nextWeaponKey.ToString();
        prevWeaponButtonText.text = SaveManager.Instance.prevWeaponKey.ToString();
        modeChangeButtonText.text = SaveManager.Instance.modeChangeKey.ToString();
        towerSelectionButtonText.text = SaveManager.Instance.towerSelectionKey.ToString();
        waitingForKeyPressUI.SetActive(false);
    }


}
