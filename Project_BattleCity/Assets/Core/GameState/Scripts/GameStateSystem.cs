using System.Collections;
using Core.Reference;
using Core.Event;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gameplay.Bunker;
using Core.Track;

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

        [Header("Events")]
        public GameEvent eventSpawnPlayer;
        public GameEvent eventUpdateHealth;
        public GameEvent eventGameStateInitialized;

        [Header("Player")]
        public int initialPlayerLives = 3;

        private void Awake()
        {
            TrackManager.Instance.Reset();
        }

        private void Start()
        {
            TrackManager.Instance.playerLives.value = initialPlayerLives;

            eventGameStateInitialized.Raise();
        }

        public void CheckState()
        {
            Debug.Log("check game state");

            if (TrackManager.Instance.player.gameObject == null && TrackManager.Instance.playerLives.value > 0)
            {
                Debug.Log("Reducing player live!");

                TrackManager.Instance.playerLives.value--;
                eventSpawnPlayer.Raise();
                eventUpdateHealth.Raise();
            }

            if (TrackManager.Instance.playerLives.value <= 0)
            { // If player has no live left
                Debug.Log("Player has no life left!");

                DeclareDefeat();
            }

            var allEnemyBunkersCaptured = true;

            foreach (var relatedBunker in TrackManager.Instance.enemyBunkers.activeGameObjects)
            { // If all enemy bunkers are captured
                if (relatedBunker == null)
                    continue;

                var bunkerObject = relatedBunker.GetComponent<Bunker>();

                if (!bunkerObject.isBunkerFriendly)
                {
                    allEnemyBunkersCaptured = false;

                    break;
                }
            }

            if (allEnemyBunkersCaptured)
            {
                Debug.Log("All enemy bunkers captured!");

                DeclareVictory();
            }

            if (TrackManager.Instance.allyBunkers.gameObject != null)
            { // If player bunker is captured
                Debug.Log("Player bunker captured!");

                var playerBunker = TrackManager.Instance.allyBunkers.gameObject.GetComponent<Bunker>();
                if (!playerBunker.isBunkerFriendly)
                {
                    DeclareDefeat();
                }
            }
        }

        private void DeclareDefeat()
        {
            Debug.Log("Defeat!");

            Time.timeScale = timeScaleFactor;

            if (defeatPanel != null) defeatPanel.SetActive(true);

            if (menuSceneName != "") StartCoroutine(WaitAndLoadScene(menuSceneName, transitionDelay * timeScaleFactor));
        }

        private void DeclareVictory()
        {
            Debug.Log("Victory!");

            Time.timeScale = timeScaleFactor;

            if (victoryPanel != null) victoryPanel.SetActive(true);

            if (nextLevelSceneName != "") StartCoroutine(WaitAndLoadScene(nextLevelSceneName, transitionDelay * timeScaleFactor));
        }

        private static IEnumerator WaitAndLoadScene(string sceneName, float waitForSeconds)
        {
            yield return new WaitForSeconds(waitForSeconds);

            SceneManager.LoadScene(sceneName);
        }

        private void Reset()
        {
            Time.timeScale = 1.0f;
            TrackManager.Instance.playerLives.value = 3;
            //score.value = 0;

            if (TrackManager.Instance.player.gameObject != null)
            {
                Destroy(TrackManager.Instance.player.gameObject);
            }

            foreach (var ally in TrackManager.Instance.allies.activeGameObjects)
            {
                Destroy(ally);
            }

            TrackManager.Instance.allies.totalGameObjects = 0;
            TrackManager.Instance.allies.destroyedGameObjects = 0;
            TrackManager.Instance.allies.activeGameObjects.Clear();

            foreach (var enemy in TrackManager.Instance.enemies.activeGameObjects)
            {
                Destroy(enemy);
            }

            TrackManager.Instance.enemies.totalGameObjects = 0;
            TrackManager.Instance.enemies.destroyedGameObjects = 0;
            TrackManager.Instance.enemies.activeGameObjects.Clear();

            if (TrackManager.Instance.allyBunkers.gameObject != null)
            {
                Destroy(TrackManager.Instance.allyBunkers.gameObject);
            }

            foreach (var alliedBunker in TrackManager.Instance.allyBunkers.relatedGameObjects)
            {
                Destroy(alliedBunker);
            }

            TrackManager.Instance.allyBunkers.relatedGameObjects.Clear();

            foreach (var spawns in TrackManager.Instance.spawns.activeGameObjects)
            {
                Destroy(spawns);
            }

            TrackManager.Instance.spawns.totalGameObjects = 0;
            TrackManager.Instance.spawns.destroyedGameObjects = 0;
            TrackManager.Instance.spawns.activeGameObjects.Clear();

            foreach (var remains in TrackManager.Instance.remains.activeGameObjects)
            {
                Destroy(remains);
            }

            TrackManager.Instance.remains.totalGameObjects = 0;
            TrackManager.Instance.remains.destroyedGameObjects = 0;
            TrackManager.Instance.remains.activeGameObjects.Clear();
        }

        private void OnDestroy()
        {
            Reset();
        }
    }
}
