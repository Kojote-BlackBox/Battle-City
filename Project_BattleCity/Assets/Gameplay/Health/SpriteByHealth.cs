using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Health
{
    [Serializable]
    public class HealthSprite
    {
        [SerializeField] public Sprite sprite;
        [SerializeField] public int health;
    }

    public class SpriteByHealth : MonoBehaviour
    {
        [Header("Components")]
        public SpriteRenderer spriteRenderer;
        public ComponentHealth healthComponent;

        [Header("Sprites")]
        public List<HealthSprite> healthSprites;
        private Dictionary<int, Sprite> _spritesByHealth;



        public void Awake()
        {
            _spritesByHealth = new Dictionary<int, Sprite>();

            foreach (var healthSprite in healthSprites)
            {
                _spritesByHealth.Add(healthSprite.health, healthSprite.sprite);
            }
        }

        public void UpdateSpriteByHealth()
        {
            var health = healthComponent.currentHealth;

            if (health <= 0) return;

            if (_spritesByHealth.ContainsKey(health))
            {
                spriteRenderer.sprite = _spritesByHealth[health];
            }
        }
    }
}
