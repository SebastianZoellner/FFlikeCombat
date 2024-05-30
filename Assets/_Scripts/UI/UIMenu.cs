using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMenu : MonoBehaviour
{
    [SerializeField] GameObject menuScreen;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    private AudioManager audioManager;
    private int currentResolutionIndex;
    private Resolution[] resolutionArray;
    private void Awake()
    {
        resolutionArray = Screen.resolutions;
        SetResolutionDropdown();
    }

    

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        menuScreen.SetActive(false);
    }

    public void OnMenuButtonDown()
    {
        menuScreen.SetActive(!menuScreen.activeInHierarchy);
    }

    public void ChangeMusicVolume(float volume)
    {
        audioManager.AdjustVolume(volume, AudioManager.AudioMixerChannel.Music);
    }

    public void ChangeBackgroundVolume(float volume)
    {
        audioManager.AdjustVolume(volume, AudioManager.AudioMixerChannel.Background);
    }

    public void ChangeSFXVolume(float volume)
    {
        audioManager.AdjustVolume(volume, AudioManager.AudioMixerChannel.SFX);
    }

    public void ReturnToGame()
    {
        menuScreen.SetActive(false);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen( bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutionArray[resolutionIndex].width, resolutionArray[resolutionIndex].height, Screen.fullScreen);
    }

    private void SetResolutionDropdown()
    {
        

        List<string> options = new List<string>();

        for (int i = 0; i < resolutionArray.Length; ++i)
        {
            options.Add(resolutionArray[i].width + " x " + resolutionArray[i].height);

            if (resolutionArray[i].width == Screen.currentResolution.width && resolutionArray[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;

        }

        resolutionDropdown.ClearOptions();

        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

}
