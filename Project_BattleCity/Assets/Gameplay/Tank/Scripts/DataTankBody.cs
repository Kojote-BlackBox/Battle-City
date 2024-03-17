using UnityEngine;
using Gameplay.Health;

namespace Gameplay.Tank
{
    [CreateAssetMenu(fileName = "NewDataTankBody", menuName = "Data/Tank/Body")]
    public class DataTankBody : ScriptableObject
    {
        [Header("Health")]
        public DataHealth dataHealth;

        [Header("Animation")]
        public RuntimeAnimatorController animationController;

        [Header("Trail Effect")]
        public bool enableTrail = true;
        public GameObject prefabTrail;
        public int trailSegments = 6;
        public float trailTime = 1.0f;
        public float scaleTrail = 1.0f;

        [Header("Engine - Movement")]
        public float forwardSpeed = 1.6f;
        public float backwardSpeedPercentage = 0.5f;
        public float rotationSpeed = 0.25f;

        [Header("Engine - Audio")]
        public AudioClip audioClipEngine;
        public float pitchIdle;
        public float pitchMovement;
        public float volumeIdle;
        public float volumeMovement;
    }
}
