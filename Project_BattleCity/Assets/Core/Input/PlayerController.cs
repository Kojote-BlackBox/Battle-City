using UnityEngine;
using UnityEngine.InputSystem;
using Gameplay.Tank;
using Core.Aim;
using Utilities;

namespace Core.Input
{
    [RequireComponent(typeof(ComponentAimIndicator))]
    public class PlayerController : MonoBehaviour
    {
        #region directions
        private Vector2 _directionTank = Vector2.zero;
        private float _directionTurret;
        #endregion

        #region tank
        private TankBody _tankBody;
        private TankTurret _tankTurret;
        #endregion

        #region input
        private InputMapping _playerActions;
        #endregion

        #region aim
        private ComponentAimIndicator _componentAimIndicator;
        #endregion

        private void Awake()
        {
            _playerActions = new InputMapping();

            _tankBody = gameObject.GetComponentInChildren<TankBody>();
            _tankTurret = gameObject.GetComponentInChildren<TankTurret>();
            _componentAimIndicator = gameObject.GetComponentInChildren<ComponentAimIndicator>();

            _playerActions.bindingMask = InputBinding.MaskByGroup("Mouse"); // Note: You can change the input schema here!

            _playerActions.Gameplay.Shoot.started += ctx => OnShoot(ctx);
            _playerActions.Gameplay.TurretRotation.performed += ctx => OnTurretRotation(ctx);
            _playerActions.Gameplay.ChassieMovement.performed += ctx => OnChassieMovement(ctx);

            _playerActions.Gameplay.TurretRotation.canceled += ctx => _directionTurret = 0f;
            _playerActions.Gameplay.ChassieMovement.canceled += ctx => _directionTank = Vector2.zero;
        }

        private void Update()
        {
            if (_playerActions.bindingMask.HasValue && _playerActions.bindingMask.Value.groups == "Mouse")
            {
                var mousePosition = _playerActions.Gameplay.MouseTurretRotation.ReadValue<Vector2>();
                var mouseWorldPosition = UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);
                var vectorToTarget = Utils.CalculateDirectionToTarget(gameObject.transform.position, mouseWorldPosition);
                var rotationToTarget = Utils.CalculateRotationToTarget(vectorToTarget, _tankTurret.GetCurrentDirection());

                _tankTurret.Rotate(rotationToTarget, 0f);
            }

            if (_componentAimIndicator != null)
            {
                _componentAimIndicator.SetDirection(_tankTurret.GetCurrentDirection());
                _componentAimIndicator.SetLength(_tankTurret.GetRange());
                _componentAimIndicator.SetOrigin(_tankBody.gameObject.transform.position);
            }
        }

        private void OnEnable()
        {
            if (_playerActions != null)
            {
                _playerActions.Gameplay.Enable();
            }
        }

        private void OnDisable()
        {
            if (_playerActions != null)
            {
                _playerActions.Gameplay.Disable();
            }
        }

        public void OnChassieMovement(InputAction.CallbackContext context)
        {
            _directionTank = context.ReadValue<Vector2>().normalized;
        }

        public void OnTurretRotation(InputAction.CallbackContext context)
        {
            _directionTurret = context.ReadValue<float>();
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            _tankTurret.Shoot();
        }

        private void FixedUpdate()
        {
            _tankBody.Rotate(_directionTank.x, 0f);
            _tankBody.Move(_directionTank.y);
            _tankTurret.Rotate(_directionTurret, _directionTank.x);
        }
    }

}

