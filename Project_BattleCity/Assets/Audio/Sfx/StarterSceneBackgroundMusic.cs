using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StarterSceneBackgroundMusic : MonoBehaviour {

    AudioSource audioSource;
    private InputAction playMusicAction;

    private void ToggleMusic() {
        if (audioSource.isPlaying) {
            audioSource.Pause();
        } else {
            audioSource.Play();
        }
    }
    void OnDestroy() {
        playMusicAction.Disable();
    }

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        // Input Action initialisieren
        playMusicAction = new InputAction(binding: "<Keyboard>/p");
        playMusicAction.performed += _ => ToggleMusic();
        playMusicAction.Enable();
    }
}
