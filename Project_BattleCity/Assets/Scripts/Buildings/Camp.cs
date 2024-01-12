using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour {
    public CampData campData;
    private float currentCapTime = 0.0f; // Neue Variable für den Einnahmefortschritt
    private List<CapedEnemy> tankCount = new List<CapedEnemy>();
    private CircleCollider2D circleCollider;
    private Sprite colorSprite;

    [System.Serializable]
    public class CapedEnemy {
        public float capedTime;
        public GameObject tank;
    }

    void Start() {
        colorSprite = Resources.Load<Sprite>("Buildings/Light");

        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = true;

        // Event-Handler hinzufügen
        campData.OnCampCaptured.AddListener(UpdateCampSprite);

        // Initialisierung des Haupt-SpriteRenderer
        var campSpriteRenderer = GetComponent<SpriteRenderer>();
        campSpriteRenderer.sprite = campData.campSprite;

        // Initialisierung des Licht-SpriteRenderer
        var lightGameObject = new GameObject("Light");
        lightGameObject.transform.parent = transform;
        lightGameObject.transform.localPosition = new Vector3(-0.07f, -0.79f, 0);

        var colorSpriteRenderer = lightGameObject.AddComponent<SpriteRenderer>();
        colorSpriteRenderer.sprite = colorSprite;
        colorSpriteRenderer.sortingOrder = (int)LayerType.ObjectOverlay;

        // Weise colorSpriteRenderer zum CampData zu, um nur das Licht-Sprite einzufärben
        campData.colorSpriteRenderer = colorSpriteRenderer;

        UpdateCampSprite();
    }

    void UpdateCampSprite() {
        // Aktualisieren Sie das Farbsprite des Camps
        if (campData.colorSpriteRenderer != null) {
            campData.colorSpriteRenderer.color = campData.IsFriendly ? campData.friendlyColor : campData.enemyColor;
        }
    }

    void Update() {
        if (IsCaped()) {
            UpdateCaptureTime();
        } else {
            // Setze currentCapTime sofort auf 0, wenn das Camp nicht mehr besetzt ist
            if (currentCapTime > 0) {
                currentCapTime = 0;
                UpdateColorSprite(); // Aktualisiere die Farbe sofort
            }
        }

        // Aktualisiere die Farbe, wenn das Camp besetzt ist
        if (IsCaped()) {
            UpdateColorSprite();
        }

        // Prüfe auf vollständige Einnahme
        if (IsCampFullyCaptured()) {
            SwitchCampOwnership();
        }
    }

    void UpdateColorSprite() {
        // Berechne den Lerp-Faktor basierend auf dem Einnahmefortschritt
        float lerpFactor = currentCapTime / campData.maxCapTime;

        // Bestimme die Start- und Endfarbe basierend auf dem aktuellen Zustand und Einnahmeprozess
        Color startColor = campData.IsFriendly ? campData.friendlyColor : campData.enemyColor;
        Color endColor = campData.IsFriendly ? campData.enemyColor : campData.friendlyColor;

        // Lerp die Farbe von der Startfarbe zur Endfarbe basierend auf dem Einnahmefortschritt
        Color currentColor = Color.Lerp(startColor, endColor, lerpFactor);
        campData.colorSpriteRenderer.color = currentColor;
    }


    bool IsCaped() {
        return tankCount.Count > 0;
    }

    void UpdateCaptureTime() {
        foreach (CapedEnemy enemy in tankCount) {
            enemy.capedTime += Time.deltaTime;
            currentCapTime += Time.deltaTime;

            // Begrenze currentCapTime auf maxCapTime
            if (currentCapTime > campData.maxCapTime) {
                currentCapTime = campData.maxCapTime;
            }
        }
    }

    bool IsCampFullyCaptured() {
        return GetTotalCaptureTime() >= campData.maxCapTime;
    }

    float GetTotalCaptureTime() {
        float totalCaptureTime = 0.0f;
        foreach (CapedEnemy enemy in tankCount) {
            totalCaptureTime += enemy.capedTime;
        }
        return totalCaptureTime;
    }

    void SwitchCampOwnership() {
        campData.IsFriendly = !campData.IsFriendly;
        tankCount.Clear();
        Debug.Log("Camp wurde eingenommen und ist jetzt " + (campData.IsFriendly ? "freundlich" : "feindlich"));
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            // Überprüfen, ob das Camp feindlich ist
            if (!campData.IsFriendly) {
                AddTankToCaptureList(collision.gameObject);
                Debug.Log("Spieler betritt feindliches Camp: " + collision.gameObject.name);
            }
        } else if (collision.CompareTag("Enemy")) {
            // Überprüfen, ob das Camp freundlich ist
            if (campData.IsFriendly) {
                AddTankToCaptureList(collision.gameObject);
                Debug.Log("Gegner betritt freundliches Camp: " + collision.gameObject.name);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy")) {
            RemoveTankFromCaptureList(collision.gameObject);
            Debug.Log("Einheit verlässt das Camp: " + collision.gameObject.name);
        }
    }

    void AddTankToCaptureList(GameObject tank) {
        tankCount.Add(new CapedEnemy { tank = tank, capedTime = 0.0f });
    }

    void RemoveTankFromCaptureList(GameObject tank) {
        tankCount.RemoveAll(enemy => enemy.tank == tank);
    }
}