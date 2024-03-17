using UnityEngine;
using Gameplay.Health;
using Core.Tag;
using Core;

namespace Gameplay.Projectile
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(TrailRenderer))]
    [RequireComponent(typeof(ComponentTags))]
    public class ComponentShell : MonoBehaviour
    {
        #region shell
        [Header("Shell")]
        [SerializeField] private DataShell _dataShell;

        private bool _initialized = false;
        #endregion

        #region movement
        public Vector2 directionForward { get; set; }
        #endregion

        #region ownership
        public bool ownershipPlayer { get; set; }
        #endregion

        #region bullet-drop
        public float _bulletDropEachFrame { get; private set; }
        #endregion

        #region gameobject
        private Rigidbody2D _rigidbody;
        #endregion

        #region shadow
        private GameObject _gameObjectShadow;
        #endregion

        #region tracking
        private Vector3 _positionPrevious;
        private float _totalDistanceTraveled;
        #endregion

        void Awake()
        {
            var componentTags = GetComponent<ComponentTags>();
            componentTags?.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagProjectile));
        }

        public void InitializeShell(DataShell dataShell)
        {
            _dataShell = dataShell;

            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();

            _positionPrevious = transform.position;
            _bulletDropEachFrame = 0;
            _rigidbody.velocity = directionForward * _dataShell.velocity;

            if (_dataShell.enableShadow && _dataShell.prefabShadow != null)
            {
                GameObject gameObjectShadow = Instantiate(_dataShell.prefabShadow, GetComponent<Renderer>().bounds.center, Quaternion.identity);
                ComponentShellShadow componentShellShadow = gameObjectShadow.GetComponent<ComponentShellShadow>();

                componentShellShadow.Initialize(this);

                _gameObjectShadow = gameObjectShadow;
            }

            _initialized = true;
        }

        private void Update()
        {
            if (!_initialized) return;

            _totalDistanceTraveled += Vector2.Distance(_positionPrevious, transform.position);

            _positionPrevious = transform.position;

            if (_dataShell.distanceMax <= _totalDistanceTraveled)
            {
                TriggerDestruction();
            }
        }

        private void FixedUpdate()
        {
            if (!_initialized) return;

            float time = Time.deltaTime;
            float gravityFactor = 1.5f;

            _bulletDropEachFrame += gravityFactor * time * time;

            transform.position = new Vector2(transform.position.x, transform.position.y - _bulletDropEachFrame);
        }

        void OnTriggerEnter2D(Collider2D objectCollision)
        {
            var componentTags = objectCollision.gameObject.GetComponentInChildren<ComponentTags>();

            if (!componentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagHealth))) return;

            TriggerDestruction();

            var componentHealthDamageable = objectCollision.gameObject.GetComponentInChildren<ComponentHealth>();
            if (componentHealthDamageable != null)
            {
                Debug.Log("SHELL DAMAGE: " + _dataShell.damage);
                componentHealthDamageable.ModifyHealth(-_dataShell.damage);
            }
        }

        private void TriggerDestruction()
        {
            if (_dataShell.effectHit != null)
            {
                var effectHit = Instantiate(_dataShell.effectHit, gameObject.transform.position, Quaternion.identity);
                Destroy(effectHit, _dataShell.effectHitDestructionDelay);
            }

            Destroy(_gameObjectShadow);
            Destroy(gameObject);
        }

        public float GetVelocity()
        {
            return _dataShell.velocity;
        }
    }
}

