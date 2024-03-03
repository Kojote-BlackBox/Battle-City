using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoMenuController : MonoBehaviour {
    /*
    // https://www.youtube.com/watch?v=YOaYQrN1oYQ
    // https://www.youtube.com/watch?v=qUYpQ8ySkLU
    */

    Resolution[] resolutions;

    public GraphicsSettings temporaryGraphicsSettings = new GraphicsSettings();
    public GraphicsSettings graphicsSettings = new GraphicsSettings();

    public TMP_Dropdown resulutionDropdown;
    public Toggle fullscreenToggle;

    private void Start() {
        ReloadSettings();
    }

    public void ReloadSettings() {
        graphicsSettings = SettingsManager.Instance.LoadGraphicsSettings();

        fullscreenToggle.isOn = graphicsSettings.Fullscreen;

        // Parse die gespeicherte Auflösung
        string[] resolutionParts = graphicsSettings.Resolution.Split('x');
        int savedWidth = int.Parse(resolutionParts[0]);
        int savedHeight = int.Parse(resolutionParts[1]);

        temporaryGraphicsSettings.Resolution = graphicsSettings.Resolution;
        temporaryGraphicsSettings.Fullscreen = graphicsSettings.Fullscreen;
        temporaryGraphicsSettings.TextureQuality = graphicsSettings.TextureQuality;

        resolutions = Screen.resolutions;
        resulutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        int savedResolutionIndex = 0;
        bool saveResolutionFound = false;

        for (int i = 0; i < resolutions.Length; i++) {
            Resolution resolution = resolutions[i];
            string option = resolution.width + " x " + resolution.height;
            options.Add(option);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }

            if (resolution.width == savedWidth && resolution.height == savedHeight) {
                savedResolutionIndex = i;
                saveResolutionFound = true;
            }
        }

        resulutionDropdown.AddOptions(options);

        if (saveResolutionFound) {
            resulutionDropdown.value = savedResolutionIndex;
        } else {
            resulutionDropdown.value = currentResolutionIndex;
        }

        resulutionDropdown.RefreshShownValue();
    }

    public GraphicsSettings GetCurrentVideoSettings() {
        // Aktualisiere die Werte von temporaryGraphicsSettings basierend auf der Benutzereingabe
        Resolution selectedResolution = resolutions[resulutionDropdown.value];
        temporaryGraphicsSettings.Resolution = selectedResolution.width + " x " + selectedResolution.height;
        temporaryGraphicsSettings.Fullscreen = fullscreenToggle.isOn;

        // Rückgabe der aktualisierten Einstellungen
        return temporaryGraphicsSettings;
    }


    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void OnFullscreeneChanged(bool isFullscreen) {
        Screen.fullScreen = fullscreenToggle.isOn;
    }
}

