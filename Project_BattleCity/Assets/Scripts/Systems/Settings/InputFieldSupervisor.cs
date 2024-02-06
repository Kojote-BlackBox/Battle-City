using UnityEngine;
using System.Collections.Generic;

public class InputFieldSupervisor : MonoBehaviour {
    private List<InputMenuControl> inputFields;

    void Awake() {
        inputFields = new List<InputMenuControl>(GetComponentsInChildren<InputMenuControl>());
    }

    public void UpdateKeyAssignment(string newKey, InputMenuControl requestingInputField) {
        // Entferne die Zuweisung der neuen Taste von anderen Eingabefeldern
        foreach (var inputField in inputFields) {
            if (inputField != requestingInputField && inputField.GetCurrentKey() == newKey) {
                inputField.ClearKey();
            }
        }
    }
}
