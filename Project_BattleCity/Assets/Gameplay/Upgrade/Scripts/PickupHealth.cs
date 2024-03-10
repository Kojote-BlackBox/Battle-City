using Core.Event;
using Core.Reference;
using UnityEngine;

namespace Gameplay.Pickup
{
    public class PickupHealth : Pickup
    {
        public ReferenceInt health;

        public GameEvent eventUpdateHealth;
        
        protected override void Apply(GameObject go)
        {
            health.value++;
            eventUpdateHealth.Raise();
        }
    }
}