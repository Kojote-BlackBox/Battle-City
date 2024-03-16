using System.Collections.Generic;
using AI;
using Core.Tag;
using Core.Reference;
using Core.Event;
using UnityEngine;
using System.Linq;
using Core.Input;
using Core.Track;
using Gameplay.Health;

namespace Core.Spawn
{
    public class SpawnSystem : MonoBehaviour
    {
        #region tracking
        private float _elapsedTime;
        #endregion

        #region spawns
        [HideInInspector] public List<SpawnPoint> spawnPoints;
        private SpawnPoint _spawnNext;
        private SpawnPoint _spawnPlayer;
        #endregion

        /*#region upgrades
        [Header("Upgrades")]
        public List<Upgrade> upgrades;
        private Tilemap _tilemapWalkable;
        private List<Vector3> _tileWorldLocations;
        #endregion*/

        #region events
        [Header("Events")]
        public GameEvent eventUpdateSpawnInfo;
        public GameEvent eventSpawnUpgrade;
        #endregion

        #region player
        [Header("Player")]
        public UnityEngine.Camera playerCamera;
        #endregion

        private void Awake()
        {
            /*var gameObjectTileMap = GameObject.FindGameObjectWithTag(TagWalkable);
            if (gameObjectTileMap != null)
            {
                _tilemapWalkable = gameObjectTileMap.GetComponent<Tilemap>();

                _tileWorldLocations = new List<Vector3>();

                foreach (var pos in _tilemapWalkable.cellBounds.allPositionsWithin)
                {
                    var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                    var place = _tilemapWalkable.CellToWorld(localPlace);

                    if (_tilemapWalkable.HasTile(localPlace))
                        _tileWorldLocations.Add(place);
                }
            }*/
        }

        public void Initialize()
        {
            var gameObjectsSpawn = GameObject.FindGameObjectsWithTag(GameConstants.TagSpawn);

            Debug.Log("found " + gameObjectsSpawn.Count() + " spawn points");

            if (gameObjectsSpawn.Length <= 0) return;

            foreach (var gameObjectSpawn in gameObjectsSpawn) {
                var spawnPoint = gameObjectSpawn.GetComponent<SpawnPoint>();

                if (spawnPoint == null || spawnPoint.prefabSpawnObject == null) {
                    Debug.LogError("can not spawn null object");
                    continue;
                }

                spawnPoints.Add(spawnPoint);

                TrackManager.Instance.spawns.totalGameObjects++;
            }
  
            spawnPoints.Sort(delegate (SpawnPoint x, SpawnPoint y)
            {
                if (x.spawnDelay > y.spawnDelay)
                    return 1;
                if (x.spawnDelay < y.spawnDelay)
                    return -1;

                return 0;
            });

            NextSpawnPoint();
        }

        private void NextSpawnPoint()
        {
            if (spawnPoints.Count <= 0)
            {
                Debug.Log("no spawn points remaining");

                _spawnNext = null;
                //spawnInfo.image = null;
                //spawnInfo.label = "No reinforcements!";
                eventUpdateSpawnInfo.Raise();

                return;
            }

            _spawnNext = spawnPoints[0];

            Debug.Log(_spawnNext);
            Debug.Log("spawn points remaining" + spawnPoints.Count);

            if (_spawnNext.isFriendly)
            {
                _spawnPlayer = _spawnNext;
            }

            spawnPoints.RemoveAt(0);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;

            if (_spawnNext == null || !(_spawnNext.spawnDelay <= _elapsedTime)) return;

            Debug.Log("spawning");

            var instanceGameObjectSpawn = Instantiate(_spawnNext.prefabSpawnObject, _spawnNext.gameObject.transform.position, Quaternion.identity);

            var instancedComponentTags = instanceGameObjectSpawn.GetComponentInChildren<ComponentTags>();
            if (instancedComponentTags != null)
            {
                if (instancedComponentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTank)))
                {
                    if (_spawnNext.isFriendly)
                    {
                        Debug.Log("spawning friendly tank");

                        instancedComponentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagFriendly));

                        if (TrackManager.Instance.player.gameObject == null) {
                            spawnPlayer(instanceGameObjectSpawn);
                        } else {
                            instanceGameObjectSpawn.AddComponent<AIController>(); // TODO: differentiate between enemy and friend in ai controller
                            TrackManager.Instance.allies.totalGameObjects++;
                            TrackManager.Instance.allies.activeGameObjects.Add(instanceGameObjectSpawn);
                        }
                    } 
                    else
                    {
                        Debug.Log("spawning enemy tank");

                        TrackManager.Instance.enemies.totalGameObjects++;
                        TrackManager.Instance.enemies.activeGameObjects.Add(instanceGameObjectSpawn);
                        instanceGameObjectSpawn.AddComponent<AIController>();

                        instancedComponentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagEnemy));
                    }
                } else if (instancedComponentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagPickup))) {
                    Debug.Log("spawning pickup");

                    TrackManager.Instance.pickups.activeGameObjects.Add(instanceGameObjectSpawn);
                    TrackManager.Instance.pickups.totalGameObjects++;
                }
            }

            NextSpawnPoint();
        }

        public void SpawnPlayer()
        {
            Debug.Log("spawn player");

            if (_spawnPlayer == null) return;

            var instancedGameObjectPlayer = Instantiate(_spawnPlayer.prefabSpawnObject, _spawnPlayer.gameObject.transform);

            spawnPlayer(instancedGameObjectPlayer);

        }

        private void spawnPlayer (GameObject instancedGameObject) {

            TrackManager.Instance.player.gameObject = instancedGameObject;

            var instancedComponentTags = instancedGameObject.GetComponentInChildren<ComponentTags>();

            instancedComponentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagFriendly));

            instancedGameObject.AddComponent<PlayerController>();

            var camera = UnityEngine.Camera.main;
            var componentCamera = camera.GetComponent<Camera>();
            componentCamera.gameObjectToFollow = instancedGameObject;

            var componentHealth = instancedGameObject.GetComponentInChildren<ComponentHealth>();
            componentHealth.onHealthChanged = GameEventManager.Instance.updateHealth;
        }

        public void SpawnUpgrade()
        {
            /*if (upgrades.Count <= 0) return;

            if (upgrade.gameObject != null)
                Destroy(upgrade.gameObject);

            var randomIndex = UnityEngine.Random.Range(0, _tileWorldLocations.Count);
            var location = _tileWorldLocations[randomIndex];

            upgrade.gameObject = Instantiate(upgrades[0].gameObject, location, Quaternion.identity);

            upgrades.RemoveAt(0);*/
        }
    }
}
