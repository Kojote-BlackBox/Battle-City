using Core.Event;
using UnityEngine;

namespace Gameplay.Projectile
{
    [CreateAssetMenu(fileName = "NewDataShell", menuName = "Data/Projectile/Shell")]
    public class DataShell : ScriptableObject
    {
        [Header("Shell")]
        public GameObject prefabShell;

        [Header("Effects")]
        public GameObject effectHit;
        public float effectHitDestructionDelay;

        [Header("Damage")]
        public int penetration;
        public int damage;
        public int damageRadius;

        [Header("Travel")]
        public int distanceMax;
        public int velocity;

        [Header("Lifetime")]
        public float lifetimeInSecondsMax;

        [Header("Shadow")]
        public bool enableShadow;
        public GameObject prefabShadow;

        [Header("Scene")]
        public string gameObjectNameParent;
    }
}

