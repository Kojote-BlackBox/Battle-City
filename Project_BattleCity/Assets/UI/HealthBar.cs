using Core.Track;
using Gameplay.Health;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Slider slider;

    private bool isInitialized = false;
    private ComponentHealth targetHealthComponent;

    void Update() {
        if (targetHealthComponent == null) {
            if (TrackManager.Instance.player != null && TrackManager.Instance.player.gameObject != null) {
                GameObject player = TrackManager.Instance.player.gameObject;
                targetHealthComponent = player.GetComponent<ComponentHealth>();

                if (targetHealthComponent == null) return;
            }
        }

        if (!isInitialized) {
            InitializeHealthBar();
        }
    }

    void InitializeHealthBar() {
        if (targetHealthComponent.dataHealth == null) {
            Debug.LogError("Konnte health bar nicht initialisieren.");

            return;
        }

        //targetHealthComponent.onHealthChanged.AddListener(UpdateHealthDisplay);

        slider.maxValue = targetHealthComponent.dataHealth.health;
        slider.value = targetHealthComponent.currentHealth;  

        isInitialized = true;
    }

    private void OnDisable() {
      /*  if (targetHealthComponent != null) {
            targetHealthComponent.onHealthChanged.RemoveListener(UpdateHealthDisplay);
        }*/
    }

    public void UpdateHealthDisplay() {
        slider.maxValue = targetHealthComponent.dataHealth.health;
        slider.value = targetHealthComponent.currentHealth;
    }
}
