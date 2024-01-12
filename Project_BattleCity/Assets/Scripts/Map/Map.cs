using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using static UnityEngine.InputSystem.Controls.AxisControl;
using System;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.InputSystem;

public class Map : MonoBehaviour {
    [Header("Tile Prefabs")]
    [Space(10)]
    public GameObject tilePrefab;
    public GameObject waterTilePrefab;

    [Header("Tank Prefabs")]
    public GameObject enemyPrefab;

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
    private CampBuilder campBuilder;

    [Header("AI Settings")]
    public Overmind overmind;

    //###### AUDIO ########
    private AudioSource audioSource;
    private InputAction playMusicAction;
    //##############


    //##### AUDIO #########
    void OnDestroy() {
        playMusicAction.Disable();
    }

    private void ToggleMusic() {
        if (audioSource.isPlaying) {
            audioSource.Pause();
        } else {
            audioSource.Play();
        }
    }
    //##############


    private void Awake() {
        //####### AUDIO #######
        audioSource = GetComponent<AudioSource>();
        // Input Action initialisieren
        playMusicAction = new InputAction(binding: "<Keyboard>/p");
        playMusicAction.performed += _ => ToggleMusic();
        playMusicAction.Enable();
        //##############

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
        campBuilder = new CampBuilder(this.gameObject);
        overmind = new Overmind(this.gameObject);
    }

    void Start() {
        GenerateWorld();
        GenerateCamps();
        EnemyDies();
    }

    // TODO inprogress
    private void GenerateCamps() {
        campBuilder.Generate();
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
            //TODO
            //Utility.Victory();
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
