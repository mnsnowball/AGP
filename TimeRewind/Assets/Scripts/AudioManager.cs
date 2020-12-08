using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SoundType{SFX, Music}
[System.Serializable]
public class Sound{
    public string name;
    public AudioManager manager;
    public SoundType type;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float volumeRandomness = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;
    
    [HideInInspector]
    public AudioSource source;
    public bool loop;
    

    public void SetSource(AudioSource _source){
        source = _source;
        source.clip = clip;
    }

    public void Play(){
        //if(source.isPlaying){
            //source.Stop();
        //}
        if (type == SoundType.Music)
        {
            source.volume = volume * (1 + Random.Range(-volumeRandomness/2f, volumeRandomness/2f)) * manager.masterVolume * manager.musicVolume;
            
        } else if (type == SoundType.SFX)
        {
            //Debug.Log(source);
            source.volume = volume * (1 + Random.Range(-volumeRandomness/2f, volumeRandomness/2f)) * manager.masterVolume * manager.sfxVolume;
        }
        source.pitch = pitch * (1 + Random.Range(-randomPitch/2f, randomPitch/2f));
        source.Play();
    }

}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Slider master;
    public Slider sfx;
    public Slider music;
    [Range(0f, 1f)]
    public float masterVolume;
    [Range(0f, 1f)]
    public float sfxVolume;
    [Range(0f, 1f)]
    public float musicVolume;

    [SerializeField]
    public Sound[] sounds;
    [SerializeField]
    public Sound[] dialogueSounds;
    int lastPlayedIndex = 0;
    private void Start() {
        //XMLManager.ins.LoadPrefs();
        instance = this;

        masterVolume = XMLManager.ins.userPrefs.masterVolume;
        sfxVolume = XMLManager.ins.userPrefs.sfxVolume;
        musicVolume = XMLManager.ins.userPrefs.backgroundVolume;

        //master.value = XMLManager.ins.userPrefs.masterVolume;
        //sfx.value = XMLManager.ins.userPrefs.sfxVolume;
        //music.value = XMLManager.ins.userPrefs.backgroundVolume;

        foreach(Sound s in sounds){
            GameObject _go = new GameObject("Sound_" + s.name);
            s.SetSource(_go.AddComponent<AudioSource>());
            //Debug.Log(s.source);
            s.source.clip = s.clip;
            s.manager = this;
            if (s.type == SoundType.SFX)
            {
                s.source.volume = s.volume * sfxVolume * masterVolume;
            } else if (s.type == SoundType.Music)
            {
                s.source.volume = s.volume * musicVolume * masterVolume;
            }
            
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach(Sound s in dialogueSounds){
            GameObject _go = new GameObject("Sound_" + s.name);
            s.SetSource(_go.AddComponent<AudioSource>());
            //Debug.Log(s.source);
            s.source.clip = s.clip;
            s.manager = this;
            if (s.type == SoundType.SFX)
            {
                s.source.volume = s.volume * sfxVolume * masterVolume;
            } else if (s.type == SoundType.Music)
            {
                s.source.volume = s.volume * musicVolume * masterVolume;
            }
            
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            PlaySound("Menu_BG");
        }

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            PlaySound("Background_Ambience");
        }
    }

    public void PlaySound(string _name){
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
           
        Debug.LogWarning("AudioManager: No sound of name " + _name + " was found");     
    }

    public void UpdateVolume(){
        masterVolume = XMLManager.ins.userPrefs.masterVolume;
        sfxVolume = XMLManager.ins.userPrefs.sfxVolume;
        musicVolume = XMLManager.ins.userPrefs.backgroundVolume;
        masterVolume = master.value;
        sfxVolume = sfx.value;
        musicVolume = music.value;

        foreach (Sound s in sounds)
        {
            if (s.source != null && s.type == SoundType.SFX)
            {
                s.source.volume = s.volume * sfxVolume * masterVolume;
            } else if (s.source != null && s.type == SoundType.Music)
            {
                s.source.volume = s.volume * musicVolume * masterVolume;
            }
        }
    }

    public void SaveVolumeSettings(){
        XMLManager.ins.userPrefs.SetVolume(master.value, sfx.value, music.value);
        XMLManager.ins.SavePrefs();
        UpdateVolume();
    }

    public void PlayRandomDialogueAudio(){
        if(!dialogueSounds[lastPlayedIndex].source.isPlaying){
            int playIndex = Random.Range(0, dialogueSounds.Length);
            lastPlayedIndex = playIndex;
            dialogueSounds[playIndex].Play();
        }
        
    }
}
