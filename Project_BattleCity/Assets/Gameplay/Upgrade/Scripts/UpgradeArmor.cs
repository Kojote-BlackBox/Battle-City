using Core.Tag;
using Core;
using Gameplay.Tank;
using UnityEngine;

namespace Gameplay.Upgrade
{
    public class UpgradeArmor : Upgrade
    {
        #region upgrade
        [SerializeField] private DataTankBody _dataTankBody;
        #endregion
        /*[Header("Shield")]
        public Color shieldColor;
        public float shieldDurationInSeconds;

        protected override void Apply(GameObject go)
        {
            if (go == null) return;

            var hc = go.GetComponent<ComponentHealth>();

            if (hc == null) return;

            //hc.ApplyShield(shieldDurationInSeconds);
            //hc.ChangeColorForDuration(shieldDurationInSeconds, shieldColor);
        }*/
        protected override void Apply(GameObject gameObjectApply) {
            if (gameObjectApply == null) return;

            var componentTags = gameObjectApply.GetComponent<ComponentTags>();
            if (componentTags == null) return;

            if (!componentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTank))) return;

            var componentTank = gameObjectApply.GetComponent<Tank.Tank>();
            componentTank?.ChangeTankBody(_dataTankBody);
        }
    }
}