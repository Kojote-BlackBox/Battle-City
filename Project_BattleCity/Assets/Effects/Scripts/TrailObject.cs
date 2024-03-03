using UnityEngine;

namespace Effect.Trail
{
    public class TrailObject : MonoBehaviour
    {
        #region appearance
        [Header("Appearance")]
        public new SpriteRenderer renderer;
        public Color startColor, endColor;
        private float _displayTime;
        private float _timeDisplayed;
        #endregion

        #region reusability
        private bool _inUse;
        #endregion

        #region effects
        private ComponentEffectTrail _componentTrailEffect;
        #endregion

        private void Awake()
        {
            renderer.enabled = false;
        }

        private void Update()
        {
            if (!_inUse) return;

            _timeDisplayed += Time.deltaTime;

            renderer.color = Color.Lerp(startColor, endColor, _timeDisplayed / _displayTime);

            if (!(_timeDisplayed >= _displayTime)) return;

            _componentTrailEffect.RemoveTrailObject(gameObject);
            _inUse = false;
            renderer.enabled = false;
        }

        public void Initiate(float displayTime, Sprite spriteTrail, Vector2 position, Quaternion rotation, ComponentEffectTrail componentEffectTrail)
        {
            _displayTime = displayTime;
            transform.position = position;
            transform.rotation = rotation;
            _timeDisplayed = 0;
            _componentTrailEffect = componentEffectTrail;
            _inUse = true;
            renderer.enabled = true;

            if (spriteTrail != null) renderer.sprite = spriteTrail;
        }
    }
}
