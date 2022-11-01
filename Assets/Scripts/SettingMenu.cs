using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixer effectaudioMixer;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    // Start is called before the first frame update
    void Start()
    {
        resolutions=Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i=0; i<resolutions.Length;i++) { string option = resolutions[i] + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width&& resolutions[i].height == Screen.currentResolution.height) 
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetVolume(float volume) 
    {
        audioMixer.SetFloat("Volume", volume);
    } 
    public void SetGeneralVolume(float volume) 
    {
        audioMixer.SetFloat("Volume", volume);
        effectaudioMixer.SetFloat("Volume", volume);
    } 
    public void SetEffectVolume(float volume) 
    {
        effectaudioMixer.SetFloat("Volume", volume);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
