using Gameplay.Health;
using Core.Reference;
using Gameplay.Tank;
using UnityEngine;
using Core.Tag;
using Core.Track;
using Core;

namespace Gameplay.Pickup
{
    public class PickupTimer : Pickup
    {
        [Header("Timer")]
        public float timerDuration;
        public Color timerColor;

        protected override void Apply(GameObject gameObjectToApply)
        {
            if (gameObjectToApply == null) return;

            var tags = gameObjectToApply.GetComponent<ComponentTags>();
            if (tags == null) {
                Debug.LogError("tag not set on apply pickup");

                return;
            }

            if (tags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagFriendly))) {
                foreach (var enemy in TrackManager.Instance.allies.activeGameObjects) {
                    var mc = enemy.GetComponent<TankBody>();
                    var sc = enemy.GetComponent<TankTurret>();
                    var hc = enemy.GetComponent<ComponentHealth>();
                    
                    mc.DeactiveForDuration(timerDuration);
                    sc.DeactivateForDuration(timerDuration);

                    hc.ChangeColorForDuration(timerDuration, timerColor);
                }
            } else {
                foreach (var enemy in TrackManager.Instance.enemies.activeGameObjects) {
                    var mc = enemy.GetComponent<TankBody>();
                    var sc = enemy.GetComponent<TankTurret>();
                    var hc = enemy.GetComponent<ComponentHealth>();

                    mc.DeactiveForDuration(timerDuration);
                    sc.DeactivateForDuration(timerDuration);

                    hc.ChangeColorForDuration(timerDuration, timerColor);
                }
            }
        }
    }
}