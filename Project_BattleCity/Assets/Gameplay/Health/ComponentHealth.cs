using System.Collections;
using Core.Reference;
using Core.Event;
using UnityEngine;
using Core.Tag;
using Core;
using Gameplay.Tank;
using System.Drawing;
using Core.Track;
using UnityEngine.Events;

namespace Gameplay.Health
{
    [RequireComponent(typeof(ComponentTags))]
    public class ComponentHealth : MonoBehaviour
    {
       // private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private SpriteRenderer[] _childrenSpriteRenderers;
        /*private static readonly int Health = Animator.StringToHash("Health");*/

        //[HideInInspector] public ComponentDataHealth healthDataComponent;

        /*private bool _hasShield;
        private float _shieldDurationRemaining;*/
        private float _colorDurationRemaining;

        /*[Header("Appearance")]
        public SpriteByHealth spriteByHealthComponent;*/

        #region data
        [Header("Data")]
        public DataHealth dataHealth;
        public int currentHealth;
        public GameEvent onHealthChanged;
        #endregion

        #region death
        [Header("Death")]
        //public AK.Wwise.Event destroySound;
        public GameEvent onDeathEvent;
        public GameObject remainsToSpawn;
        #endregion

        private void Awake() {
            var componentTags = GetComponentInChildren<ComponentTags>();
            if (componentTags != null ) {
                componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagHealth));
            }

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _childrenSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            if (dataHealth == null) return;

            currentHealth = dataHealth.health;
        }

        private void Update()
        {
            /*_shieldDurationRemaining -= Time.deltaTime;
            _colorDurationRemaining -= Time.deltaTime;

            if (!(_shieldDurationRemaining <= 0.0f)) return;

            _hasShield = false;
            _shieldDurationRemaining = 0.0f;*/
        }

        public void ModifyHealth(int amount, bool byPlayer)
        {
            if (dataHealth == null) {
                Debug.LogError("Gameobject " + gameObject + " has not health data set!");

                return;
            }

            currentHealth += amount;

            Debug.Log("modified health by " + amount + " - current health: " + currentHealth);

            // Event auslösen, wenn die Gesundheit geändert wird
            onHealthChanged?.Raise();

            if (currentHealth > dataHealth.health)
                currentHealth = dataHealth.health;

            dataHealth.eventAudioHit?.Play(transform.position);

            if (currentHealth > 0) return; //|| _hasShield) return;

            if (TrackManager.Instance.player.gameObject != null && TrackManager.Instance.player.gameObject == gameObject)
                TrackManager.Instance.player.gameObject = null;
            else if (TrackManager.Instance.enemies.activeGameObjects.Contains(gameObject))
            {
                TrackManager.Instance.enemies.activeGameObjects.Remove(gameObject);
                TrackManager.Instance.enemies.destroyedGameObjects++;
            }

            if (dataHealth.eventAudioDestroyed != null)
            {
                dataHealth.eventAudioDestroyed.Play(transform.position);
            }

            var delay = 0.0f;

            if (remainsToSpawn != null)
                StartCoroutine(WaitAndSpawnRemains(remainsToSpawn, gameObject, TrackManager.Instance.remains, delay));
            else
                Destroy(gameObject, delay);

        }

        public void Reset() {
            if (dataHealth == null) {
                Debug.LogError("Gameobject " + gameObject + " has not health data set!");

                return;
            }

            currentHealth = dataHealth.health;
            onHealthChanged?.Raise();
        }

        /*public void ApplyShield(float duration)
        {
            _hasShield = true;
            _shieldDurationRemaining = duration;
        }*/

        public void ChangeColorForDuration(float duration, UnityEngine.Color color)
        {
            if (_spriteRenderer != null) _spriteRenderer.color = color;

            foreach (var spriteRenderer in _childrenSpriteRenderers)
            {
                spriteRenderer.color = color;
            }

            Invoke(nameof(ResetColorChange), duration);
        }

        private void ResetColorChange() {
            if (_spriteRenderer != null) _spriteRenderer.color = UnityEngine.Color.white;

            foreach (var spriteRenderer in _childrenSpriteRenderers) {
                spriteRenderer.color = UnityEngine.Color.white;
            }
        }

        private static IEnumerator WaitAndSpawnRemains(GameObject remainsToSpawn, GameObject parent, ReferenceGameObjects remainsTracker, float waitForSeconds)
        {
            var rotation = parent.transform.rotation;

            var componentRotation = parent.GetComponent<ComponentRotation>();
            if (componentRotation != null)
            {
                rotation = componentRotation.GetCurrentRotation();
            }

            var instancedGameObjectRemains = Instantiate(remainsToSpawn, parent.transform.position, rotation);

            if (remainsTracker == null) yield break;

            remainsTracker.activeGameObjects.Add(instancedGameObjectRemains);
            remainsTracker.totalGameObjects++;

            foreach (Transform child in parent.transform)
            {
                child.gameObject.SetActive(false);
            }

            Destroy(parent, waitForSeconds);
        }

        private void OnDestroy()
        {
            if (onDeathEvent != null) onDeathEvent.Raise();
        }
    }
}
