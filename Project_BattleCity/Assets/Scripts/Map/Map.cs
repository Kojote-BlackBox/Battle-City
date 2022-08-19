using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Map : MonoBehaviour {
    public GameObject tilePrefab;
    public GameObject waterTilePrefab;
    public GameObject enemyPrefab;
    public GameObject campPrefab;

    [SerializeField]
    public int rows;
    [SerializeField]
    public int cols;
    [SerializeField]
    public int layer;

    [SerializeField]
    private float waterCoverage;

    public Sprite[] mapSprites;
    public GameObject[,,] map;
    private Utility.TileType tileType;
    public int enemyCount;

    public List<GameObject> camps = new List<GameObject>();

    private MapBuilder mapBuilder;

    private void Awake() {
        // InitializeMap
        QualitySettings.antiAliasing = 0;
        enemyCount = 0;
        mapSprites = Resources.LoadAll<Sprite>("TileMap/GroundTileset");
        cols = 50;
        rows = 50;
        layer = 2;
        map = new GameObject[cols, rows, layer];
        mapBuilder = new MapBuilder(this.gameObject);
    }

    void Start() {
        //GenerateCamp();
        GenerateWorld();
        EnemyDies();
    }

    // TODO inprogress
    private void GenerateCamp() {
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
        // Default Layer
        mapBuilder.FillMapWithTile(Utility.LAWN);

        // mapBuilder.VariationSet
        // # Utility.TileType groundType
        // # float coverage
        // # float coherence
        MapBuilder.BuildSet ground = new MapBuilder.BuildSet( Utility.TileType.Ground, 0.2f, 0.2f );
        // MapBuilder.BuildSet softGround = new MapBuilder.BuildSet(Utility.TileType.SoftGround, coverage, coherence);
        MapBuilder.BuildSet water = new MapBuilder.BuildSet( Utility.TileType.Water, waterCoverage, 0.6f );
        // MapBuilder.BuildSet swamp = new MapBuilder.BuildSet(Utility.TileType.Swamp, coverage, coherence);
        // MapBuilder.BuildSet mud = new MapBuilder.BuildSet(Utility.TileType.Mud, coverage, coherence);
        // MapBuilder.BuildSet ice = new MapBuilder.BuildSet(Utility.TileType.Ice, coverage, coherence);
        // MapBuilder.BuildSet fragileIce = new MapBuilder.BuildSet(Utility.TileType.FragileIce, coverage, coherence);
        // MapBuilder.BuildSet desert = new MapBuilder.BuildSet(Utility.TileType.Desert, coverage, coherence);
        // MapBuilder.BuildSet asphalt = new MapBuilder.BuildSet( Utility.TileType.Asphalt, coverage, coherence );
        // TODO City

        mapBuilder.Generate(ground);
        mapBuilder.Generate(water);
        mapBuilder.BuildToIsland();

        mapBuilder.GenerateWaterTransitions();
    }

    public void EnemyDies() {
        enemyCount--;

        if (enemyCount < 0) {
            Utility.Victory();
        } else {
            GameObject enemy = (GameObject)Instantiate(enemyPrefab, transform);
            enemy.transform.position = new Vector2(
                    Random.Range(1.0f, (float)map.GetLength(0) - 1.0f),
                    Random.Range(1.0f, (float)map.GetLength(1) - 1.0f)
            );
        }
    }

    public GameObject GetTileOnPosition(Vector2 position) {
        return map[(int)position.x, (int)position.y, 0];
    }
}
