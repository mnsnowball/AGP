using System.Collections;
using System.Collections.Generic; // lets us use lists
using UnityEngine;
using UnityEngine.UI;

using System.Xml;               // basic XML attributes
using System.Xml.Serialization; // access xmlSerializer
using System.IO;                // file management

public class XMLManager : MonoBehaviour
{
    public static XMLManager ins;
    // Start is called before the first frame update
    void Awake()
    {
        ins = this;
        LoadPrefs();
    }

    // list of items
    public UserPrefs userPrefs;

    // save items
    public void SavePrefs() {
        // open a new xml file
        XmlSerializer serializer = new XmlSerializer(typeof(UserPrefs));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/user_prefs.xml", FileMode.Create);
        serializer.Serialize(stream, userPrefs);
        stream.Close();
    }

    // load items
    public void LoadPrefs() {
        XmlSerializer serializer = new XmlSerializer(typeof(UserPrefs));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/user_prefs.xml", FileMode.Open);
        userPrefs = serializer.Deserialize(stream) as UserPrefs;
        stream.Close();
    }

    public void SetCamAssist(Toggle toSet){
        userPrefs.cameraAssist = toSet.isOn;
    }

    public void SetInvertCamX(Toggle toSet){
        userPrefs.invertCameraX = toSet.isOn;
    }

    public void SetInvertCamY(Toggle toSet){
        userPrefs.invertCameraY = toSet.isOn;
    }

    public void SetContrast(Slider toSet){
        userPrefs.contrast = (int)toSet.value;
    }

    public void SetFOV(Slider toSet){
        userPrefs.fov = (int)toSet.value;
    }

}


[System.Serializable]
public class UserPrefs {
    //sound settings
    public float masterVolume;
    public float sfxVolume;
    public float backgroundVolume;

    //display settings
    public bool enabledBackgroundMovement;
    public bool windowedMode;
    public int fov;
    public int contrast;
    public int fontSize;

    //gameplay settings
    public bool cameraAssist;
    public bool invertCameraX;
    public bool invertCameraY;

    
    public UserPrefs(){    // default settings defined here
        masterVolume = 0.7f;
        sfxVolume = 0.7f;
        backgroundVolume = 0.7f;

        //display settings
        enabledBackgroundMovement = true;
        windowedMode = false;
        fov = 70;
        contrast = 50;
        fontSize = 30;


        //gameplay settings
        cameraAssist = false;
        invertCameraX = false;
        invertCameraY = false;
    }

    public void SetVolume(float master, float sfx, float music){
        masterVolume = master;
        sfxVolume = sfx;
        backgroundVolume = music;
    }
}
