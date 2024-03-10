using AI;
using Core.Input;
using Core.Tag;
using UnityEngine;
using Core;

namespace Gameplay.Pickup
{
    [RequireComponent(typeof(ComponentTags))]
    public abstract class Pickup : MonoBehaviour
    {
        protected void Awake() {
            var componentTags = GetComponent<ComponentTags>();

            componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagPickup));
        }

        protected abstract void Apply(GameObject gameObject);

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<PlayerController>() == null && collider.gameObject.GetComponent<AIController>() == null) return;

            Apply(collider.gameObject);

            Destroy(gameObject);
        }
    }
}