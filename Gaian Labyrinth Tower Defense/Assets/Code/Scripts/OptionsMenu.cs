using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TMP_Dropdown resolutionDropdown;
    UnityEngine.Resolution[] resolutions;

    public TMP_Dropdown FPSDropdown;

    void Start()
    {
        resolutions = Screen.resolutions;
        PopulateResolutionDropdown();
        SetFPS();
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
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
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

    void Update() {
        
    }
}
