using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct VolumeControl {
    public TMP_Text name;
    public Slider slider;
    public TMP_Text text;
    public Toggle toggle;
}

public class AudioMenuController : MonoBehaviour {
    private AudioSettings audioSettings = new AudioSettings();
    private AudioSettings temporaryAudioSettings = new AudioSettings();

    [Header("Volume Controls")]
    public VolumeControl[] volumeControls;
    private SliderValue[] sliderValues;

    private void Start() {
        ReloadSettings();
    }

    public void ReloadSettings() {
        audioSettings = SettingsManager.Instance.LoadAudioSettings();
        sliderValues = new SliderValue[volumeControls.Length]; // Initialisierung des Arrays

        temporaryAudioSettings.MusicVolume = audioSettings.MusicVolume;
        temporaryAudioSettings.SFXVolume = audioSettings.SFXVolume;
        temporaryAudioSettings.VoiceVolume = audioSettings.VoiceVolume;

        // Setze zuerst die Slider-Werte
        for (int i = 0; i < volumeControls.Length; i++) {
            // Entferne alte Listener
            volumeControls[i].slider.onValueChanged.RemoveAllListeners();

            string gameObjectName = volumeControls[i].name.gameObject.name.Trim(); // Zugriff auf den Namen des GameObjects

            if (gameObjectName.Equals("MusicVolume")) {
                volumeControls[i].slider.value = audioSettings.MusicVolume;
                volumeControls[i].slider.onValueChanged.AddListener((value) => OnMusicVolumeChanged(value));
            } else if (gameObjectName.Equals("SFXVolume")) {
                volumeControls[i].slider.value = audioSettings.SFXVolume;
                volumeControls[i].slider.onValueChanged.AddListener((value) => OnSFXVolumeChanged(value));
            } else if (gameObjectName.Equals("VoiceVolume")) {
                volumeControls[i].slider.value = audioSettings.VoiceVolume;
                volumeControls[i].slider.onValueChanged.AddListener((value) => OnVoiceVolumeChanged(value));
            }

            // Aktualisiere die Textanzeige entsprechend
            sliderValues[i] = new SliderValue(volumeControls[i].slider, volumeControls[i].text, volumeControls[i].toggle);
        }
    }

    public void ResettSettings() {
        Debug.Log("Reload Settings");
        
        for (int i = 0; i < volumeControls.Length; i++) {
            string gameObjectName = volumeControls[i].name.gameObject.name.Trim();

            if (gameObjectName.Equals("MusicVolume")) {
                volumeControls[i].slider.value = temporaryAudioSettings.MusicVolume;
            } else if (gameObjectName.Equals("SFXVolume")) {
                volumeControls[i].slider.value = temporaryAudioSettings.SFXVolume;
            } else if (gameObjectName.Equals("VoiceVolume")) {
                volumeControls[i].slider.value = temporaryAudioSettings.VoiceVolume;
            }
        }
        
    }

    private void OnMusicVolumeChanged(float value) {
        audioSettings.MusicVolume = value;
    }

    private void OnSFXVolumeChanged(float value) {
        audioSettings.SFXVolume = value;
    }

    private void OnVoiceVolumeChanged(float value) {
        audioSettings.VoiceVolume = value;
    }

    public AudioSettings GetCurrentAudioSettings() {
        return audioSettings;
    }
}