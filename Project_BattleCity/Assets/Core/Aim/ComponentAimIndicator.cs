using UnityEngine;

namespace Core.Aim
{
    public class ComponentAimIndicator : MonoBehaviour
    {
        #region appearance
        private Sprite _spriteIndicator;
        #endregion

        #region position
        private Vector3 _direction;
        private float _length;
        private Vector3 _origin;
        #endregion

        #region indicator
        private GameObject _gameObjectIndicator;
        #endregion

        private void Start()
        {
            if (_gameObjectIndicator != null) return;

            _gameObjectIndicator = new GameObject
            {
                name = "AimIndicator"
            };

            // TODO: load sprite indicator

            _spriteIndicator = Resources.Load<Sprite>("Indicators/Cursors/Cursor");

            var renderer = _gameObjectIndicator.AddComponent<SpriteRenderer>();
            renderer.sprite = _spriteIndicator;
        }

        void Update()
        {
            if (_gameObjectIndicator == null) return;

            _gameObjectIndicator.transform.position = _origin + (_direction * _length);
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction;
        }

        public void SetLength(float length)
        {
            if (length <= 0f)
            {
                return;
            }

            _length = length;
        }

        public void SetOrigin(Vector3 origin)
        {
            _origin = origin;
        }
    }
}

