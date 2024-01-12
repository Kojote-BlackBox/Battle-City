using System.Collections.Generic;
using UnityEngine;

public class CampBuilder {
    public Map mapScript;

    private Sprite campSprite;

    GameObject buildingsFolder;

    private List<CampData> allCamps = new List<CampData>();

    public CampBuilder(GameObject origin) {
        this.mapScript = origin.GetComponent<Map>();

        // Ressource einmalig laden und vorrätig halten
        campSprite = Resources.Load<Sprite>("Buildings/Camp");
        buildingsFolder = GameObject.Find("Buildings");
    }

    /* TODO  Verteilun
     * - Camps Anzahl entsprechend Map größe und Spielschwirigkeit.
     * - Spieler Camp Randomesiert auf geprüften Grund.
     * - Spieler und Gegner Camp müssen über Landbrücke erreichbar sein.
     * - Spieler Soll im Eigenen Camp Regulär Spawnen.
     * - Camp als Schop nutzbar / Spiel Pausiert.
     * - Camp Heilt und Aufmunitionierung.
     */
    public void Generate() {

        // Erstelle freundschaftliches Camp
        Generate(true, new Vector2(mapScript.cols / 2, mapScript.rows / 2));

        // Erstelle feindliches Camp
        Generate(false, new Vector2(mapScript.cols / 2 + 3, mapScript.rows / 2 + 3));

        // Erstelle feindliches Camp
        Generate(false, new Vector2(mapScript.cols / 2 - 3, mapScript.rows / 2 - 3));
    }

    public void Generate(bool isFriendly, Vector2 position) {
        // Direkt die Camp-Instanz erstellen
        GameObject camp = new GameObject();
        camp.transform.parent = buildingsFolder.transform;
        camp.name = "Camp";

        // Füge SpriteRenderer hinzu und setze das allgemeine Camp-Sprite
        SpriteRenderer spriteRenderer = camp.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = campSprite;
        spriteRenderer.sortingOrder = (int)LayerType.ObjectOverlay;

        // Füge den CircleCollider2D hinzu
        CircleCollider2D circleCollider = camp.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;

        // Erstelle und weise CampData zu
        Camp campScript = camp.AddComponent<Camp>();
        CampData campData = CreateCampData(isFriendly);
        campData.OnCampCaptured.AddListener(CheckGameEndCondition);
        campScript.campData = campData;

        // Setze die Position des Camps
        camp.transform.position = position;

        // Füge das Camp zur Liste hinzu
        allCamps.Add(campData);
    }

    private void CheckGameEndCondition() {
        
        int friendlyCampCount = 0;
        int enemyCampCount = 0;

        // Iteriere durch alle Camps
        foreach (CampData campData in allCamps) {
            if (campData.IsFriendly) {
                friendlyCampCount++;
            } else {
                enemyCampCount++;
            }
        }

        // Überprüfe Siegesbedingung
        if (friendlyCampCount == 0) {
            // Auslösen der Niederlagenbedingung
            Utility.GameOver();
        }

        // Überprüfe Niederlagenbedingung
        if (enemyCampCount == 0) {
            // Auslösen der Siegesbedingung
            Utility.Victory();
        } 
    }

    private CampData CreateCampData(bool isFriendly) {
        CampData campData = ScriptableObject.CreateInstance<CampData>();

        // Setze das allgemeine Camp-Sprite
        campData.campSprite = campSprite;

        // Farben für freundliche und feindliche Camps festlegen (falls erforderlich)
        campData.friendlyColor = Color.green; // oder eine andere Farbe Ihrer Wahl
        campData.enemyColor = Color.red; // oder eine andere Farbe Ihrer Wahl

        // Setze den anfänglichen Zustand des Camps
        campData.IsFriendly = isFriendly;

        // MaxCapTime oder andere Eigenschaften setzen
        campData.maxCapTime = 5.0f;

        return campData;
    }

}
