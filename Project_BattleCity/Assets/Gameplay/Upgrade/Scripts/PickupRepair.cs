using System.Linq;
using Gameplay.Health;
using Core.Reference;
using UnityEngine;
using Core.Tag;
using Core;
using Gameplay.Tank;

namespace Gameplay.Pickup
{
    public class PickupRepair : Pickup
    {
        protected override void Apply(GameObject go)
        {
            if (go == null) return;

            var componentTags = go.GetComponent<ComponentTags>();
            if (componentTags == null) return;

            if (!componentTags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagHealth))) return;

            var componentHealth = go.GetComponent<ComponentHealth>();
            componentHealth.Reset();

            /*if (bunker == null) return;

            foreach (var healthComponent in bunker.relatedGameObjects.Select(brickWall => brickWall.GetComponent<ComponentHealth>()))
            {
                healthComponent.ModifyHealth(healthComponent.dataHealth.health - healthComponent.currentHealth, false);
            }*/
        }
    }
}