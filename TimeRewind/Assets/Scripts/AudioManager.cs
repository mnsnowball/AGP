using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[System.Serializable]
public class Sound{


    public string name;
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
        source.volume = volume * (1 + Random.Range(-volumeRandomness/2f, volumeRandomness/2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch/2f, randomPitch/2f));
        source.Play();
    }

}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    public Sound[] sounds;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        } else
        {
            Debug.LogError("More than one AudioManager exists");
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in sounds){
            GameObject _go = new GameObject("Sound_" + s.name);
            s.SetSource(_go.AddComponent<AudioSource>());

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start() {
        
    }

    public void PlaySound(string _name){
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
            }
        }
           
        Debug.LogWarning("AudioManager: No sound of name " + _name + " was found");     
    }
}
