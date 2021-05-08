using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class SettingsManager : MonoBehaviour
{

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;
    public Slider contrastSlider;
    public Toggle windowMode;
    public Toggle invertCamX;
    public Toggle invertCamY;

    Volume postProcessing;
    ColorAdjustments color;
    CinemachineFreeLook clientViewCam;

    private void Start()
    {
        clientViewCam = GameObject.FindObjectOfType<CinemachineFreeLook>();
        postProcessing = GameObject.FindObjectOfType<Volume>();
        if (postProcessing != null)
        {
            postProcessing.profile.TryGet<ColorAdjustments>(out color);
        }
        ReadSettings();
        ApplySettings();
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

    public void UpdateContrast() 
    {
        if (color != null)
        {
            color.contrast.value = contrastSlider.value;
        }
        
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
        
        if (XMLManager.ins != null)
        {
            Debug.Log("Reading saved values to sliders");
            masterVolume.value = XMLManager.ins.userPrefs.masterVolume;
            sfxVolume.value = XMLManager.ins.userPrefs.sfxVolume;
            musicVolume.value = XMLManager.ins.userPrefs.backgroundVolume;
        }
        
    }

    public void ReadSettings() 
    {
        contrastSlider.value = XMLManager.ins.userPrefs.contrast;
        windowMode.isOn = XMLManager.ins.userPrefs.windowedMode;
        invertCamX.isOn = XMLManager.ins.userPrefs.invertCameraX;
        invertCamY.isOn = XMLManager.ins.userPrefs.invertCameraY;
    }

    public void SaveSettings()
    {
        if (XMLManager.ins != null)
        {
            XMLManager.ins.userPrefs.SetVolume(masterVolume.value, sfxVolume.value, musicVolume.value);
            XMLManager.ins.userPrefs.windowedMode = windowMode.isOn;
            XMLManager.ins.userPrefs.invertCameraX = invertCamX.isOn;
            XMLManager.ins.userPrefs.invertCameraY = invertCamY.isOn;
            XMLManager.ins.userPrefs.contrast = contrastSlider.value;
            XMLManager.ins.SavePrefs();
        }
        if (AudioManager.instance != null)
        {
            AudioManager.instance.UpdateVolume(masterVolume.value, musicVolume.value, sfxVolume.value);
        }
        
    }

    public void ApplySettings()
    {
        // TODO: apply camera settings
        if (color != null && XMLManager.ins != null)
        {
            color.contrast.value = XMLManager.ins.userPrefs.contrast;
        }
        else
        {
            Debug.LogWarning("Could not find post processing or XML manager");
        }
        if (clientViewCam != null)
        {
            // apply inversion settings for x and y to client view
            clientViewCam.m_YAxis.m_InvertInput = XMLManager.ins.userPrefs.invertCameraY;
            clientViewCam.m_XAxis.m_InvertInput = XMLManager.ins.userPrefs.invertCameraX;

        }
        else
        {
            Debug.LogWarning("Could not find client view camera");
        }
    }

}
