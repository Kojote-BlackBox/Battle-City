using System.Collections;
using Core.Reference;
using Core.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gameplay.Bunker;

namespace Core.GameState
{
    public class GameStateSystem : MonoBehaviour
    {
        [Header("Huds")]
        public GameObject victoryPanel;
        public GameObject defeatPanel;

        [Header("Transition")]
        public float transitionDelay;
        public string menuSceneName;
        public string nextLevelSceneName;

        [Header("Pausing")]
        public float timeScaleFactor;

        [Header("Tracking")]
        public ReferenceGameObjects enemies;
        public ReferenceGameObject player;
        public ReferenceGameObject upgrade;
        public ReferenceRelatedGameObjects bunker;
        public ReferenceGameObjects remains;
        public ReferenceInt score;
        public ReferenceInt lives;

        [Header("Events")]
        public GameEvent eventSpawnPlayer;
        public GameEvent eventUpdateHealth;
        public GameEvent eventGameStateInitialized;

        private void Start()
        {
            //Reset();

            eventGameStateInitialized.Raise();
        }

        public void CheckState()
        {
            Debug.Log("check game state");

            if (player.gameObject == null && lives.value > 0)
            {
                lives.value--;
                eventSpawnPlayer.Raise();
                eventUpdateHealth.Raise();
            }

            if (lives.value <= 0 || bunker.gameObject == null || !bunker.gameObject.activeSelf)
            {
                Time.timeScale = timeScaleFactor;

                if (defeatPanel != null)
                    defeatPanel.SetActive(true);

                StartCoroutine(WaitAndLoadScene(menuSceneName, transitionDelay * timeScaleFactor));
            }

            if (enemies.destroyedGameObjects < enemies.totalGameObjects) return;

            foreach (var relatedBunker in bunker.relatedGameObjects)
            {
                var bunkerObject = relatedBunker.GetComponent<Bunker>();

                if (!bunkerObject.isBunkerFriendly) return;
            }

            Time.timeScale = timeScaleFactor;
            victoryPanel.SetActive(true);

            StartCoroutine(WaitAndLoadScene(nextLevelSceneName, transitionDelay * timeScaleFactor));
        }

        private static IEnumerator WaitAndLoadScene(string sceneName, float waitForSeconds)
        {
            yield return new WaitForSeconds(waitForSeconds);

            SceneManager.LoadScene(sceneName);
        }

        private void Reset()
        {
            Time.timeScale = 1.0f;
            lives.value = 3;
            score.value = 0;

            if (player.gameObject != null)
            {
                Destroy(player.gameObject);
            }

            if (bunker.gameObject != null)
            {
                Destroy(bunker.gameObject);
            }

            bunker.relatedGameObjects.Clear();

            enemies.totalGameObjects = 0;
            enemies.destroyedGameObjects = 0;
            enemies.activeGameObjects.Clear();
            remains.totalGameObjects = 0;
            remains.destroyedGameObjects = 0;
            remains.activeGameObjects.Clear();

            if (upgrade.gameObject != null)
            {
                Destroy(upgrade.gameObject);
            }

        }

        private void OnDestroy()
        {
            Reset();
        }
    }
}
