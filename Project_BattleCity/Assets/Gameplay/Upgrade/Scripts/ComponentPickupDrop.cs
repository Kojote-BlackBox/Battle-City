using UnityEngine;

namespace Gameplay.Pickup
{
    public class ComponentPickupDrop : MonoBehaviour
    {
        #region effects
        [Header("Effects")]
        public GameObject prefabEffectDrop;
        private ParticleSystem _particlesEffectDrop;
        private GameObject _gameObjectEffectDrop;
        #endregion



        public void Start()
        {
            if (prefabEffectDrop == null) return;

            _gameObjectEffectDrop = Instantiate(prefabEffectDrop, transform.position, Quaternion.identity);
            _gameObjectEffectDrop.transform.parent = transform;
            _particlesEffectDrop = _gameObjectEffectDrop.GetComponent<ParticleSystem>();

            if (enabled)
                _particlesEffectDrop.Play();
            else
                _particlesEffectDrop.Stop();
        }

        public void OnEnable()
        {
            if (_particlesEffectDrop == null) return;

            _particlesEffectDrop.Play();
        }

        public void OnDisable()
        {
            if (_particlesEffectDrop == null) return;

            _particlesEffectDrop.Stop();
        }

        public void OnDestroy()
        {
            Destroy(_gameObjectEffectDrop);
        }
    }
}


