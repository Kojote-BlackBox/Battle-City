using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class MainMenuController : MonoBehaviour {
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string starterSceneName = "StarterScene";

    [SerializeField] private GameObject startMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;

    [SerializeField] private GameObject audioOptionView;
    [SerializeField] private GameObject videoOptionView;
    [SerializeField] private GameObject inputOptionView;
    [SerializeField] private GameObject gameSettingsOptionView;
    private bool isGameSceneLoaded = false;

    AudioMenuController audioMenuController;
    VideoMenuController videoMenuController;

    private void Start() {
        audioMenuController = audioOptionView.GetComponent<AudioMenuController>();
        videoMenuController = videoOptionView.GetComponent<VideoMenuController>();

        // Starten Sie den asynchronen Ladevorgang im Hintergrund
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        StartCoroutine(LoadGameSceneAsync());
    }

    private IEnumerator LoadGameSceneAsync() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);

        // Verhindert das sofortige Aktivieren der geladenen Szene
        asyncLoad.allowSceneActivation = false;

        // Warten, bis die Szene vollständig geladen ist
        while (!asyncLoad.isDone) {
            // Überprüfen Sie, ob die Szene fast geladen ist (0.9 entspricht 90%)
            if (asyncLoad.progress >= 0.9f && !isGameSceneLoaded) {
                // Aktivieren Sie die geladene Szene
                asyncLoad.allowSceneActivation = true;
                isGameSceneLoaded = true; // Setzen Sie die Flagge auf true, um sicherzustellen, dass dies nur einmal geschieht
            }

            yield return null;
        }
    }

    public void StartGame() {
        if (isGameSceneLoaded) {
            // Hier können Sie den Code für den Spielstart platzieren
            Debug.Log("Spiel wird gestartet...");

            // Entfernen Sie die StarterScene
            SceneManager.UnloadSceneAsync(starterSceneName);
        }
    }

    public void CloseSettings() {
        audioOptionView.SetActive(false);
        videoOptionView.SetActive(false);
        inputOptionView.SetActive(false);
        gameSettingsOptionView.SetActive(false);
    }

    public void OpenSettings() {
        startMenuPanel.SetActive(!startMenuPanel.activeSelf);
        settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);
    }

    public void OpenAudioSettings() {
        Debug.Log("Einstellungen Audio");
        audioMenuController.ResettSettings();

        audioOptionView.SetActive(!audioOptionView.activeSelf);
        videoOptionView.SetActive(false);
        inputOptionView.SetActive(false);
        gameSettingsOptionView.SetActive(false);
    }

    public void OpenVideoSettings() {
        Debug.Log("Einstellungen Video");
        videoMenuController.ReloadSettings();

        audioOptionView.SetActive(false);
        videoOptionView.SetActive(!videoOptionView.activeSelf);
        inputOptionView.SetActive(false);
        gameSettingsOptionView.SetActive(false);
    }

    public void OpenInputSettings() {
        Debug.Log("Einstellungen Input");
        audioOptionView.SetActive(false);
        videoOptionView.SetActive(false);
        inputOptionView.SetActive(!inputOptionView.activeSelf);
        gameSettingsOptionView.SetActive(false);
    }

    public void OpenGameSettings() {
        Debug.Log("Einstellungen Game");
        audioOptionView.SetActive(false);
        videoOptionView.SetActive(false);
        inputOptionView.SetActive(false);
        gameSettingsOptionView.SetActive(!gameSettingsOptionView.activeSelf);
    }

    public void BackButtonSettings() {
        Debug.Log("Einstellungen Back");
        CloseSettings();

        startMenuPanel.SetActive(!startMenuPanel.activeSelf);
        settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);
    }

    public void SaveSettings() {
        Debug.Log("Einstellungen übernehmen");

        // Überprüfen, ob die Audio-Option ausgewählt ist
        if (audioOptionView.activeSelf) {
            Debug.Log("Save Audio");
            AudioSettings currentAudioSettings = audioMenuController.GetCurrentAudioSettings();
            SettingsManager.Instance.SaveAudioSettings(currentAudioSettings);
            audioMenuController.ReloadSettings();

        // Video-Option ist ausgewählt
        } else if (videoOptionView.activeSelf) {
            Debug.Log("Save Video");
            GraphicsSettings currentVideoSettings = videoMenuController.GetCurrentVideoSettings();
            SettingsManager.Instance.SaveVideoSettings(currentVideoSettings);
            videoMenuController.ReloadSettings();

        } else if (inputOptionView.activeSelf) {
            Debug.Log("Save Input");
            // Video-Option ist ausgewählt

        } else if (gameSettingsOptionView.activeSelf) {
            Debug.Log("Save GameSettings");
            // Video-Option ist ausgewählt
        }
    }

    public void QuitGame() {
        // Beenden des Spiels
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}