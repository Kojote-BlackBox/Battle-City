using Core.Track;
using UnityEngine;
using TMPro;  // Importieren des TextMeshPro-Namespace

public class LivesDisplay : MonoBehaviour {
    public TextMeshProUGUI livesText;

    public void UpdateLivesDisplay() {
        livesText.text = TrackManager.Instance.playerLives.value + " x " ;
    }
}