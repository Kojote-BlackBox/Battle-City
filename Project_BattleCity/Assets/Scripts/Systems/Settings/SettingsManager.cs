using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour {
    private static SettingsManager _instance;
    private GameSettings currentSettings; // Variable zum Zwischenspeichern der Einstellungen
    private string settingsPath;

    public static SettingsManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<SettingsManager>();
                if (_instance == null) {
                    GameObject obj = new GameObject("SettingsManager");
                    _instance = obj.AddComponent<SettingsManager>();
                }
            }
            return _instance;
        }
    }

    private void Awake() {

        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        settingsPath = Path.Combine(Application.persistentDataPath, "GameSettings.json");
        if (!File.Exists(settingsPath)) {
            string originalPath = Path.Combine(Application.dataPath, "Scripts/Systems/Settings/GameSettings.json");
            if (File.Exists(originalPath)) {
                File.Copy(originalPath, settingsPath);
                Debug.Log("Settings file copied to: " + settingsPath);
            } else {
                Debug.LogError("Original settings file not found: " + originalPath);
            }
        }
    }

    public void SaveSettings(GameSettings settings) {
        string json = JsonUtility.ToJson(settings, true); // 'true' für eine schön formatierte JSON
        File.WriteAllText(settingsPath, json);
    }

    public void SaveAudioSettings(AudioSettings newAudioSettings) {
        // Lade die aktuellen Einstellungen
        GameSettings currentSettings = LoadSettings();

        // Aktualisiere nur die Audioeinstellungen
        currentSettings.audioSettings = newAudioSettings;

        // Speichere die aktualisierten Einstellungen
        SaveSettings(currentSettings);
    }

    public void SaveVideoSettings(GraphicsSettings newVideoSettings) {
        // Lade die aktuellen Einstellungen
        GameSettings currentSettings = LoadSettings();

        // Aktualisiere nur die Audioeinstellungen
        currentSettings.graphicsSettings = newVideoSettings;

        // Speichere die aktualisierten Einstellungen
        SaveSettings(currentSettings);
    }

    public GameSettings LoadSettings() {
        if(currentSettings != null) {
            return currentSettings;
        }

        return LoadSettingsData();
    }

    public AudioSettings LoadAudioSettings() {
        if (currentSettings == null) {
            LoadSettingsData();
        }

        return currentSettings.audioSettings;
    }

    public GraphicsSettings LoadGraphicsSettings() {
        if (currentSettings == null) {
            LoadSettingsData();
        }

        return currentSettings.graphicsSettings;
    }

    private GameSettings LoadSettingsData() {
        if (File.Exists(settingsPath)) {
            try {
                string json = File.ReadAllText(settingsPath);
                GameSettings settings = JsonUtility.FromJson<GameSettings>(json);

                if (settings == null) {
                    Debug.LogError("Failed to parse the GameSettings JSON.");
                } else {
                    currentSettings = settings;
                    Debug.Log("Successfully loaded GameSettings.");
                }

                return settings;
            } catch (System.Exception ex) {
                Debug.LogError("Error parsing settings file: " + ex.Message);
            }
        } else {
            Debug.LogWarning("Settings file not found at path: " + settingsPath);
        }

        // Erstelle Standard-GameSettings, falls keine Datei vorhanden ist oder ein Fehler auftritt
        currentSettings = new GameSettings {
            audioSettings = new AudioSettings { /* Standardwerte hier einsetzen */ },
            graphicsSettings = new GraphicsSettings { /* ... */ },
            gameplaySettings = new GameplaySettings { /* ... */ }
        };
        return currentSettings;
    }
}