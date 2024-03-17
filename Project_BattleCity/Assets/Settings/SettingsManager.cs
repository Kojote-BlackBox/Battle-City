using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour {
    [SerializeField] private GameObject controlSettingPrefab; // Prefab für Steuerungseinstellungen
    [SerializeField] private Transform controlSettingsParent; // Parent-Objekt für Steuerungseinstellungen

    private static SettingsManager _instance;
    private GameSettings currentSettings; // Variable zum Zwischenspeichern der Einstellungen
    private string settingsPath;

    public static SettingsManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindFirstObjectByType<SettingsManager>();
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
        LoadOrCreateSettings();
    }

    private void Start() {
        if (currentSettings == null) {
            currentSettings = LoadSettings();
        }

        PopulateControlSettings();
    }

    private void PopulateControlSettings() {
        if (currentSettings == null || currentSettings.controlSettings == null) {
            Debug.LogError("CurrentSettings oder controlSettings sind null.");
            return;
        }

        // Debug-Ausgabe zur Überprüfung der Anzahl der Properties
        Debug.Log("Anzahl der Properties in controlSettings: " + currentSettings.controlSettings.GetType().GetProperties().Length);


        if (controlSettingPrefab == null) {
            Debug.LogError("ControlSettingPrefab ist nicht zugewiesen.");
            return;
        }

        if (controlSettingsParent == null) {
            Debug.LogError("ControlSettingsParent ist nicht zugewiesen.");
            return;
        }

        if (currentSettings == null || currentSettings.controlSettings == null) {
            Debug.LogError("CurrentSettings oder controlSettings sind null.");
            return;
        }

        foreach (Transform child in controlSettingsParent) {
            Destroy(child.gameObject);
        }

        foreach (var field in currentSettings.controlSettings.GetType().GetFields()) {
            GameObject itemObj = Instantiate(controlSettingPrefab, controlSettingsParent);
            if (itemObj == null) {
                Debug.LogError("Instanziierung des Prefabs fehlgeschlagen.");
                continue;
            }
            Debug.Log("Prefab instanziiert für: " + field.Name);

            InputMenuControl inputMenuControl = itemObj.GetComponent<InputMenuControl>();
            if (inputMenuControl == null) {
                Debug.LogError("InputMenuControl Komponente nicht gefunden.");
                continue;
            }

            string settingName = field.Name; // Dies ist der Schlüsselname aus der JSON-Datei
            string settingValue = field.GetValue(currentSettings.controlSettings)?.ToString() ?? "";
            inputMenuControl.Initialize(currentSettings.controlSettings, settingName, settingValue);
        }
    }


    private void LoadOrCreateSettings() {
        if (File.Exists(settingsPath)) {
            try {
                string json = File.ReadAllText(settingsPath);
                currentSettings = JsonUtility.FromJson<GameSettings>(json);

                if (currentSettings == null) {
                    Debug.LogError("Failed to parse the GameSettings JSON.");
                    CreateDefaultSettings();
                } else {
                    Debug.Log("Successfully loaded GameSettings.");
                }
            } catch (System.Exception ex) {
                Debug.LogError("Error parsing settings file: " + ex.Message);
                CreateDefaultSettings();
            }
        } else {
            Debug.LogWarning("Settings file not found at path: " + settingsPath);
            CreateDefaultSettings();
        }
    }

    private void CreateDefaultSettings() {
        var audioSettings = new AudioSettings();
        audioSettings.MusicVolume = 0.5f;
        audioSettings.SFXVolume = 0.5f;
        audioSettings.VoiceVolume = 0.5f;

        var graphicSettings = new GraphicsSettings();
        graphicSettings.Fullscreen = true;
        graphicSettings.TextureQuality = "High";
        graphicSettings.Resolution = "1920x1080";

        var controlSettings = new ControlSettings();
        controlSettings.turnLeft = "A";
        controlSettings.turnRight = "D";
        controlSettings.forward = "W";
        controlSettings.backward = "S";
        controlSettings.shoot = "Space";
        controlSettings.turretLeft = "Q";
        controlSettings.turretRight = "E";
        controlSettings.toggleMusic = "P";
        controlSettings.menuPause = "Escape";

        currentSettings = new GameSettings {
            audioSettings = audioSettings,
            graphicsSettings = graphicSettings,
            gameplaySettings = new GameplaySettings { /* ... */ },
            controlSettings = controlSettings // Hier Standardwerte für ControlSettings hinzufügen
        };

        SaveSettings(currentSettings);
    }

    public void SaveSettings(GameSettings settings) {
        string json = JsonUtility.ToJson(settings, true);
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
        if (currentSettings != null) {
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

    public void SaveControlSettings(ControlSettings newControlSettings) {
        GameSettings currentSettings = LoadSettings();
        currentSettings.controlSettings = newControlSettings;
        SaveSettings(currentSettings);
    }

    public ControlSettings LoadControlSettings() {
        if (currentSettings == null) {
            LoadSettingsData();
        }

        return currentSettings.controlSettings;
    }

    public void ResetControlSettingsToDefault() {
        if (currentSettings != null && currentSettings.defaultControlSettings != null) {
            // Kopiere die Werte von defaultControlSettings zu controlSettings
            CopyControlSettings(currentSettings.defaultControlSettings, currentSettings.controlSettings);

            SaveSettings(currentSettings); // Speichere die aktualisierten Einstellungen

            // Aktualisiere das UI entsprechend den neuen Einstellungen
            PopulateControlSettings();
        }
    }

    private void CopyControlSettings(ControlSettings source, ControlSettings destination) {
        var fields = typeof(ControlSettings).GetFields();
        foreach (var field in fields) {
            field.SetValue(destination, field.GetValue(source));
        }
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
            gameplaySettings = new GameplaySettings { /* ... */ },
            controlSettings = new ControlSettings(), // Leere Steuerungseinstellungen
            defaultControlSettings = new ControlSettings() // Hier Standardwerte für ControlSettings einfügen
        };
        return currentSettings;
    }
}