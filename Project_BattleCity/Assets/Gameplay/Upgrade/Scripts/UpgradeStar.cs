using Core;
using Core.Tag;
using Gameplay.Tank;
using UnityEngine;

namespace Gameplay.Upgrade
{
    public class UpgradeStar : Upgrade
    {
        #region upgrade
        [SerializeField] private DataTankTurret _dataTankTurret;
        #endregion

        protected override void Apply(GameObject gameObjectApply)
        {
            if (gameObjectApply == null) return;

            var componentTags = gameObjectApply.GetComponent<ComponentTags>();
            if (componentTags == null) return;

            if (!componentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagTank))) return;

            var componentTank = gameObjectApply.GetComponent<Tank.Tank>();
            componentTank?.ChangeShell(_dataTankTurret.dataShell);
            componentTank?.ChangeTankTurret(_dataTankTurret);
        }
    }
}