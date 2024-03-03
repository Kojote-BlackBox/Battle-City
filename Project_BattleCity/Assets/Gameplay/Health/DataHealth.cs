using Core.Tag;
using UnityEngine;
using Core.Event;

namespace Gameplay.Health
{
    [CreateAssetMenu(fileName = "NewDataHealth", menuName = "Data/Health")]
    public class DataHealth : ScriptableObject
    {
        [Header("Health")]
        public int health;

        [Header("Audio")]
        public GameEventAudio eventAudioDestroyed;
        public GameEventAudio eventAudioHit;
    }
}
