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
        if (ins != null && ins != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            ins = this;
            DontDestroyOnLoad(this.gameObject);
        }
        LoadPrefs();
        LoadUnlockedLevels();
    }

    // list of items
    public UserPrefs userPrefs;
    public Levels unlockedLevels;

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

    public void SaveUnlockedLevels() 
    {
        Debug.Log("Saving unlocks");
        // open a new xml file
        XmlSerializer serializer = new XmlSerializer(typeof(Levels));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/levels_unlocked.xml", FileMode.Create);
        serializer.Serialize(stream, unlockedLevels);
        stream.Close();
    }

    public void LoadUnlockedLevels()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Levels));
        FileStream stream = new FileStream(Application.dataPath + "/StreamingAssets/XML/levels_unlocked.xml", FileMode.Open);
        unlockedLevels = serializer.Deserialize(stream) as Levels;
        stream.Close();
    }

}

[System.Serializable]
public class Levels
{
    public List<bool> isUnlocked;

    public Levels() 
    {
        isUnlocked = new List<bool>();
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
    public float contrast;

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
        contrast = 30f;


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
