using Core;
using Core.Tag;
using Effect.Trail;
using Gameplay.Health;
using UnityEngine;
using World;

namespace Gameplay.Tank
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(ComponentTags))]
    public class TankBody : ComponentRotation
    {
        #region tank
        [Header("Data")]
        [SerializeField] private DataTankBody _dataTankBody;
        #endregion

        #region animation
        private const string KEY_DIRECTIONX = "directionX";
        private const string KEY_DIRECTIONY = "directionY";

        private Animator _animator;
        #endregion

        #region movement
        private float _factorSpeed = 1.0f;
        private float _thresholdConsideredMovement = 0.1f;
        private bool _isMoving = false;
        private float _isMovingCheckInterval = 0.25f;
        private float _elapsedTime;
        private bool _isDeactivated = false;
        #endregion

        #region environment
        private Map _map;
        #endregion

        #region components
        private Rigidbody2D _rigidbody;
        private AudioSource _audioSource;
        private ComponentTags _componentTags;
        private ComponentEffectTrail _componentEffectTrail;
        private ComponentHealth _componentHealth;
        #endregion

        #region tracking
        private Vector2 _positionPrevious;
        #endregion

        void Update()
        {
            if (_dataTankBody == null || _isDeactivated) return;

            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _isMovingCheckInterval)
            {
                _elapsedTime = 0f;

                _isMoving = Vector2.Distance(transform.position, _positionPrevious) >= _thresholdConsideredMovement;

                _positionPrevious = transform.position;
            }

            UpdateEffects();

            if (_componentEffectTrail != null) _componentEffectTrail.directionTrail = _currentDirection;
        }

        public void DeactiveForDuration(float duration)
        {
            Debug.Log("deactivated tank body");

            _isDeactivated = true;

            Invoke(nameof(Activate), duration);
        }

        public void Activate()
        {
            _isDeactivated = false;
        }

        public void SetDataTankBody(DataTankBody dataTankBody)
        {
            _dataTankBody = dataTankBody;

            initializeData();
        }

        private void initializeData()
        {
            if (_componentTags == null)
            {
                _componentTags = GetComponent<ComponentTags>();

                if (_componentTags == null)
                {
                    _componentTags = gameObject.AddComponent<ComponentTags>();
                }
            }

            if (!_componentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTank))) _componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTank));

            if (_dataTankBody == null) return;

            if (_animator == null) _animator = GetComponent<Animator>();
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
            if (_componentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagHealth)))
            {
                var componentHealth = GetComponent<ComponentHealth>();
                if (componentHealth != null)
                {
                    componentHealth.dataHealth = _dataTankBody.dataHealth;
                    componentHealth.Reset();
                    _componentHealth = componentHealth;
                }
            }

            if (_dataTankBody.enableTrail)
            {
                if (_componentEffectTrail == null)
                {
                    _componentEffectTrail = gameObject.AddComponent<ComponentEffectTrail>();
                }

                _componentEffectTrail.prefabTrail = _dataTankBody.prefabTrail;
                _componentEffectTrail.prefabTrail.transform.localScale = new Vector3(1.0f, _dataTankBody.scaleTrail, 1.0f);
                _componentEffectTrail.trailSegments = _dataTankBody.trailSegments;
                _componentEffectTrail.trailTime = _dataTankBody.trailTime;
            }


            _animator.runtimeAnimatorController = _dataTankBody.animationController;
            rotationTime = _dataTankBody.rotationSpeed; // from ComponentRotation

            if (_audioSource.isPlaying) _audioSource.Stop();

            _audioSource.clip = _dataTankBody.audioClipEngine;
            _audioSource.volume = _dataTankBody.volumeIdle;
            _audioSource.pitch = _dataTankBody.pitchIdle;

            _audioSource.Play();

            UpdateAnimationParameters();
        }

        override protected void Awake()
        {
            base.Awake();

            initializeData();

            _map = GameObject.Find("Map").GetComponent<Map>();

            _positionPrevious = transform.position;
        }

        private void UpdateAnimationParameters()
        {
            if (_dataTankBody == null) return;

            _animator.SetFloat(KEY_DIRECTIONX, _currentDirection.x);
            _animator.SetFloat(KEY_DIRECTIONY, _currentDirection.y);
        }

        public void LateUpdate() {
            if (_componentHealth.currentHealth == 1) {
                _animator.runtimeAnimatorController = _dataTankBody.animationControllerDamaged;
            } else {
                _animator.runtimeAnimatorController = _dataTankBody.animationController;
            }
        }

        override public void Rotate(float directionInput, float rotationModifier)
        {
            if (_isDeactivated) return;

            base.Rotate(directionInput, rotationModifier); // TODO: propagate tank body rotation

            UpdateAnimationParameters();
        }

        public void Move(float directionInput)
        {
            if (_isDeactivated) return;

            _inputDirection.y = directionInput;
        }

        private void FixedUpdate()
        {
            if (_isDeactivated) return;

            if (_dataTankBody == null) return;

            if (_inputDirection.y == 0f) return;

            var movementInDirection = _currentDirection * _factorSpeed * Time.fixedDeltaTime;
            var movement = _inputDirection.y < 0f ? -1 * (_dataTankBody.forwardSpeed * _dataTankBody.backwardSpeedPercentage) * movementInDirection : movementInDirection * _dataTankBody.forwardSpeed;

            _rigidbody.MovePosition((Vector2)transform.position + movement);
        }

        private void UpdateEffects()
        {
            if (_dataTankBody == null) return;

            if (_isMoving)
            {
                _audioSource.pitch = _dataTankBody.pitchMovement;
                _audioSource.volume = _dataTankBody.volumeMovement;

                if (_componentEffectTrail != null) _componentEffectTrail.SetEnabled(true);
            }
            else
            {
                _audioSource.pitch = _dataTankBody.pitchIdle;
                _audioSource.volume = _dataTankBody.volumeIdle;

                if (_componentEffectTrail != null) _componentEffectTrail.SetEnabled(false);
            }
        }

        void UpdateEnviroment() // TODO
        {
            GameObject tile = _map.GetTileOnPosition((Vector2)GetComponent<Renderer>().bounds.center);
            _factorSpeed = tile.GetComponent<Tile>().slowDownFactor;
        }
    }
}
