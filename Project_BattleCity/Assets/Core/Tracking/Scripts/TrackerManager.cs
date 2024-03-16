using Core.Pattern;
using UnityEngine;
using Core.Reference;
using UnityEngine.Events;

namespace Core.Track
{
    public class TrackManager : Singleton<TrackManager>
    {
        #region player
        [Header("Player")]
        public ReferenceGameObject player;
        public ReferenceInt playerLives;
        public ReferenceGameObjects allies;
        public ReferenceRelatedGameObjects allyBunkers;
        #endregion

        #region enemies
        [Header("Enemies")]
        public ReferenceGameObjects enemyBunkers;
        public ReferenceGameObjects enemies;
        #endregion

        #region spawns
        [Header("Spawns")]
        public ReferenceGameObjects spawns;
        #endregion

        #region misc
        [Header("Misc")]
        public ReferenceGameObjects remains;
        public ReferenceGameObjects pickups;
        #endregion

        public void Reset() {
            player.gameObject = null;
            playerLives.value = 0;

            allies.totalGameObjects = 0;
            allies.activeGameObjects.Clear();
            allies.destroyedGameObjects = 0;

            allyBunkers.gameObject = null;
            allyBunkers.relatedGameObjects.Clear();

            enemies.totalGameObjects = 0;
            enemies.activeGameObjects.Clear();
            enemies.destroyedGameObjects = 0;

            enemyBunkers.totalGameObjects = 0;
            enemyBunkers.activeGameObjects.Clear();
            enemyBunkers.destroyedGameObjects = 0;

            spawns.totalGameObjects = 0;
            spawns.activeGameObjects.Clear();
            spawns.destroyedGameObjects = 0;

            remains.totalGameObjects = 0;
            remains.activeGameObjects.Clear();
            remains.destroyedGameObjects = 0;

            pickups.totalGameObjects = 0;
            pickups.activeGameObjects.Clear();
            pickups.destroyedGameObjects = 0;
        }
    }
}
