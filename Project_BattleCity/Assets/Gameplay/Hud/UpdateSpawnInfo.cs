using Core.Reference;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Hud
{
    public class UpdateSpawnInfo : MonoBehaviour
    {
        [Header("SpawnInfo")]
        public ReferenceSpawnInfo spawnInfo;
        public GameObject spawnInfoTextObject;
        public GameObject spawnInfoImageObject;
        
        private TextMeshProUGUI _spawnInfoText;
        private Image _spawnInfoImage;

        private void Awake()
        {
            _spawnInfoText = spawnInfoTextObject.GetComponent<TextMeshProUGUI>();
            _spawnInfoImage = spawnInfoImageObject.GetComponent<Image>();
        }
        
        public void ChangeSpawnInfo()
        {
            if (spawnInfo == null) return;
            
            if(_spawnInfoImage != null)
                _spawnInfoImage.sprite = spawnInfo.image;
            
            if (_spawnInfoText != null)
                _spawnInfoText.text = spawnInfo.label;
        }
    }
}
