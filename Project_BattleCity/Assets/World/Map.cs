using UnityEngine;
using System;
using UnityEngine.InputSystem;
using World.Builder;
using Gameplay.Bunker;
using Core.Spawn;
using System.Collections.Generic;
using Core;

namespace World
{
    // TODO: move to data spawninfo or something like that
    [Serializable]
    public class SpawnInfo
    {
        public GameObject prefabObjectToSpawn;
        public int spawnDelay;
    }

    public class Map : MonoBehaviour
    {
        #region map
        public GameObject[,,] map;
        public int rows;
        public int columns;
        [HideInInspector] public int layer;

        private Dictionary<Vector2Int, bool> _gridOccupancy;

        private RectInt _mapBounds;
        #endregion

        #region tiles
        [Header("Tiles")]
        public GameObject tilePrefab;
        public GameObject waterTilePrefab;
        #endregion

        [Header("Water Settings")]
        public float waterCoverage;

        #region builders
        private BuilderMap builderMap;
        private BuilderBunker builderBunker;
        #endregion

        #region audio
        private AudioSource audioSource;
        private InputAction playMusicAction;
        #endregion

        #region bunkers
        [Header("Bunker Settings")]
        public DataBunker dataBunkerFriendly;
        public DataBunker[] dataBunkersEnemy;
        public GameObject parentObjectBunkers;
        #endregion

        #region spawnpoints
        [Header("Spawnpoints Settings")]
        public GameObject prefabSpawnpoint;
        public List<SpawnInfo> spawnPoints;
        #endregion

        private void Awake()
        {
            _mapBounds = new RectInt(
                xMin: 0,
                yMin: 0,
                width: columns,
                height: rows
            );

            initializeAudio();
            initializeMap();
            initializeBuilder();

            _gridOccupancy = new Dictionary<Vector2Int, bool>();
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (i == 0 || i == columns - 1) _gridOccupancy[new Vector2Int(i, j)] = true;
                    else if (j == 0 || j == rows - 1) _gridOccupancy[new Vector2Int(i, j)] = true;
                    else _gridOccupancy[new Vector2Int(i, j)] = false;
                }
            }

            generateWorld();
            generateBunkers();
            generatePickups();
        }

        void initializeAudio()
        {
            audioSource = GetComponent<AudioSource>();

            playMusicAction = new InputAction(binding: "<Keyboard>/p");
            playMusicAction.performed += _ => ToggleMusic();
            playMusicAction.Enable();
        }

        void initializeMap()
        {
            QualitySettings.antiAliasing = 0;

            LayerType[] layerCount = (LayerType[])Enum.GetValues(typeof(LayerType));
            layer = layerCount.Length;

            map = new GameObject[columns, rows, layer];
        }

        void initializeBuilder()
        {
            builderMap = new BuilderMap(this);

            builderBunker = new BuilderBunker
            {
                sceneParentObject = parentObjectBunkers,
                dataBunkerFriendly = dataBunkerFriendly,
                dataBunkersEnemy = dataBunkersEnemy,
            };
        }

        void OnDestroy()
        {
            playMusicAction.Disable();
        }

        private void ToggleMusic()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Play();
            }
        }

        private void generateBunkers()
        {
            if (builderBunker == null) return;

            builderBunker.Generate(this);
        }

        private void generatePickups()
        {
            if (spawnPoints == null || spawnPoints.Count <= 0) return;

            foreach (var spawnPoint in spawnPoints)
            {
                var mapRectangle = new Vector2Int(
                    UnityEngine.Random.Range(0, GameConstants.MapRectangleCount),
                    UnityEngine.Random.Range(0, GameConstants.MapRectangleCount)
                );

                while (mapRectangle.x == 1 && mapRectangle.y == 1)
                {
                    mapRectangle = new Vector2Int(
                        UnityEngine.Random.Range(0, GameConstants.MapRectangleCount),
                        UnityEngine.Random.Range(0, GameConstants.MapRectangleCount)
                    );
                }

                var positionSpawnpoint = GetRandomPointForObject(
                    new Vector2Int(1, 1),
                    new Vector2Int(GameConstants.MapRectangleCount, GameConstants.MapRectangleCount),
                    mapRectangle
                );

                var instantiatedSpawnpoint = Instantiate(prefabSpawnpoint, positionSpawnpoint, Quaternion.identity);

                var componentSpawnpoint = instantiatedSpawnpoint.GetComponent<SpawnPoint>();
                componentSpawnpoint.prefabSpawnObject = spawnPoint.prefabObjectToSpawn;
                componentSpawnpoint.spawnDelay = spawnPoint.spawnDelay;
            }
        }

        private void generateWorld()
        {
            if (MapAtlas.Instance == null)
            {
                Debug.LogError("Die MapAtlas wurde nicht geladen. Stellen Sie sicher, dass Sie sie zuerst laden, bevor Sie die Welt generieren.");
                return;
            }

            // Default Layer
            builderMap.InitializeBaseLayer(Utility.EARTH_TILE);

            // Generate( tileData Eines Spezifischen tiles, float coverage, float coherence )
            builderMap.Generate(Utility.GRASS_TILE, 0.2f, 0.2f);
        }

        public GameObject GetTileOnPosition(Vector2 position)
        {
            return map[(int)position.x, (int)position.y, 0];
        }

        public RectInt GetMapBounds()
        {
            return _mapBounds;
        }

        public RectInt GetPossibleSpawnBounds(Vector2Int spawnObjectSize)
        {
            var spawnBounds = new RectInt(
                xMin: spawnObjectSize.x,
                yMin: spawnObjectSize.y,
                width: columns - spawnObjectSize.x,
                height: rows - spawnObjectSize.y
            );

            return spawnBounds;
        }

        public Vector2 GetRandomPointForObject(Vector2Int objectSize, Vector2Int rectangles, Vector2Int rectangleToSpawnIn)
        {
            int rectWidth = columns / rectangles.x;
            int rectHeight = rows / rectangles.y;

            int rectX = rectangleToSpawnIn.x * rectWidth;
            int rectY = rectangleToSpawnIn.y * rectHeight;

            int maxAttempts = 100;
            while (maxAttempts-- > 0)
            {
                int x = UnityEngine.Random.Range(rectX, rectX + rectWidth - objectSize.x);
                int y = UnityEngine.Random.Range(rectY, rectY + rectHeight - objectSize.y);
                var position = new Vector2Int(x, y);

                if (CanPlaceObject(position, objectSize))
                {
                    MarkPlacement(position, objectSize);
                    return new Vector2(position.x, position.y);
                }
            }

            Debug.LogWarning("Couldn't find a suitable position for the object.");
            return Vector2.negativeInfinity;
        }

        public bool CanPlaceObject(Vector2Int position, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (_gridOccupancy[new Vector2Int(position.x + x, position.y + y)])
                        return false;
                }
            }
            return true;
        }

        public void MarkPlacement(Vector2Int position, Vector2Int size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    _gridOccupancy[new Vector2Int(position.x + x, position.y + y)] = true;
                }
            }
        }
    }
}
