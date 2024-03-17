using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using Core.Event;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;

    [SerializeField] private GameObject audioOptionView;
    [SerializeField] private GameObject videoOptionView;
    [SerializeField] private GameObject inputOptionView;

    AudioMenuController audioMenuController;
    VideoMenuController videoMenuController;
    private GameEvent escapeEvent;

    private void Awake() {
        escapeEvent = GameEventManager.Instance.EventKeyEscapeOnPush;

        if (escapeEvent == null) {
            Debug.LogError("Escape event not found");
        }
    }

    private void Start()
    {
        audioMenuController = audioOptionView.GetComponent<AudioMenuController>();
        videoMenuController = videoOptionView.GetComponent<VideoMenuController>();
    }

    public void CloseSettings()
    {

        audioOptionView.SetActive(false);
        videoOptionView.SetActive(false);
        inputOptionView.SetActive(false);
    }

    public void OpenSettings()
    {
        startMenuPanel.SetActive(!startMenuPanel.activeSelf);
        settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);
    }

    public void OpenAudioSettings()
    {
        Debug.Log("Einstellungen Audio");
        audioMenuController.ResettSettings();

        audioOptionView.SetActive(!audioOptionView.activeSelf);
        videoOptionView.SetActive(false);
        inputOptionView.SetActive(false);
    }

    public void OpenVideoSettings()
    {
        Debug.Log("Einstellungen Video");
        videoMenuController.ReloadSettings();

        audioOptionView.SetActive(false);
        videoOptionView.SetActive(!videoOptionView.activeSelf);
        inputOptionView.SetActive(false);
    }

    public void OpenInputSettings()
    {
        Debug.Log("Einstellungen Input");
        audioOptionView.SetActive(false);
        videoOptionView.SetActive(false);
        inputOptionView.SetActive(!inputOptionView.activeSelf);
    }

    public void OpenGameSettings()
    {
        Debug.Log("Einstellungen Game");
        audioOptionView.SetActive(false);
        videoOptionView.SetActive(false);
        inputOptionView.SetActive(false);
    }

    public void BackButtonSettings()
    {
        Debug.Log("Einstellungen Back");
        CloseSettings();

        startMenuPanel.SetActive(!startMenuPanel.activeSelf);
        settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);

        // Hier simulieren wir das Drücken der Escape-Taste
        escapeEvent?.Raise();
    }


    public void SaveSettings()
    {
        Debug.Log("Einstellungen �bernehmen");

        // �berpr�fen, ob die Audio-Option ausgew�hlt ist
        if (audioOptionView.activeSelf)
        {
            Debug.Log("Save Audio");
            AudioSettings currentAudioSettings = audioMenuController.GetCurrentAudioSettings();
            SettingsManager.Instance.SaveAudioSettings(currentAudioSettings);
            audioMenuController.ReloadSettings();

            // Video-Option ist ausgew�hlt
        }
        else if (videoOptionView.activeSelf)
        {
            Debug.Log("Save Video");
            GraphicsSettings currentVideoSettings = videoMenuController.GetCurrentVideoSettings();
            SettingsManager.Instance.SaveVideoSettings(currentVideoSettings);
            videoMenuController.ReloadSettings();

        }
        else if (inputOptionView.activeSelf)
        {
            Debug.Log("Save Input");
            // Video-Option ist ausgew�hlt

        }
    }

    public void QuitGame()
    {
        // Beenden des Spiels
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}