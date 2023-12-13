using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using static UnityEngine.InputSystem.Controls.AxisControl;
using System;

public class Map : MonoBehaviour {
    [Header("Tile Prefabs")]
    [Space(10)]
    public GameObject tilePrefab;
    public GameObject waterTilePrefab;
    public GameObject enemyPrefab;
    public GameObject campPrefab;

    [Header("Base Settings")]
    [Space(10)]
    [SerializeField]
    public int rows;
    [SerializeField]
    public int cols;
    [SerializeField]
    public int layer;

    [Header("Water Settings")]
    [Space(10)]
    [SerializeField]
    private float waterCoverage;

    public GameObject[,,] map;
    public int enemyCount;

    public List<GameObject> camps = new List<GameObject>();

    private MapBuilder mapBuilder;

    private void Awake() {
        // InitializeMap
        QualitySettings.antiAliasing = 0;
        waterCoverage = 0.05f;
        enemyCount = 0;
        cols = 50;
        rows = 50;
        LayerType[] layerCount = (LayerType[])Enum.GetValues(typeof(LayerType));
        layer = layerCount.Length;
        map = new GameObject[cols, rows, layer];
        mapBuilder = new MapBuilder(this.gameObject);
    }

    void Start() {
        GenerateWorld();
        //GenerateCamps();
        EnemyDies();
    }

    // TODO inprogress
    private void GenerateCamps() {

        //campBuilder.InitializeCamps();
        int campPosX = cols / 2;
        int campPosY = rows / 2;

        // Set the Frendly Camp on the Map
        GameObject camp = Instantiate(campPrefab, transform.position, Quaternion.identity) as GameObject;

        while (map[campPosX, campPosY, 1] != null) {
            campPosX++;
            campPosY++;
        }

        camp.transform.position = new Vector3(campPosX, campPosY, -0.01f);
        camp.GetComponent<Camp>().position = new Vector2(campPosX, campPosY);
        camp.GetComponent<Camp>().setFrendly(true);
        camps.Add(camp);

        // Set Enemy Camp on the Map
        GameObject enemyCamp = Instantiate(campPrefab, transform.position, Quaternion.identity) as GameObject;

        campPosX += 3;
        campPosY += 3;

        enemyCamp.transform.position = new Vector3(campPosX, campPosY, -0.01f);
        enemyCamp.GetComponent<Camp>().position = new Vector2(campPosX, campPosY);
        enemyCamp.GetComponent<Camp>().setFrendly(false);
        camps.Add(enemyCamp);
    }

    private void GenerateWorld() {

        if (MapAtlas.Instance == null) {
            Debug.LogError("Die MapAtlas wurde nicht geladen. Stellen Sie sicher, dass Sie sie zuerst laden, bevor Sie die Welt generieren.");
            return;
        }

        // Default Layer
        mapBuilder.InitializeBaseLayer(Utility.EARTH_TILE);

        // Generate( tileData Eines Spezifischen tiles, float coverage, float coherence )
        mapBuilder.Generate(Utility.GRASS_TILE, 0.2f, 0.2f);
        //mapBuilder.Generate(Utility.WATER_TILE, 0.1f, 0.6f);

        // mapBuilder.Generate(softGround, coverage, coherence);
        // mapBuilder.Generate(swamp, coverage, coherence);
        // mapBuilder.Generate(mud, coverage, coherence);
        // mapBuilder.Generate(ice, coverage, coherence);
        // mapBuilder.Generate(fragileIce, coverage, coherence);
        // mapBuilder.Generate(desert, coverage, coherence);
        // mapBuilder.Generate(asphalt, coverage, coherence);
        // TODO City

        //mapBuilder.BuildToIsland();

        //mapBuilder.GenerateWaterTransitions();
    }

    public void EnemyDies() {
        enemyCount--;

        if (enemyCount < 0) {
            Utility.Victory();
        } else {
            GameObject enemy = (GameObject)Instantiate(enemyPrefab, transform);
            enemy.transform.position = new Vector2(
                    UnityEngine.Random.Range(1.0f, (float)map.GetLength(0) - 1.0f),
                    UnityEngine.Random.Range(1.0f, (float)map.GetLength(1) - 1.0f)
            );
        }
    }

    public GameObject GetTileOnPosition(Vector2 position) {
        return map[(int)position.x, (int)position.y, 0];
    }
}
