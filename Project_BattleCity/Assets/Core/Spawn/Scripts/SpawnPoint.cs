using Core.Tag;
using UnityEngine;
using UnityEditor.Animations;

namespace Core.Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        #region spawn
        [Header("Spawn")]
        public GameObject prefabSpawnObject;

        public int spawnDelay; // TODO: make float

        public bool isFriendly;

        public bool enableUpgradeDrop;
        #endregion

        #region appearance
        [Header("Appearance")]
        public bool enableSpawnAnimation;

        public AnimatorController animationController;

        private Animator _animator;
        #endregion

        private void Awake() {
            if (enableSpawnAnimation) {
                _animator = GetComponent<Animator>();
                if (_animator == null) {
                    Debug.LogError("spawn animation enabled but no animator on object");

                    return;
                }

                if (animationController == null) {
                    Debug.LogError("spawn animation enabled but no animation controller set");

                    return;
                }

                _animator.runtimeAnimatorController = animationController;
            }
        }
    }
}
