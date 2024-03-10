using System.Linq;
using Gameplay.Health;
using Core.Reference;
using UnityEngine;

namespace Gameplay.Pickup
{
    public class PickupShovel : Pickup
    {
        [Header("Tracking")] public ReferenceRelatedGameObjects bunker;

        protected override void Apply(GameObject go)
        {
            if (bunker == null) return;

            foreach (var healthComponent in bunker.relatedGameObjects.Select(brickWall => brickWall.GetComponent<ComponentHealth>()))
            {
                healthComponent.ModifyHealth(healthComponent.dataHealth.health - healthComponent.currentHealth, false);
            }
        }
    }
}