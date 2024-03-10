using Gameplay.Health;
using Core.Reference;
using Gameplay.Tank;
using UnityEngine;

namespace Gameplay.Pickup
{
    public class PickupTimer : Pickup
    {
        [Header("Timer")]
        public float timerDuration;
        public Color timerColor;

        [Header("Enemies")]
        public ReferenceGameObjects enemies;

        protected override void Apply(GameObject go)
        {
            foreach (var enemy in enemies.activeGameObjects)
            {
               /* var mc = enemy.GetComponent<TankMovementComponent>();
                var sc = enemy.GetComponent<TankShootComponent>();
                var hc = enemy.GetComponent<ComponentHealth>();

                mc.Deactivate(timerDuration);
                sc.Deactivate(timerDuration);

                hc.ChangeColorForDuration(timerDuration, timerColor);*/
            }
        }
    }
}