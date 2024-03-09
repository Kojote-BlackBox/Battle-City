using UnityEngine;
using Gameplay.Bunker;
using Core;
using Core.Reference;
using Core.Spawn;
using Core.Tag;


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

        #region tracking
        public ReferenceRelatedGameObjects trackerBunker { get; set; }
        #endregion

        public void Generate(Map map)
        {
            SpawnBunker(dataBunkerFriendly, new Vector2(map.columns / 2, map.rows / 2));

            // TODO: tell the map that this spot is occupied

            foreach (var bunkerEnemy in dataBunkersEnemy)
            {
                var spawnPoint = map.GetRandomPointForObject(bunkerEnemy.sizeUnit);

                // TODO: ccheck if possible to spawn at that point

                SpawnBunker(bunkerEnemy, spawnPoint);
            }
        }

        public void SpawnBunker(DataBunker dataBunker, Vector2 position)
        {
            var strOwnership = dataBunker.isFriendly ? "Friendly" : "Enemy";

            GameObject bunker = new GameObject();

            if (sceneParentObject != null) bunker.transform.parent = sceneParentObject.transform;

            bunker.name = "Bunker " + strOwnership;

            SpriteRenderer spriteRenderer = bunker.AddComponent<SpriteRenderer>();

            Bunker bunkerComponent = bunker.AddComponent<Bunker>();
            bunkerComponent.dataBunker = dataBunker;

            ComponentTags componentTags = bunker.AddComponent<ComponentTags>();
            componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagBunker));

            if (dataBunker.prefabSpawnObject != null)
            {
                SpawnPoint spawnPoint = bunker.AddComponent<SpawnPoint>();
                spawnPoint.isFriendly = dataBunker.isFriendly;
                spawnPoint.enableUpgradeDrop = dataBunker.prefabDropUpgrade != null;
                spawnPoint.spawnDelay = (int)dataBunker.spawnTime;
                spawnPoint.prefabSpawnObject = dataBunker.prefabSpawnObject;
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
                trackerBunker.gameObject = bunker; // TODO: only one player bunker is supported right now
            }
            else
            {
                trackerBunker.relatedGameObjects.Add(bunker);
                componentTags.AddTag(TagManager.Instance.GetTagByIdentifier(GameConstants.TagEnemy));
            }

            Debug.Log("spawning " + strOwnership + " bunker");
        }
    }
}

