using Gameplay.Health;
using UnityEngine;

namespace Gameplay.Upgrade
{
    public class UpgradeArmor : Upgrade
    {
        [Header("Shield")]
        public Color shieldColor;
        public float shieldDurationInSeconds;

        protected override void Apply(GameObject go)
        {
            if (go == null) return;

            var hc = go.GetComponent<ComponentHealth>();

            if (hc == null) return;

            //hc.ApplyShield(shieldDurationInSeconds);
            //hc.ChangeColorForDuration(shieldDurationInSeconds, shieldColor);
        }
    }
}