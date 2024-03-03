using UnityEngine;

namespace Gameplay.Tank
{
    public class ComponentRotation : MonoBehaviour
    {
        #region constants
        public const float DIRECTION_ROTATE_LEFT = -1.0f;
        public const float DIRECTION_ROTATE_RIGHT = 1.0f;
        #endregion

        #region rotation
        [Header("Rotation")]
        [Tooltip("How fast the gameobject is rotating.")]
        public float rotationTime;
        [Tooltip("Whether the rotation is propagated to its children.")]
        public bool propagateRotation;
        #endregion

        #region direction
        protected Vector2 _currentDirection;
        protected Vector2 _inputDirection;
        protected float _inputSum = 0.0f;
        protected float _rotationDegree = 0.0f;
        protected float _stepRotation = 22.5f;
        #endregion

        #region children
        protected ComponentRotation[] _children;
        #endregion

        virtual protected void Awake()
        {
            _inputDirection = Vector2.up;
            _currentDirection = Vector2.up;

            _children = GetComponentsInChildren<ComponentRotation>();
        }

        virtual protected void Start()
        {

        }

        virtual public void Rotate(float directionInput, float rotationModifier)
        {
            _inputDirection.x = directionInput;

            UpdateDirection(rotationModifier);

            if (_children != null && propagateRotation)
            {
                foreach (var child in _children)
                {
                    if (child == this) continue;
                    child.Rotate(directionInput, rotationTime);
                }
            }
        }

        virtual protected void UpdateDirection(float rotationModifier)
        {
            _inputSum += _inputDirection.x * (Time.deltaTime * 90.0f) * (1.0f - rotationTime + rotationModifier);
            if (_inputSum > _stepRotation)
            {
                _inputSum = 0.0f;
                _rotationDegree += _stepRotation;
            }
            else if (_inputSum < -_stepRotation)
            {
                _inputSum = 0.0f;
                _rotationDegree -= _stepRotation;
                if (_rotationDegree < 0.0f)
                {
                    _rotationDegree += 360.0f;
                }
            }
            _rotationDegree %= 360.0f;

            _currentDirection = Quaternion.AngleAxis(_rotationDegree, -Vector3.forward) * Vector2.up;
        }

        public Vector2 GetCurrentDirection()
        {
            return _currentDirection;
        }

        public Vector2 GetCurrentInputDirection()
        {
            return _inputDirection;
        }

        public float GetRotationTime()
        {
            return rotationTime;
        }

        public Quaternion GetCurrentRotation()
        {
            return Quaternion.Euler(0f, 0f, _rotationDegree);
        }
    }


}

