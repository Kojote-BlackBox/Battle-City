using Core.Tag;
using UnityEngine;

namespace Core.Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        #region spawn
        [Header("Spawn")]
        public GameObject prefabSpawnObject;
        public int spawnDelay; // TODO: make float
        public bool isFriendly;
        public ComponentTags spawnTags;
        public bool enableUpgradeDrop;
        #endregion

        private void Start()
        {
            spawnTags = prefabSpawnObject.GetComponentInChildren<ComponentTags>(true);
        }
    }
}
