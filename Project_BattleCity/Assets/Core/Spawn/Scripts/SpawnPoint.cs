using Core.Tag;
using UnityEngine;
using UnityEditor.Animations;

namespace Core.Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        #region spawn
        [Header("Spawn")]
        public GameObject[] prefabsToSpawn;
        private int _nextPrefab = 0;
        public GameObject instantiatedGameObject;

        public int spawnDelay;
        private float _elapsedTime = 0f;

        public bool isFriendly;
        public bool isPlayerSpawn;
        private bool _readyToSpawn = false;
        private bool _isReset = true;
        #endregion

        #region respawn
        [Header("Respawn")]
        public bool enableRespawn;
        public bool onlyRespawnIfPrevIsDestroyed;
        public int timeToRespawn;
        #endregion

        #region appearance
        [Header("Appearance")]
        public bool enableSpawnAnimation;

        public AnimatorController animationController;

        private Animator _animator;
        #endregion

        private void Awake()
        {
            if (enableSpawnAnimation)
            {
                _animator = GetComponent<Animator>();
                if (_animator == null)
                {
                    Debug.LogError("spawn animation enabled but no animator on object");

                    return;
                }

                if (animationController == null)
                {
                    Debug.LogError("spawn animation enabled but no animation controller set");

                    return;
                }

                _animator.runtimeAnimatorController = animationController;
            }
        }

        private void Update()
        {
            if (onlyRespawnIfPrevIsDestroyed && instantiatedGameObject != null) return;

            if (!_isReset) return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= spawnDelay)
            {
                if (onlyRespawnIfPrevIsDestroyed && instantiatedGameObject == null)
                {
                    _readyToSpawn = true;
                    _isReset = false;
                }
                else if (!onlyRespawnIfPrevIsDestroyed)
                {
                    _readyToSpawn = true;
                    _isReset = false;
                }
            }
        }

        public GameObject GetNextSpawnPrefab()
        {
            if (_nextPrefab >= prefabsToSpawn.Length) return null;

            var prefabToSpawn = prefabsToSpawn[_nextPrefab];

            _nextPrefab++;

            if (_nextPrefab >= prefabsToSpawn.Length) _nextPrefab = prefabsToSpawn.Length - 1;

            return prefabToSpawn;
        }

        public bool IsReadToSpawn()
        {
            return _readyToSpawn;
        }

        public void Reset()
        {
            if (!enableRespawn) return;

            _isReset = true;
            _readyToSpawn = false;
            _elapsedTime = 0f;
        }
    }
}
