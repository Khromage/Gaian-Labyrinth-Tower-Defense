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
    private Action<KeyCode> onKeySelected; // Delegate to store the callback when a key is selected
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
        gameObject.SetActive(false);
        DisplayVGroup.SetActive(false);
        AudioVGroup.SetActive(false);
        GraphicsVGroup.SetActive(false);
        ControlsVGroup.SetActive(false);
        waitingText.gameObject.SetActive(false);
        jumpButtonText.text = LoadoutManager.Instance.jumpKey.ToString();
        interactButtonText.text = LoadoutManager.Instance.interactKey.ToString();
        nextWeaponButtonText.text = LoadoutManager.Instance.nextWeaponKey.ToString();
        prevWeaponButtonText.text = LoadoutManager.Instance.prevWeaponKey.ToString();
        modeChangeButtonText.text = LoadoutManager.Instance.modeChangeKey.ToString();
        towerSelectionButtonText.text = LoadoutManager.Instance.towerSelectionKey.ToString();

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
        LoadoutManager lm = FindObjectOfType<LoadoutManager>();
        Debug.Log(keyAction);
        lm.SetKeyToRebind(keyAction);
        waitingForKeyPressUI.SetActive(true); // Show the UI that we are waiting for key press
        StartCoroutine(WaitForKeyPress());
    }

    private IEnumerator WaitForKeyPress()
    {
        Debug.Log("started coroutine");
        bool keyFound = false;
        KeyCode newKey = KeyCode.None;
        
        while (!keyFound)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    newKey = keyCode;
                    keyFound = true;
                    break;
                }
            }
            yield return null; // Wait until next frame
        }

        // Callback to inform the Player class about the new key
        onKeySelected?.Invoke(newKey);
        // Update UI
        jumpButtonText.text = LoadoutManager.Instance.jumpKey.ToString();
        interactButtonText.text = LoadoutManager.Instance.interactKey.ToString();
        nextWeaponButtonText.text = LoadoutManager.Instance.nextWeaponKey.ToString();
        prevWeaponButtonText.text = LoadoutManager.Instance.prevWeaponKey.ToString();
        modeChangeButtonText.text = LoadoutManager.Instance.modeChangeKey.ToString();
        towerSelectionButtonText.text = LoadoutManager.Instance.towerSelectionKey.ToString();
        waitingForKeyPressUI.SetActive(false);
    }

// Use this method to subscribe the Player's method to the delegate
    public void RegisterOnKeySelectedCallback(Action<KeyCode> callback)
    {
        onKeySelected += callback;
    }

// Use this method to unsubscribe when needed
    public void UnregisterOnKeySelectedCallback(Action<KeyCode> callback)
    {
        onKeySelected -= callback;
    }
}
