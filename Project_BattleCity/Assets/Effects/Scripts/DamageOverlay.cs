using System.Collections.Generic;
using Gameplay.Health;
using Gameplay.Tank;
using UnityEngine;

namespace Effects.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DamageOverlay : MonoBehaviour
    {
        public ComponentHealth healthComponent;
        //public TankMovementComponent movementComponent;

        [Header("LeftOverlays")] public List<Sprite> leftSprites;
        [Header("RightOverlays")] public List<Sprite> rightSprites;
        [Header("UpOverlays")] public List<Sprite> upSprites;
        [Header("DownOverlays")] public List<Sprite> downSprites;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            /*if (movementComponent.forwardDirection.x > 0.01f)
            {
                _renderer.sprite = rightSprites[healthComponent.currentHealth - 1];
            }
            if (movementComponent.forwardDirection.x < 0.01f)
            {
                _renderer.sprite = leftSprites[healthComponent.currentHealth - 1];
            }
            if (movementComponent.forwardDirection.y > 0.01f)
            {
                _renderer.sprite = upSprites[healthComponent.currentHealth - 1];
            }
            if (movementComponent.forwardDirection.y < 0.01f)
            {
                _renderer.sprite = downSprites[healthComponent.currentHealth - 1];
            }*/
        }
    }
}
