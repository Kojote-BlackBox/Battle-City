using UnityEngine;
using System;
using UnityEngine.InputSystem;
using Core.Reference;
using World.Builder;
using Gameplay.Bunker;
using Core.Tag;

namespace World
{
    public class Map : MonoBehaviour
    {
        #region map
        public GameObject[,,] map;
        public int rows;
        public int columns;
        [HideInInspector] public int layer;
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
        public ReferenceRelatedGameObjects trackerBunker;
        public DataBunker dataBunkerFriendly;
        public DataBunker[] dataBunkersEnemy;
        public GameObject parentObjectBunkers;
        #endregion

        #region tags
        [Header("Tags")]
        public DataTag tagBunker;
        public DataTag tagFriendly;
        public DataTag tagEnemy;
        #endregion

        private void Awake()
        {
            initializeAudio();
            initializeMap();
            initializeBuilder();

            generateWorld();
            generateBunkers();
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
                trackerBunker = trackerBunker,
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

        public Vector2 GetMapBoundsX()
        {
            return new Vector2();
        }

        public Vector2 GetMapBoundsY()
        {
            return new Vector2();
        }
    }
}
