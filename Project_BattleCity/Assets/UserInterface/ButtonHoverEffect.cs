using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private TextMeshProUGUI buttonText;
    private Color normalColor;

    void Start() {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null) {
            normalColor = buttonText.color;
        } else {
            Debug.LogError("TextMeshProUGUI component not found in the button. Make sure there is a TextMeshProUGUI component within the button.", this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        buttonText.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData) {
        buttonText.color = normalColor;
    }

    void OnDisable() {
        // Setze die Farbe zurück, wenn das GameObject deaktiviert wird
        if (buttonText != null) {
            buttonText.color = normalColor;
        }
    }
}
