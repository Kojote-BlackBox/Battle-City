using Gameplay.Health;
using Core.Reference;
using UnityEngine;

namespace Gameplay.Pickup
{
    public class PickupGrenade : Pickup
    {
        public ReferenceGameObjects enemies;

        protected override void Apply(GameObject go)
        {
            Debug.Log("Enemies active: " + enemies.activeGameObjects.Count);

            for (var index = enemies.activeGameObjects.Count - 1; index >= 0; --index)
            {
                var hc = enemies.activeGameObjects[index].GetComponent<ComponentHealth>();

                hc.ModifyHealth(-hc.currentHealth, true);
            }

            Debug.Log("Enemies active: " + enemies.activeGameObjects.Count);
        }
    }
}
