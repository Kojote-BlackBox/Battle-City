using Core.Reference;
using UnityEngine;
using Core.Event;

namespace Gameplay.Score
{
    public class AddsToScore : MonoBehaviour
    {
        [Header("Tracking")]
        public ReferenceInt referencePlayerScore;

        [Header("Events")]
        public GameEvent eventUpdateScore;

        [Header("Score")]
        public int amount;

        private void Awake()
        {
            eventUpdateScore.Raise();
        }

        public void AddToScore()
        {
            if (referencePlayerScore == null) return;

            referencePlayerScore.value += amount;

            eventUpdateScore.Raise();
        }
    }
}
