using Core.Reference;
using TMPro;
using UnityEngine;

namespace Gameplay.Hud
{
    public class UpdateHealth : MonoBehaviour
    {
        [Header("Health")]
        public ReferenceInt health;
        public GameObject healthObject;

        private TextMeshProUGUI _healthText;
    
        private void Awake()
        {
            _healthText = healthObject.GetComponent<TextMeshProUGUI>();
        }

        public void ChangeHealth()
        {
            if (_healthText == null) return;

            _healthText.text = "Health: " + health.value;
        }
    }
}