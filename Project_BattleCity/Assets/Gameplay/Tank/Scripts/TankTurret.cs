using UnityEngine;
using Gameplay.Projectile;
using Core.Event;
using Core.Tag;
using Core;
using System;
using Gameplay.Health;

namespace Gameplay.Tank
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class TankTurret : ComponentRotation
    {
        #region tank
        [Header("Data")]
        [SerializeField] private DataTankTurret _dataTankTurret;
        #endregion

        #region animation
        private const string KEY_DIRECTIONX = "directionX";
        private const string KEY_DIRECTIONY = "directionY";

        private Animator _animator;
        #endregion

        #region barrel
        private bool _isReloaded = true;
        private bool _isReloading = false;
        private float _timeReload;
        private bool _isDeactivated = false;
        #endregion

        #region muzzle
        private ParticleSystem particleSystemMuzzleFlash;
        #endregion

        #region components
        private AudioSource _audioSource;
        private ComponentTags _componentTags;
        private ComponentHealth _componentHealth;
        #endregion

        #region events
        private GameEventAudio _eventAudioShoot;
        #endregion

        public void SetDataTankTurret(DataTankTurret dataTankTurret)
        {
            _dataTankTurret = dataTankTurret;

            initializeData();
        }

        public void DeactivateForDuration(float duration)
        {
            _isDeactivated = true;

            Invoke(nameof(Activate), duration);
        }

        public void Activate()
        {
            _isDeactivated = false;
        }

        private void initializeData()
        {
            if (_componentTags == null)
            {
                _componentTags = GetComponentInParent<ComponentTags>();

                if (_componentTags == null)
                {
                    _componentTags = gameObject.AddComponent<ComponentTags>();
                }
            }

            if (!_componentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTurret))) _componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTurret));

            if (_dataTankTurret == null) return;

            if (_animator == null) _animator = GetComponent<Animator>();
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
            if (_componentHealth == null) _componentHealth = GetComponentInParent<ComponentHealth>();

            rotationTime = _dataTankTurret.rotationSpeed; // from ComponentRotation
            _timeReload = _dataTankTurret.reloadTime;
            _animator.runtimeAnimatorController = _dataTankTurret.animationController;
            _eventAudioShoot = _dataTankTurret.eventAudioShot;

            UpdateAnimationParameters();
        }

        private void LateUpdate() {
            if (_componentHealth.currentHealth == 1) {
                _animator.runtimeAnimatorController = _dataTankTurret.animationControllerDamaged;
            } else {
                _animator.runtimeAnimatorController = _dataTankTurret.animationController;
            }
        }

        override public void Rotate(float directionInput, float rotationModifier)
        {
            if (_isDeactivated) return;

            base.Rotate(directionInput, rotationModifier);

            UpdateAnimationParameters();
        }

        public void RotateLeft()
        {
            if (_isDeactivated) return;

            base.Rotate(DIRECTION_ROTATE_LEFT, 0.0f);

            UpdateAnimationParameters();
        }

        public void RotateRight()
        {
            if (_isDeactivated) return;

            base.Rotate(DIRECTION_ROTATE_RIGHT, 0.0f);

            UpdateAnimationParameters();
        }

        public float GetRange()
        {
            if (_dataTankTurret == null || _dataTankTurret.dataShell == null) return 0;

            return _dataTankTurret.dataShell.distanceMax;
        }

        public bool IsReloading()
        {
            return _isReloading;
        }

        public void Shoot()
        {
            if (_isDeactivated) return;

            if (_dataTankTurret == null) return;

            if (_isReloaded)
            {
                _eventAudioShoot?.Play(transform.parent.position);

                if (_dataTankTurret.effectMuzzleFlash != null)
                {
                    Vector3 effectSpawnPoint = GetComponent<Renderer>().bounds.center + (Vector3)(_currentDirection * _dataTankTurret.offsetFactorMuzzle);

                    if (particleSystemMuzzleFlash == null)
                    {
                        particleSystemMuzzleFlash = Instantiate(_dataTankTurret.effectMuzzleFlash, effectSpawnPoint, Quaternion.identity);
                        particleSystemMuzzleFlash.transform.parent = gameObject.transform;
                    }
                    else
                    {
                        particleSystemMuzzleFlash.transform.position = effectSpawnPoint;
                        particleSystemMuzzleFlash.Play();
                    }
                }

                if (_dataTankTurret.dataShell.prefabShell != null)
                {
                    SpawnProjectile(_dataTankTurret.dataShell.prefabShell);
                }

                _isReloaded = false;
            }

            if (!_isReloaded && !_isReloading)
            {
                Invoke("Reload", _timeReload);

                _isReloading = true;
            }
        }

        override protected void Awake()
        {
            base.Awake();

            initializeData();
        }

        private void UpdateAnimationParameters()
        {
            if (_dataTankTurret == null) return;

            _animator.SetFloat(KEY_DIRECTIONX, _currentDirection.x);
            _animator.SetFloat(KEY_DIRECTIONY, _currentDirection.y);
        }

        private void Reload()
        {
            _isReloaded = true;
            _isReloading = false;
        }

        private void SpawnProjectile(GameObject prefabProjectile)
        {
            if (_isDeactivated) return;

            if (_dataTankTurret == null) return;

            Vector3 projectileSpawnPoint = GetComponent<Renderer>().bounds.center;
            projectileSpawnPoint.y += 0.15f;

            GameObject gameObjectProjectile = Instantiate(prefabProjectile, projectileSpawnPoint + (Vector3)(_currentDirection * _dataTankTurret.offsetFactorMuzzle), Quaternion.identity); // TODO
            gameObjectProjectile.transform.right = -1 * _currentDirection;

            ComponentShell componentShell = gameObjectProjectile.GetComponent<ComponentShell>();
            componentShell.directionForward = _currentDirection;
            componentShell.InitializeShell(_dataTankTurret.dataShell);

            Physics2D.IgnoreCollision(gameObjectProjectile.GetComponent<Collider2D>(), transform.parent.gameObject.GetComponent<Collider2D>());
        }
    }
}
