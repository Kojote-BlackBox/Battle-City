using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject enemyPrefab;
    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    private Sprite[] mapSprites;
    public GameObject[,] map;
    private Utility.TileType tileType;
    public int enemyCount;

    void Start()
    {
        enemyCount = 0;
        mapSprites = Resources.LoadAll<Sprite>("TileMap/landscapeTileSet");
        cols = 15;
        rows = 15;
        map = new GameObject[cols, rows];

        GenerateWorld();
        EnemyDies();
    }

    public void EnemyDies()
    {
        enemyCount--;
        if (enemyCount < 0)
        {
            // TODO Sieg
            //QuitGame();
        }
        else
        {
            GameObject enemy = (GameObject)Instantiate(enemyPrefab, transform);
            enemy.transform.position = new Vector2(
                    Random.Range(1.0f, (float)map.GetLength(0) - 1.0f),
                    Random.Range(1.0f, (float)map.GetLength(1) - 1.0f)
            );
        }
    }

    public void QuitGame()
    {
        // save any game data here
        #if UNITY_EDITOR
             // Application.Quit() does not work in the editor so
             // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
             UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void GenerateWorld()
    {
        GameObject referenceTile = Instantiate(tilePrefab, transform.position, Quaternion.identity) as GameObject;
        referenceTile.GetComponent<SpriteRenderer>().sprite = mapSprites[7];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                GameObject tile = (GameObject)Instantiate(referenceTile, transform);
                tile.transform.SetParent(this.transform);
                SetTileSprite(tile);
                SetTilePosition(tile, new Vector2(x, y));

                map[x, y] = tile;
            }
        }
        Destroy(referenceTile);

        // TODO Center Map Provide weird behavior 
        //transform.position = new Vector2(-(cols / 2), -(rows / 2));
    }
    void SetTilePosition(GameObject tile, Vector2 tilePosition)
    {
        tile.transform.position = tilePosition;
        tile.GetComponent<Tile>().position = tilePosition;
    }

    public GameObject GetTileOnPosition(Vector2 position)
    {
        return map[(int)position.x, (int)position.y];
    }

    void SetTileSprite(GameObject tile)
    {
        // Random 0,1,2,3 
        switch ((int)Random.Range(0, 10))
        {
            case 0:
                //Mud
                tile.GetComponent<SpriteRenderer>().sprite = mapSprites[0];
                SetTileType(tile, Utility.TileType.Mud);
                break;
            case 1:
                //Water
                tile.GetComponent<SpriteRenderer>().sprite = mapSprites[25];
                SetTileType(tile, Utility.TileType.Water);
                break;
            case 2:
                //Swamp
                tile.GetComponent<SpriteRenderer>().sprite = mapSprites[35];
                SetTileType(tile, Utility.TileType.Swamp);
                break;
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
                //Ground
                tile.GetComponent<SpriteRenderer>().sprite = mapSprites[7];
                SetTileType(tile, Utility.TileType.Ground);
                break;
            default:
                break;
        }
    }

    void SetTileType(GameObject tile, Utility.TileType type)
    {
        switch (type)
        {
            case Utility.TileType.Ground:
                tile.GetComponent<Tile>().slowDown = 0.9f; // to 90% of initial Speed
                tile.GetComponent<Tile>().passable = true;
                break;
            case Utility.TileType.SoftGround:
                tile.GetComponent<Tile>().slowDown = 0.8f;
                tile.GetComponent<Tile>().passable = true;
                break;
            case Utility.TileType.Mud:
                tile.GetComponent<Tile>().slowDown = 0.5f;
                tile.GetComponent<Tile>().passable = true;
                break;
            case Utility.TileType.Desert:
                tile.GetComponent<Tile>().slowDown = 0.7f;
                tile.GetComponent<Tile>().passable = true;
                break;
            case Utility.TileType.Asphalt:
                tile.GetComponent<Tile>().slowDown = 1.0f;
                tile.GetComponent<Tile>().passable = true;
                break;
            case Utility.TileType.Water:
                tile.GetComponent<Tile>().passable = false;
                break;
            case Utility.TileType.Ice:
                tile.GetComponent<Tile>().slowDown = 1.0f;
                tile.GetComponent<Tile>().passable = true;
                break;
            case Utility.TileType.FragileIce:
                tile.GetComponent<Tile>().slowDown = 1.0f;
                tile.GetComponent<Tile>().passable = true;
                break;
            case Utility.TileType.Swamp:
                tile.GetComponent<Tile>().slowDown = 0.2f;
                tile.GetComponent<Tile>().passable = true;
                break;
            default:
                tile.GetComponent<Tile>().passable = false;
                break;
        }
        tile.GetComponent<Tile>().type = type;
    }
}
