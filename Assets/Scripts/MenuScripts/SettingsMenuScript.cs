using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown resDropdown;

    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        /*foreach (Resolution resolution in resolutions) {
            string option = resolution.width + " x " + resolution.height;
            options.Add(option);

            if (resolution == Screen.currentResolution)
            {
                currentResolutionIndex = i;
            }
        }*/

        resDropdown.AddOptions(options);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume) {
        Debug.Log(volume);
        //audioMixer.SetFloat("MainMixerVolume", volume);
        audioMixer.SetFloat("MainMixerVolume", Mathf.Log10(volume) * 20);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen) {
        Debug.Log("Fullscreen toggled to " + isFullscreen);
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        Debug.Log("Resolution set to " + resolution.width + " x " + resolution.height);
    }
}
