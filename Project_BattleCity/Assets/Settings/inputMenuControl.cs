using UnityEngine;
using TMPro;
using System;

public class InputMenuControl : MonoBehaviour {
    public TMP_InputField inputField; // Referenz auf das Input Field
    public TextMeshProUGUI settingLabel; // Referenz auf das Text-Label für den Namen der Einstellung

    private ControlSettings controlSettings; // ControlSettings-Referenz
    private string controlSettingName; // Name der spezifischen Steuerungseinstellung
    private InputFieldSupervisor supervisor; // Referenz auf den Supervisor

    void Start() {
        supervisor = GetComponentInParent<InputFieldSupervisor>(); // Finde den Supervisor
    }

    // Methode zum Initialisieren des UI-Elements
    public void Initialize(ControlSettings settings, string settingName, string currentValue) {
        controlSettings = settings;
        controlSettingName = settingName;

        if (settingLabel != null) {
            settingLabel.text = settingName;
        } else {
            Debug.LogError("SettingLabel ist nicht zugewiesen im InputMenuControl.");
        }

        if (inputField != null) {
            inputField.text = currentValue;
            inputField.onValueChanged.RemoveAllListeners();
            inputField.onValueChanged.AddListener(HandleInputChanged);
        } else {
            Debug.LogError("InputField ist nicht zugewiesen im InputMenuControl.");
        }
    }

    private void HandleInputChanged(string value) {

        if (value == " ") {
            supervisor?.UpdateKeyAssignment("Space", this);
            inputField.text = "Space";
            inputField.DeactivateInputField();

        } else if (value != "Space") {
            string upperCaseValue = value.ToUpper();

            supervisor?.UpdateKeyAssignment(upperCaseValue, this);
            inputField.text = upperCaseValue;
            inputField.DeactivateInputField();
        }
    }

    public string GetCurrentKey() {
        return inputField.text;
    }

    public void ClearKey() {
        inputField.text = "";
    }
}
