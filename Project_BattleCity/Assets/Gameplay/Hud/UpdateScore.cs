using Core.Reference;
using TMPro;
using UnityEngine;

namespace Gameplay.Hud
{
    public class UpdateScore : MonoBehaviour
    {
        [Header("Score")]
        public ReferenceInt score;
        public GameObject scoreObject;

        private TextMeshProUGUI _scoreText;
    
        private void Awake()
        {
            _scoreText = scoreObject.GetComponent<TextMeshProUGUI>();
        }

        public void ChangeScore()
        {
            if (_scoreText == null) return;

            _scoreText.text = "Score: " + score.value;
        }
    }
}
