using UnityEngine;
using Core.Event;
using Core.Tag;

namespace Gameplay.Bunker
{
    [CreateAssetMenu(fileName = "NewDataBunker", menuName = "Data/Bunker", order = 1)]
    public class DataBunker : ScriptableObject
    {
        #region bunker
        [Header("Bunker")]
        public bool isFriendly;
        public Sprite spriteBunker;
        public Vector2Int sizeUnit;

        [Header("Bunker Light")]
        public Sprite spriteBunkerLight;
        public Vector2 localPositionBunkerLight;
        #endregion

        #region ownership
        [Header("Capping")]
        public float capTimeMax;
        #endregion

        #region events
        [Header("Events")]
        public GameEvent eventCheckGameState;
        #endregion

        #region spawn
        [Header("Spawn")]
        public GameObject prefabSpawnObject;
        public GameObject prefabDropUpgrade;
        public float spawnTime;
        #endregion

        #region tags
        [Header("Tags")]
        public DataTag tagTank;
        public DataTag tagEnemy;
        #endregion
    }
}

