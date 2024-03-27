using System;
using System.Collections.Generic;
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

}
