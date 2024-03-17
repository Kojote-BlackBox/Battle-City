using Gameplay.Health;
using UnityEngine;
using Core.Track;
using Core.Tag;
using Core;
using System.Collections.Generic;

namespace Gameplay.Pickup
{
    public class PickupGrenade : Pickup
    {
        protected override void Apply(GameObject gameObjectToApply)
        {
            if (gameObjectToApply == null) return;

            var tags = gameObjectToApply.GetComponent<ComponentTags>();
            if (tags == null)
            {
                Debug.LogError("tag not set on apply pickup");

                return;
            }

            if (tags.ContainsTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagFriendly)))
            {
                Debug.Log("Enemies active: " + TrackManager.Instance.enemies.activeGameObjects.Count);

                for (var index = TrackManager.Instance.enemies.activeGameObjects.Count - 1; index >= 0; --index)
                {
                    var componentHealth = TrackManager.Instance.enemies.activeGameObjects[index].GetComponent<ComponentHealth>();

                    componentHealth.ModifyHealth(-componentHealth.currentHealth);
                }

                Debug.Log("Enemies active: " + TrackManager.Instance.enemies.activeGameObjects.Count);
            }
            else
            {
                Debug.Log("Allies active: " + TrackManager.Instance.allies.activeGameObjects.Count);

                for (var index = TrackManager.Instance.allies.activeGameObjects.Count - 1; index >= 0; --index)
                {
                    var componentHealth = TrackManager.Instance.allies.activeGameObjects[index].GetComponent<ComponentHealth>();

                    componentHealth.ModifyHealth(-componentHealth.currentHealth);
                }

                var componentHealthPlayer = TrackManager.Instance.player.gameObject.GetComponent<ComponentHealth>();
                componentHealthPlayer.ModifyHealth(-componentHealthPlayer.currentHealth);

                Debug.Log("Allies active: " + TrackManager.Instance.allies.activeGameObjects.Count);
            }
        }
    }
}
