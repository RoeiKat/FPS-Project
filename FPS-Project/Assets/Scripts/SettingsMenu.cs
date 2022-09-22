using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public MouseLook mouseLook;
    public Slider mouseSensSlider;

    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start()
    {
        mouseSensSlider.value = mouseLook.mouseSens;
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i< resolutions.Length; i ++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void setMouseSens (float mouseSens)
    {
        mouseLook.mouseSens = mouseSens;
    }

    public void setResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void setVolume (float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void setQuality (int gfxQualityIndex)
    {
        QualitySettings.SetQualityLevel(gfxQualityIndex);
    }

    public void setFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
