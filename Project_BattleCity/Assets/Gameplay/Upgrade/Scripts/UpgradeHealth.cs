using Core.Event;
using Core.Reference;
using UnityEngine;

namespace Gameplay.Upgrade
{
    public class UpgradeHealth : Upgrade
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