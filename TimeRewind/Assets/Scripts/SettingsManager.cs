using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;

    public Camera mainCam;

    private void Start()
    {
        ReadVolume();
    }

    public void SetInvertCamX(Toggle toSet)
    {
        XMLManager.ins.userPrefs.invertCameraX = toSet.isOn;
    }

    public void SetInvertCamY(Toggle toSet)
    {
        XMLManager.ins.userPrefs.invertCameraY = toSet.isOn;
    }

    public void SetContrast(Slider toSet)
    {
        XMLManager.ins.userPrefs.contrast = (int)toSet.value;
    }

    public void SetWindowedMode(Toggle toSet)
    {
        // Toggle fullscreen
        Screen.fullScreen = toSet.isOn;
        Debug.Log("Setting Windowed Mode: " + toSet.isOn);
    }

    public void UpdateVolume() 
    {
        AudioManager.instance.UpdateVolume(masterVolume.value, musicVolume.value, sfxVolume.value);
    }

    public void ReadVolume()
    {
        masterVolume.value = XMLManager.ins.userPrefs.masterVolume;
        sfxVolume.value = XMLManager.ins.userPrefs.sfxVolume;
        musicVolume.value = XMLManager.ins.userPrefs.backgroundVolume;
    }

    public void SaveSettings()
    {
        XMLManager.ins.userPrefs.SetVolume(masterVolume.value, sfxVolume.value, musicVolume.value);
        XMLManager.ins.SavePrefs();
        AudioManager.instance.UpdateVolume(masterVolume.value, musicVolume.value, sfxVolume.value);
    }

    public void ApplySettings()
    {
        // TODO: apply camera settings
    }

}
