using System.Collections.Generic;
using AI;
using Core.Tag;
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
        #region initialize
        private bool isInitialized = false;
        #endregion

        #region spawns
        [HideInInspector] public List<SpawnPoint> spawnPoints;
        private SpawnPoint _spawnPlayer;
        #endregion

        public void Initialize()
        {
            var gameObjectsSpawn = GameObject.FindGameObjectsWithTag(GameConstants.TagSpawn);

            Debug.Log("found " + gameObjectsSpawn.Count() + " spawn points");

            if (gameObjectsSpawn.Length <= 0) return;

            foreach (var gameObjectSpawn in gameObjectsSpawn)
            {
                var spawnPoint = gameObjectSpawn.GetComponent<SpawnPoint>();

                if (spawnPoint == null || spawnPoint.prefabsToSpawn == null)
                {
                    Debug.LogError("can not spawn null object");
                    continue;
                }

                if (spawnPoint.isPlayerSpawn) _spawnPlayer = spawnPoint;

                spawnPoints.Add(spawnPoint);

                TrackManager.Instance.spawns.totalGameObjects++;
            }

            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized || !isEverythingInitialized()) return;

            var spawnsToRemove = new List<SpawnPoint>();

            foreach (var spawn in spawnPoints)
            {
                if (spawn.IsReadToSpawn())
                {
                    var instanceGameObjectSpawn = Instantiate(spawn.GetNextSpawnPrefab(), spawn.gameObject.transform.position, Quaternion.identity);

                    var instancedComponentTags = instanceGameObjectSpawn.GetComponentInChildren<ComponentTags>();
                    if (instancedComponentTags != null)
                    {
                        if (instancedComponentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTank)))
                        {
                            spawnTank(instanceGameObjectSpawn, instancedComponentTags, spawn.isFriendly);
                        }
                        else if (instancedComponentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagPickup)))
                        {
                            spawnPickup(instanceGameObjectSpawn);
                        }
                    }

                    spawn.instantiatedGameObject = instanceGameObjectSpawn;

                    if (spawn.enableRespawn) spawn.Reset();

                    if (!spawn.enableRespawn) spawnsToRemove.Add(spawn);
                }
            }

            if (spawnsToRemove.Count > 0) Debug.Log("spawns remaining" + (spawnPoints.Count - spawnsToRemove.Count));

            foreach (var spawnToRemove in spawnsToRemove)
            {
                spawnPoints.Remove(spawnToRemove);
            }
        }

        public void SpawnPlayer()
        {
            Debug.Log("spawn player");

            if (_spawnPlayer == null) return;

            var instancedGameObjectPlayer = Instantiate(_spawnPlayer.GetNextSpawnPrefab(), _spawnPlayer.gameObject.transform);

            makePlayerTank(instancedGameObjectPlayer);
        }

        private void spawnTank(GameObject instanceGameObjectSpawn, ComponentTags instancedComponentTags, bool isFriendly)
        {
            if (isFriendly)
            {
                Debug.Log("spawning friendly tank");

                instancedComponentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagFriendly));

                if (TrackManager.Instance.player.gameObject == null)
                {
                    makePlayerTank(instanceGameObjectSpawn);
                }
                else
                {
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
        }

        private bool isEverythingInitialized() {
            return TrackManager.Instance.enemies != null && TrackManager.Instance.allies != null && TrackManager.Instance.player != null;
        }

        private void spawnPickup(GameObject instanceGameObjectSpawn)
        {
            Debug.Log("spawning pickup");

            TrackManager.Instance.pickups.activeGameObjects.Add(instanceGameObjectSpawn);
            TrackManager.Instance.pickups.totalGameObjects++;
        }

        private void makePlayerTank(GameObject instancedGameObject)
        {
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
    }
}
