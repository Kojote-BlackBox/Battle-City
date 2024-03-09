using AI;
using Core.Input;
using UnityEngine;

namespace Gameplay.Upgrade
{
    public abstract class Upgrade : MonoBehaviour
    {
        protected abstract void Apply(GameObject gameObject);

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<PlayerController>() == null && collider.gameObject.GetComponent<AIController>()) return;

            Apply(collider.gameObject);

            Destroy(gameObject);
        }
    }
}