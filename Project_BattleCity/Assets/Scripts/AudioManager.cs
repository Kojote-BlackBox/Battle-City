using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake() {

        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start() {
        Play("Theme");
    }

    public void Play(string name) {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if(sound == null) {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }

        //sound.source.Play();
        sound.source.PlayOneShot(sound.clip, sound.volume);
    }
}