using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Core;
using Core.Tag;

namespace Gameplay.Bunker
{
    public class Bunker : MonoBehaviour
    {
        #region bunker
        public DataBunker dataBunker;

        public bool isBunkerFriendly { get; private set; }
        #endregion

        #region capping
        private float cappingProgressCurrent = 0.0f;
        private List<GameObject> countCappingObjects = new List<GameObject>();
        private List<GameObject> countDefendingObjects = new List<GameObject>();
        #endregion

        #region light
        private Sprite spriteBunkerLight;
        private SpriteRenderer spriteRendererBunkerLight;
        #endregion

        void Start()
        {
            initializeBunker();
            initializeBunkerLight();

            UpdateSpriteBunker();
        }

        void initializeBunker()
        {
            isBunkerFriendly = dataBunker.isFriendly;

            var circleCollider = GetComponent<CircleCollider2D>();
            circleCollider.enabled = true;

            var spriteRendererBunker = GetComponent<SpriteRenderer>();
            spriteRendererBunker.sprite = dataBunker.spriteBunker;
        }

        void initializeBunkerLight()
        {
            spriteBunkerLight = dataBunker.spriteBunkerLight;

            var lightGameObject = new GameObject("Light");
            lightGameObject.transform.parent = transform;
            lightGameObject.transform.localPosition = new Vector3(dataBunker.localPositionBunkerLight.x, dataBunker.localPositionBunkerLight.y, 0);

            var spriteRendererLight = lightGameObject.AddComponent<SpriteRenderer>();
            spriteRendererLight.sprite = spriteBunkerLight;
            spriteRendererLight.sortingOrder = (int)LayerType.ObjectOverlay;

            this.spriteRendererBunkerLight = spriteRendererLight;
        }

        void Update()
        {
            if (IsBeingCapped())
            {
                UpdateCaptureTime();
                UpdateSpriteLight();
            }
            else
            {
                if (cappingProgressCurrent > 0)
                {
                    cappingProgressCurrent = 0;
                    UpdateSpriteLight();
                }
            }

            if (IsCaptured())
            {
                SwitchOwnership();
                UpdateSpriteBunker();
            }
        }

        void UpdateSpriteBunker()
        {
            if (spriteRendererBunkerLight == null)
            {
                return;
            }

            spriteRendererBunkerLight.color = isBunkerFriendly ? GameConstants.colorFriendly : GameConstants.colorEnemy;
        }

        void UpdateSpriteLight()
        {
            // Berechne den Lerp-Faktor basierend auf dem Einnahmefortschritt
            float lerpFactor = cappingProgressCurrent / dataBunker.capTimeMax;

            // Bestimme die Start- und Endfarbe basierend auf dem aktuellen Zustand und Einnahmeprozess
            Color startColor = isBunkerFriendly ? GameConstants.colorFriendly : GameConstants.colorEnemy;
            Color endColor = isBunkerFriendly ? GameConstants.colorEnemy : GameConstants.colorFriendly;

            // Lerp die Farbe von der Startfarbe zur Endfarbe basierend auf dem Einnahmefortschritt
            Color currentColor = Color.Lerp(startColor, endColor, lerpFactor);
            spriteRendererBunkerLight.color = currentColor;
        }

        bool IsBeingCapped()
        {
            return countCappingObjects.Count > 0;
        }

        void UpdateCaptureTime()
        {
            if (countCappingObjects.Count > 0 && countDefendingObjects.Count == 0)
            {
                cappingProgressCurrent += Time.deltaTime;

                math.clamp(cappingProgressCurrent, 0, dataBunker.capTimeMax);
            }
        }

        bool IsCaptured()
        {
            return cappingProgressCurrent >= dataBunker.capTimeMax;
        }

        void SwitchOwnership()
        {
            isBunkerFriendly = !isBunkerFriendly;

            countCappingObjects.Clear();
            countDefendingObjects.Clear();
            dataBunker.eventCheckGameState.Raise();

            Debug.Log("bunker captured: " + (isBunkerFriendly ? "friendly" : "enemy"));
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            var componentTags = collision.GetComponentInChildren<ComponentTags>();
            if (componentTags == null) return;

            if (!componentTags.ContainsTag(dataBunker.tagTank)) return;

            if (componentTags.ContainsTag(dataBunker.tagEnemy))
            {
                countCappingObjects.Add(collision.gameObject);
                Debug.Log("bunker is being capped by: " + collision.gameObject.name);

                return;
            }

            countDefendingObjects.Add(collision.gameObject);
            Debug.Log("bunker is being defended by: " + collision.gameObject.name);

        }

        void OnTriggerExit2D(Collider2D collision)
        {
            var componentTags = collision.GetComponentInChildren<ComponentTags>();
            if (componentTags == null) return;

            if (!componentTags.ContainsTag(dataBunker.tagTank)) return;

            if (componentTags.ContainsTag(dataBunker.tagEnemy))
            {
                countCappingObjects.Remove(collision.gameObject);
                Debug.Log("bunker is not being capped by anymore: " + collision.gameObject.name);

                return;
            }

            countDefendingObjects.Remove(collision.gameObject);
            Debug.Log("bunker is not being defended by anymore: " + collision.gameObject.name);
        }
    }
}
