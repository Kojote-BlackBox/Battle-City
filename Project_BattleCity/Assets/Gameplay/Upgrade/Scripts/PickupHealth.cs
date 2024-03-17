using Core.Event;
using Core.Track;
using UnityEngine;
using Core.Tag;
using Core;

namespace Gameplay.Pickup
{
    public class PickupHealth : Pickup
    {
        protected override void Apply(GameObject go)
        {
            var tags = go.GetComponent<ComponentTags>();
            if (tags == null)
            {
                Debug.LogError("tag not set on apply pickup");

                return;
            }

            if (tags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagFriendly)))
            {
                TrackManager.Instance.playerLives.value += 1;
            }
            else
            {
                TrackManager.Instance.playerLives.value -= 1;
            }

            GameEventManager.Instance.updateLive?.Raise();
        }
    }
}