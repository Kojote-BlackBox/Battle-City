using System.Collections;
using Core.Reference;
using Core.Event;
using UnityEngine;
using Core.Tag;
using Core;
using Gameplay.Tank;

namespace Gameplay.Health
{
    [RequireComponent(typeof(ComponentTags))]
    public class ComponentHealth : MonoBehaviour
    {
        /*private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private SpriteRenderer[] _childrenSpriteRenderers;
        private static readonly int Health = Animator.StringToHash("Health");*/

        //[HideInInspector] public ComponentDataHealth healthDataComponent;
        [HideInInspector] public int currentHealth;

        /*private bool _hasShield;
        private float _shieldDurationRemaining;
        private float _colorDurationRemaining;*/

        /*[Header("Appearance")]
        public SpriteByHealth spriteByHealthComponent;*/

        #region data
        [Header("Data")]
        public DataHealth dataHealth;
        #endregion

        #region death
        [Header("Death")]
        //public AK.Wwise.Event destroySound;
        public GameEvent onDeathEvent;
        public GameObject remainsToSpawn;
        #endregion

        #region tracking
        [Header("Tracking")]
        public ReferenceGameObjects enemies;
        public ReferenceGameObjects remains;
        public ReferenceGameObject player;
        public ReferenceRelatedGameObjects bunker;
        #endregion

        private void Awake() {
            var componentTags = GetComponentInChildren<ComponentTags>();
            if (componentTags != null ) {
                componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagHealth));
            }
        }

        private void Start()
        {
            /*healthDataComponent = GetComponent<ComponentDataHealth>();
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _childrenSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();*/

            //gameObject.tag = "Damageable";


            if (dataHealth == null) return;

            currentHealth = dataHealth.health;

            /*if (_animator == null) return;

            _animator.logWarnings = false;
            _animator.SetInteger(Health, currentHealth);*/
        }

        private void Update()
        {
            /*_shieldDurationRemaining -= Time.deltaTime;
            _colorDurationRemaining -= Time.deltaTime;

            if (_colorDurationRemaining <= 0.0f)
                ChangeColorForDuration(_colorDurationRemaining, Color.white);

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

            if (currentHealth > dataHealth.health)
                currentHealth = dataHealth.health;

            /*if (spriteByHealthComponent != null)
                spriteByHealthComponent.UpdateSpriteByHealth();

            if (_animator != null)
                _animator.SetInteger(Health, currentHealth);*/

            dataHealth.eventAudioHit?.Play(transform.position);

            if (currentHealth > 0) return; //|| _hasShield) return;

            /*if (byPlayer)
            {
                var scoreComponent = gameObject.GetComponent<AddsToScore>();

                if (scoreComponent != null)
                {
                    scoreComponent.AddToScore();
                }
            }*/

            if (player != null && player.gameObject == gameObject)
                player.gameObject = null;
            else if (enemies.activeGameObjects.Contains(gameObject))
            {
                enemies.activeGameObjects.Remove(gameObject);
                enemies.destroyedGameObjects++;
            }

            if (dataHealth.eventAudioDestroyed != null)
            {
                dataHealth.eventAudioDestroyed.Play(transform.position);
            }

            var delay = 0.0f;

            if (remainsToSpawn != null)
                StartCoroutine(WaitAndSpawnRemains(remainsToSpawn, gameObject, remains, delay));
            else
                Destroy(gameObject, delay);
        }

        public void Reset() {
            if (dataHealth == null) {
                Debug.LogError("Gameobject " + gameObject + " has not health data set!");

                return;
            }

            currentHealth = dataHealth.health;
        }

        /*public void ApplyShield(float duration)
        {
            _hasShield = true;
            _shieldDurationRemaining = duration;
        }*/

        /*public void ChangeColorForDuration(float duration, Color color)
        {
            _colorDurationRemaining = duration;

            if (_spriteRenderer != null) _spriteRenderer.color = color;

            foreach (var spriteRenderer in _childrenSpriteRenderers)
            {
                spriteRenderer.color = color;
            }
        }*/

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
