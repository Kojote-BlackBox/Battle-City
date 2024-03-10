using Core.Pattern;
using UnityEngine;
using Core.Reference;

namespace Core.Track
{
    public class TrackManager : Singleton<TrackManager>
    {
        #region player
        [Header("Player")]
        public ReferenceGameObject player;
        public ReferenceInt playerHealth;
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
        #endregion

        public void Reset() { // TODO: listen to event GameExit or something like that
        }
    }
}
