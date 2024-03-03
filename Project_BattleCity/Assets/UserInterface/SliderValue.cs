using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValue {
    public Slider slider;
    public TMP_Text valueText;
    public Toggle muteToggle;
    private float previousValue; // Speichert den vorherigen Slider-Wert

    public SliderValue(Slider slider, TMP_Text valueText, Toggle muteToggle) {
        this.slider = slider;
        this.valueText = valueText;
        this.muteToggle = muteToggle;
        this.previousValue = slider.value;

        // Setze den maximalen Wert des Sliders
        this.slider.maxValue = 1f;

        // Füge einen Event Listener für Slider-Wertänderungen hinzu
        this.slider.onValueChanged.AddListener(UpdateSliderValue);

        // Initialisiere den Text auf den Startwert des Sliders
        UpdateSliderValue(this.slider.value);

        // Füge Listener für den Mute-Toggle hinzu
        this.muteToggle.onValueChanged.AddListener(ToggleMute);
    }


    // Event Handler, um den Slider-Wert zu aktualisieren
    private void UpdateSliderValue(float value) {
        // Zeige den Slider-Wert im Textfeld an
        valueText.text = "" + Mathf.RoundToInt(value * 100);

        // Aktualisiere den gespeicherten Wert, wenn der Slider manuell geändert wird
        if (value > 0) {
            previousValue = value;
        }

        // Deaktiviere den Mute-Toggle, wenn der Slider manuell erhöht wird
        if (value > 0 && muteToggle.isOn) {
            muteToggle.isOn = false;
        }
    }

    // Event Handler für den Mute-Toggle
    private void ToggleMute(bool isMuted) {
        if (isMuted) {
            // Speichere den aktuellen Wert und setze den Slider auf 0
            previousValue = slider.value;
            slider.value = 0;
        } else {
            // Stelle den vorherigen Wert wieder her
            slider.value = previousValue;
        }
    }
}
