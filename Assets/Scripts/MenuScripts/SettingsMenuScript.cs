using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;

    public List<Image> audioWaves;

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
        // -80 0 -> -1 0 -> 0 1 -> 0 100
        //float vol = ((volume / 80.0f) + 1.0f) * 100.0f;
        float vol = 5.0f * volume / 4.0f + 100.0f;
        audioWaves[0].enabled = (vol > 0.0f);
        audioWaves[1].enabled = (vol > 100.0f / 3.0f);
        audioWaves[2].enabled = (vol > 2 * 100.0f / 3.0f);
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
