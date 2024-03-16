using Core.Event;
using Core.Track;
using UnityEngine;

namespace Gameplay.Pickup
{
    public class PickupHealth : Pickup
    {
        protected override void Apply(GameObject go)
        {
            TrackManager.Instance.playerLives.value += 1;
            GameEventManager.Instance.updateLive?.Raise();
        }
    }
}