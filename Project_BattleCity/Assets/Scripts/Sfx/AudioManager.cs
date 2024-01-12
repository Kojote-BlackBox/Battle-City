using UnityEngine.Audio;
using System;
using UnityEngine;

//Singelten Pattern
public class AudioManager : MonoBehaviour {

    public Sound[] sounds;
    public static AudioManager instance;

    // Lautstärkeeinstellungen für die Kategorien
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private float voiceVolume = 1f;

    private void Awake() {

        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

        } else {
            Destroy(gameObject);
            return;
        }

        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }

        LoadVolumeSettings();
    }

    public void SetMusicVolume(float volume) {
        musicVolume = volume;
        UpdateVolume(SoundType.Music, volume);
    }

    public void SetSFXVolume(float volume) {
        sfxVolume = volume;
        UpdateVolume(SoundType.SFX, volume);
    }

    public void SetVoiceVolume(float volume) {
        voiceVolume = volume;
        UpdateVolume(SoundType.Voice, volume);
    }

    private void UpdateVolume(SoundType type, float volume) {
        foreach (Sound sound in sounds) {
            if (sound.type == type) {
                sound.source.volume = volume;
            }
        }
    }

    public void SaveVolumeSettings() {
        foreach (Sound sound in sounds) {
            PlayerPrefs.SetFloat(sound.name + "_volume", sound.volume);
            PlayerPrefs.SetFloat(sound.name + "_pitch", sound.pitch);
        }
        PlayerPrefs.Save(); // Vergiss nicht, die Änderungen zu speichern!

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("VoiceVolume", voiceVolume);
        PlayerPrefs.Save(); // Speichern der Änderungen
    }

    public void LoadVolumeSettings() {
        if (PlayerPrefs.HasKey("MusicVolume")) {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
        }
        if (PlayerPrefs.HasKey("SFXVolume")) {
            SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume"));
        }
        if (PlayerPrefs.HasKey("VoiceVolume")) {
            SetVoiceVolume(PlayerPrefs.GetFloat("VoiceVolume"));
        }


        foreach (Sound sound in sounds) {
            if (PlayerPrefs.HasKey(sound.name + "_volume")) {
                sound.volume = PlayerPrefs.GetFloat(sound.name + "_volume");
            }
            if (PlayerPrefs.HasKey(sound.name + "_pitch")) {
                sound.pitch = PlayerPrefs.GetFloat(sound.name + "_pitch");
            }
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

        Debug.LogWarning("Test" + sound.name);

        //sound.source.Play();
        sound.source.PlayOneShot(sound.clip, sound.volume);
    }
}