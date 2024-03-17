using UnityEngine;
using Gameplay.Bunker;
using Core;
using Core.Reference;
using Core.Spawn;
using Core.Tag;
using Core.Track;


namespace World.Builder
{
    public class BuilderBunker
    {
        #region data
        public DataBunker dataBunkerFriendly { get; set; }
        public DataBunker[] dataBunkersEnemy { get; set; }
        #endregion

        #region scene
        public GameObject sceneParentObject { get; set; }
        #endregion

        public void Generate(Map map)
        {
            var positionPlayerBunkerX = map.columns / 2;
            var positionPlayerBunkerY = map.rows / 2;

            SpawnBunker(dataBunkerFriendly, new Vector2(positionPlayerBunkerX, positionPlayerBunkerY), true);

            map.MarkPlacement(new Vector2Int(positionPlayerBunkerX, positionPlayerBunkerY), dataBunkerFriendly.sizeUnit);

            foreach (var bunkerEnemy in dataBunkersEnemy)
            {
                var mapRectangle = new Vector2Int(
                    Random.Range(0, GameConstants.MapRectangleCount),
                    Random.Range(0, GameConstants.MapRectangleCount)
                );

                while (mapRectangle.x == 1 && mapRectangle.y == 1)
                {
                    mapRectangle = new Vector2Int(
                        Random.Range(0, GameConstants.MapRectangleCount),
                        Random.Range(0, GameConstants.MapRectangleCount)
                    );
                }

                var spawnPoint = map.GetRandomPointForObject(
                    bunkerEnemy.sizeUnit,
                    new Vector2Int(GameConstants.MapRectangleCount, GameConstants.MapRectangleCount),
                    mapRectangle
                );

                SpawnBunker(bunkerEnemy, spawnPoint, false);
            }
        }

        public void SpawnBunker(DataBunker dataBunker, Vector2 position, bool isPlayer)
        {
            Debug.Log("Spawning bunker at " + position);

            var strOwnership = dataBunker.isFriendly ? "Friendly" : "Enemy";

            GameObject bunker = new GameObject();

            if (sceneParentObject != null) bunker.transform.parent = sceneParentObject.transform;

            bunker.name = "Bunker " + strOwnership;

            SpriteRenderer spriteRenderer = bunker.AddComponent<SpriteRenderer>();

            Bunker bunkerComponent = bunker.AddComponent<Bunker>();
            bunkerComponent.dataBunker = dataBunker;

            ComponentTags componentTags = bunker.AddComponent<ComponentTags>();
            componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagBunker));

            if (dataBunker.prefabsToSpawn != null)
            {
                SpawnPoint spawnPoint = bunker.AddComponent<SpawnPoint>();
                spawnPoint.isFriendly = dataBunker.isFriendly;
                spawnPoint.isPlayerSpawn = isPlayer;
                spawnPoint.spawnDelay = (int)dataBunker.spawnTime;
                spawnPoint.prefabsToSpawn = dataBunker.prefabsToSpawn;
                if (!isPlayer) spawnPoint.enableRespawn = true;
                spawnPoint.onlyRespawnIfPrevIsDestroyed = true;
                spawnPoint.tag = GameConstants.TagSpawn;
            }

            spriteRenderer.sortingOrder = (int)LayerType.ObjectOverlay;
            spriteRenderer.sprite = dataBunker.spriteBunker;

            CircleCollider2D circleCollider = bunker.AddComponent<CircleCollider2D>();
            circleCollider.isTrigger = true;

            bunker.transform.position = new Vector3(position.x, position.y, 0f); ;

            if (dataBunker.isFriendly)
            {
                componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagFriendly));
                // TODO: only one player bunker is supported right now

                if (TrackManager.Instance.allyBunkers.gameObject != null)
                {
                    TrackManager.Instance.allyBunkers.relatedGameObjects.Add(bunker);
                }
                else
                {
                    TrackManager.Instance.allyBunkers.gameObject = bunker;
                }
            }
            else
            {
                TrackManager.Instance.enemyBunkers.activeGameObjects.Add(bunker);
                TrackManager.Instance.enemyBunkers.totalGameObjects += 1;
                componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagEnemy));
            }

            Debug.Log("spawning " + strOwnership + " bunker");
        }
    }
}

