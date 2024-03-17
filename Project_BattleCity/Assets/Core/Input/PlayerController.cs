using UnityEngine;
using UnityEngine.InputSystem;
using Gameplay.Tank;
using Core.Aim;
using Utilities;
using Core.Event;

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
        private GameObject settingsIngameMenuPanel;

        public GameEvent EventKeyEscapeOnPush;
        public GameEvent EventKeyWOnPush;
        public GameEvent EventKeySOnPush;
        public GameEvent EventKeyAOnPush;
        public GameEvent EventKeyDOnPush;
        public GameEvent EventKeySpaceOnPush;
        public GameEvent EventKeyQOnPush;
        public GameEvent EventKeyEOnPush;
        public GameEvent EventKeyPOnPush;

        #endregion

        #region aim
        private ComponentAimIndicator _componentAimIndicator;
        #endregion

        private void Awake()
        {
            EventKeyEscapeOnPush = GameEventManager.Instance.EventKeyEscapeOnPush;
            if (EventKeyEscapeOnPush == null) {
                Debug.LogError("EventKeyEscapeOnPush event not found");
            }

            EventKeyWOnPush = GameEventManager.Instance.EventKeyWOnPush;
            if (EventKeyWOnPush == null) {
                Debug.LogError("EventKeyWOnPush event not found");
            }

            EventKeySOnPush = GameEventManager.Instance.EventKeySOnPush;
            if (EventKeySOnPush == null) {
                Debug.LogError("EventKeySOnPush event not found");
            }

            EventKeyAOnPush = GameEventManager.Instance.EventKeyAOnPush;
            if (EventKeyAOnPush == null) {
                Debug.LogError("EventKeyAOnPush event not found");
            }

            EventKeyDOnPush = GameEventManager.Instance.EventKeyDOnPush;
            if (EventKeyDOnPush == null) {
                Debug.LogError("EventKeyDOnPush event not found");

            }
            EventKeySpaceOnPush = GameEventManager.Instance.EventKeySpaceOnPush;
            if (EventKeySpaceOnPush == null) {
                Debug.LogError("EventKeySpaceOnPush event not found");
            }

            EventKeyQOnPush = GameEventManager.Instance.EventKeyQOnPush;
            if (EventKeyQOnPush == null) {
                Debug.LogError("EventKeyQOnPush event not found");

            }
            EventKeyEOnPush = GameEventManager.Instance.EventKeyEOnPush;
            if (EventKeyEOnPush == null) {
                Debug.LogError("EventKeyEOnPush event not found");
            }

            EventKeyPOnPush = GameEventManager.Instance.EventKeyPOnPush;
            if (EventKeyPOnPush == null) {
                Debug.LogError("EventKeyPOnPush event not found");
            }

            settingsIngameMenuPanel = GameObject.Find("UI/SettingsIngameMenuPanel");
            if (settingsIngameMenuPanel == null) {
                Debug.LogError("SettingsIngameMenuPanel wurde nicht gefunden.");
            } else {
                settingsIngameMenuPanel.SetActive(false);
            }

            _playerActions = new InputMapping();

            _tankBody = gameObject.GetComponentInChildren<TankBody>();
            _tankTurret = gameObject.GetComponentInChildren<TankTurret>();
            _componentAimIndicator = gameObject.GetComponentInChildren<ComponentAimIndicator>();

            _playerActions.bindingMask = InputBinding.MaskByGroup("Mouse"); // Note: You can change the input schema here!

            _playerActions.Gameplay.Shoot.started += ctx => OnShoot(ctx);
            _playerActions.Gameplay.TurretRotation.performed += ctx => OnTurretRotation(ctx);
            _playerActions.Gameplay.ChassieMovement.performed += ctx => OnChassieMovement(ctx);
            _playerActions.Gameplay.MuteShortCut.performed += ctx => OnMute(ctx);

            _playerActions.Gameplay.TurretRotation.canceled += ctx => _directionTurret = 0f;
            _playerActions.Gameplay.ChassieMovement.canceled += ctx => _directionTank = Vector2.zero;
            _playerActions.Gameplay.MenuPause.started += ctx => TogglePauseMenu();
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

        private float currentTime;

        public void TogglePauseMenu() {
            if (Time.timeScale != 0) {
                currentTime = Time.timeScale;
                Time.timeScale = 0;
                if (settingsIngameMenuPanel != null) {
                    settingsIngameMenuPanel.SetActive(true);
                }
            } else {
                Time.timeScale = currentTime;
                if (settingsIngameMenuPanel != null) {
                    settingsIngameMenuPanel.SetActive(false);
                }
            }
        }

        private void OnEnable()
        {
            if (_playerActions != null)
            {
                _playerActions.Gameplay.Enable();
            }

           // Registrieren der HandlePauseToggle-Methode als Listener
            //EventKeyEscapeOnPush.RegisterListener(HandlePauseToggleEvent);
        }

        private void OnDisable()
        {
            if (_playerActions != null)
            {
                _playerActions.Gameplay.Disable();
            }

            // Deregistrieren der HandlePauseToggle-Methode als Listener
            //EventKeyEscapeOnPush.UnregisterListener(HandlePauseToggleEvent);
        }

        public void OnMute(InputAction.CallbackContext context) {
            EventKeyPOnPush.Raise();
        }

        public void OnChassieMovement(InputAction.CallbackContext context)
        {
            // TODO
            //EventKeyWOnPush.Raise();
            //EventKeySOnPush.Raise();
            //EventKeyAOnPush.Raise();
            //EventKeyDOnPush.Raise();

            _directionTank = context.ReadValue<Vector2>().normalized;
        }

        public void OnTurretRotation(InputAction.CallbackContext context)
        {
            // TODO
            //EventKeyQOnPush.Raise();
            //EventKeyEOnPush.Raise();
            _directionTurret = context.ReadValue<float>();
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            EventKeySpaceOnPush.Raise();
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

