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
    public int contrast;

    //gameplay settings
    public bool invertCameraX;
    public bool invertCameraY;

    // default settings defined here
    public UserPrefs()
    {    
        masterVolume = 0.7f;
        sfxVolume = 0.7f;
        backgroundVolume = 0.7f;

        //display settings
        windowedMode = false;
        contrast = 50;


        //gameplay settings
        invertCameraX = false;
        invertCameraY = false;
    }

    public void SetVolume(float master, float sfx, float music)
    {
        masterVolume = master;
        sfxVolume = sfx;
        backgroundVolume = music;
    }
}
