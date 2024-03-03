using UnityEditor.Animations;
using UnityEngine;
using Gameplay.Projectile;
using Core.Event;

namespace Gameplay.Tank
{
    [CreateAssetMenu(fileName = "NewDataTankTurret", menuName = "Data/Tank/Turret")]
    public class DataTankTurret : ScriptableObject
    {
        [Header("Projectile")]
        public DataShell dataShell;

        [Header("Muzzle")]
        public float offsetFactorMuzzle = 0.5f;
        public ParticleSystem effectMuzzleFlash;

        [Header("Barrel")]
        public float reloadTime;
        public GameEventAudio eventAudioShot;

        [Header("Travel")]
        public float rotationSpeed;

        [Header("Animation")]
        public AnimatorController animationController;
    }
}

