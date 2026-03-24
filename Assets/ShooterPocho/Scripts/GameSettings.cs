using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameSettings : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private TMP_Dropdown resolutionDropdown;
    [SerializeField]
    private TMP_Dropdown qualityDropdown;
    [SerializeField]
    private TMP_Dropdown FPSDropdown;
    [SerializeField]
    private Toggle fullScreenToggle;
    [SerializeField]
    private Slider musicSlider, sfxSlider;
    private DataSettings dataSettings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadSettings();
        SetUIElements();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("SettingsData") == true)
        { 
        string data = PlayerPrefs.GetString("SettingsData");
            dataSettings = JsonUtility.FromJson<DataSettings>(data);
        }
        else
        {
            dataSettings = new DataSettings();
            SetDefaultSettings();
        }
    }

    void SetDefaultSettings()
    {
        dataSettings.musicVolume = 1f;
        dataSettings.sfxVolume = 1f;
        dataSettings.fullScreen = true;
        dataSettings.quality = 1;
        dataSettings.FPS = 1; //60
        Resolution[] resolutions = Screen.resolutions;
        for( int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                dataSettings.resolution = i;
                break;
            }
        }


    }

    private void SaveSettings()
    {
        string data = JsonUtility.ToJson(dataSettings);
        PlayerPrefs.SetString("SettingsData", data);
    }

    private void SetUIElements()
    {
        //Sliders
        musicSlider.value = dataSettings.musicVolume;
        sfxSlider.value = dataSettings.sfxVolume;
        //Toggle Fullscreen
        fullScreenToggle.isOn = dataSettings.fullScreen;
        //DropdownsFPS
        FPSDropdown.value = dataSettings.FPS;
        //DropdownsResolution
        resolutionDropdown.ClearOptions();
        Resolution[] optionsResolutions = Screen.resolutions;
        for(int i = 0; i < optionsResolutions.Length; i++)
        {
            string option = optionsResolutions[i].width.ToString() + "x" + optionsResolutions[i].height.ToString();
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(option);
            resolutionDropdown.options.Add(optionData);
        }
        resolutionDropdown.value = dataSettings.resolution;
        //DropdownsQuality
        qualityDropdown.ClearOptions();
        List <TMP_Dropdown.OptionData> optionsQuality = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < QualitySettings.names.Length; i++)
        {
            optionsQuality.Add(new TMP_Dropdown.OptionData(QualitySettings.names[i]));
        }
        qualityDropdown.AddOptions(optionsQuality);
        qualityDropdown.value = dataSettings.quality;
    }
    public void ApplySettings()
    {
        //Music Volume
        dataSettings.musicVolume = musicSlider.value;
        AudioManager.instance.SetMusicVolume(dataSettings.musicVolume);
        //SFX Volume
        dataSettings.sfxVolume = sfxSlider.value;
        AudioManager.instance.SetSFXVolume(dataSettings.sfxVolume);
        //Fullscreen
        dataSettings.fullScreen = fullScreenToggle.isOn;
        Screen.fullScreen = dataSettings.fullScreen;
        //FPS
        dataSettings.FPS = FPSDropdown.value;
        switch (dataSettings.FPS)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;
            case 1:
                Application.targetFrameRate = 60;
                break;
            case 2:
                Application.targetFrameRate = 120;
                break;
            case 3:
                Application.targetFrameRate = -1;
                break;
        }
        //Quality
        dataSettings.quality = qualityDropdown.value;
        QualitySettings.SetQualityLevel(dataSettings.quality);
        //Resolution
        dataSettings.resolution = resolutionDropdown.value;
        Resolution resolution = Screen.resolutions[dataSettings.resolution];
        Screen.SetResolution(resolution.width, resolution.height, dataSettings.fullScreen);

        SaveSettings();
    }


    public void BackButton()
    { 
    
    }
    
}


public class DataSettings
{
    public int resolution;
    public int quality;
    public int FPS;
    public bool fullScreen;
    public float musicVolume;
    public float sfxVolume;
}