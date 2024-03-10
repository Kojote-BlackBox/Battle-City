using Core.Event;
using Core.Reference;
using Core.Track;
using UnityEngine;

namespace Gameplay.Pickup
{
    public class PickupHealth : Pickup
    {
        public GameEvent eventUpdateHealth;
        
        protected override void Apply(GameObject go)
        {
            TrackManager.Instance.playerLives.value++;

            eventUpdateHealth.Raise();
        }
    }
}