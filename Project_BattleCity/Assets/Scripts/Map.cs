using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Map : MonoBehaviour {
    public GameObject tilePrefab;
    public GameObject waterTilePrefab;
    public GameObject enemyPrefab;
    public GameObject campPrefab;

    [SerializeField]
    public int rows;
    [SerializeField]
    public int cols;
    [SerializeField]
    public int layer;

    [SerializeField]
    private float waterCoverage;

    private Sprite[] mapSprites;
    public GameObject[,,] map;
    private Utility.TileType tileType;
    public int enemyCount;

    void Start() {
        enemyCount = 0;
        mapSprites = Resources.LoadAll<Sprite>("TileMap/GroundTileset");
        cols = 50;
        rows = 50;
        layer = 2;
        map = new GameObject[cols, rows, layer];

        GenerateCamp();
        GenerateWorld();
        BuildToIsland();
        HideWorld();
        EnemyDies();
    }

    private void SetNewWaterTransition(int x, int y, Utility.Side side) {
        byte[] byteTile;
        switch (side){
            case Utility.Side.Down:
                byteTile = new byte[] { Utility.TRANSPARENT, Utility.TRANSPARENT, Utility.WATER, Utility.WATER };
                break;
            case Utility.Side.Left:
                byteTile = new byte[] { Utility.WATER, Utility.TRANSPARENT, Utility.WATER, Utility.TRANSPARENT };
                break;
            case Utility.Side.Right:
                byteTile = new byte[] { Utility.TRANSPARENT, Utility.WATER, Utility.TRANSPARENT, Utility.WATER };
                break;
            case Utility.Side.Up:
                byteTile = new byte[] { Utility.WATER, Utility.WATER, Utility.TRANSPARENT, Utility.TRANSPARENT };
                break;
            default:
                byteTile = new byte[] { Utility.TRANSPARENT, Utility.TRANSPARENT, Utility.TRANSPARENT, Utility.TRANSPARENT };
                break;
        }

        int spriteID = Utility.GetSpriteIDByByteMap(byteTile);

        GameObject waterTile = (GameObject)Instantiate(waterTilePrefab, transform);
        waterTile.transform.SetParent(this.transform);
        waterTile.GetComponent<Water>().SetByteMap(byteTile);

        SetTilePosition(waterTile, new Vector3(x, y, -0.01f));

        for (int i = 1; i < layer; i++) {
            Destroy(map[x, y, i]);
        }

        map[x, y, 1] = waterTile;
    }

    private void SetNewWaterTile(int x, int y) {
        GameObject waterTile = (GameObject)Instantiate(waterTilePrefab, transform);
        waterTile.transform.SetParent(this.transform);

        SetTilePosition(waterTile, new Vector3(x, y, -0.01f));

        for(int i = 0; i < layer; i++) {
            Destroy(map[x, y, i]);
        }

        map[x, y, 1] = waterTile;
    }

    /* 
     * Übergang zum Land machen
     * Hintergrundplane aufziehen
     */
    private void BuildToIsland() {
        // Set first bordered Water tile
        int layer = 1;
        List<GameObject> transitionList = new List<GameObject>();

        // Set Water Border
        for (int x = 0; x < cols; x++) {
            SetNewWaterTile(x, 0);
            SetNewWaterTile(x, (rows-1));
        }

        for (int y = 0; y < rows; y++) {
            SetNewWaterTile(0, y);
            SetNewWaterTile((cols-1), y);
        }

        // Set Transition to Land
        for (int x = 1; x < (cols-1); x++) {

            // Bottom
            if (map[x, 2, 1] == null) {
                SetNewWaterTransition(x, 1, Utility.Side.Down);
            } else {
                SetNewWaterTile(x, 1);
            }

            // Up
            if (map[x, (rows - 2), 1] == null) {
                SetNewWaterTransition(x, (rows - 2), Utility.Side.Up);
            } else {
                SetNewWaterTile(x, (rows - 2));
            }  
        }

        for (int y = 1; y < (rows-1); y++) {

            // Left
            if(map[2,y,1] == null) {
                SetNewWaterTransition(1, y, Utility.Side.Left);
            } else {
                SetNewWaterTile(1, y);
            }

            // Right
            if (map[(cols-2), y, 1] == null) {
                SetNewWaterTransition((cols -2), y, Utility.Side.Right);
            } else {
                SetNewWaterTile((cols - 2), y);
            }
        }

        // Set water Plane
        GameObject waterBackground = GameObject.Find("Background");
        waterBackground.GetComponent<SpriteRenderer>().size = new Vector2(cols + 30f, rows + 30f);
        waterBackground.transform.position = new Vector3(cols/2, rows/2, 4.0f);

    }

    private void HideWorld() {
        for (int x = 0; x < cols; x++) {
            for (int y = 0; y < cols; y++) {
                for (int z = 0; z < layer; z++) {
                    if(map[x,y,z] != null) {
                        map[x, y, z].active = false;
                    }
                }
            }
        }
    }

    // TODO inprogress
    private void GenerateCamp() {

        // Set the Base Camp on the Map
        GameObject camp = Instantiate(campPrefab, transform.position, Quaternion.identity) as GameObject;
        camp.transform.position = new Vector3( cols/2, rows/2, -0.01f);
        camp.GetComponent<Camp>().position = new Vector2( cols / 2, rows / 2);
    }

    private void GenerateWorld() {
        GenerateDefaultLayer(); //Lawn

        int layer = 0;
        Utility.TileType groundType = Utility.TileType.Ground;
        float coverage = 0.2f; // 20% - map(15x15) -> 45 fields
        float coherence = 0.2f; // 20% 45/ 100/20

        VariateGeneratedLayer(groundType, layer, coverage, coherence);

        if (waterCoverage > 0.3f) {
            waterCoverage = 0.3f;
        }

        VariateGeneratedLayer(Utility.TileType.Water, 1, waterCoverage, 0.6f);
    }

    private List<GameObject> GetDryNeighborhood(List<GameObject> neighbors, int x, int y) {

        for (int verticalOffset = -1; verticalOffset < 2; verticalOffset++) {
            for (int horizontalOffset = -1; horizontalOffset < 2; horizontalOffset++) {

                if (x + horizontalOffset < map.GetLength(0) &&
                    x + horizontalOffset >= 0 &&
                    y + verticalOffset < map.GetLength(1) &&
                    y + verticalOffset >= 0) {

                    if (map[x + horizontalOffset, y + verticalOffset, 1] == null) {
                        neighbors.Add(map[x + horizontalOffset, y + verticalOffset, 0]);
                    }
                }
            }
        }
        neighbors.Remove(map[x, y, 0]);

        return neighbors;
    }

    private void GenerateWaterTransition(List<GameObject> transitionList) {
        int layer = 1;

        // Generate referece Generic water transition tile   
        GameObject transparentTile = Instantiate(tilePrefab, transform.position, Quaternion.identity) as GameObject;
        transparentTile.GetComponent<Tile>().byteMap = Utility.TRANSPARENT_BYTE;

        // Once for each water tile on the map
        foreach (GameObject transition in transitionList) {
            int x = (int)GetTilePosition(transition).x;
            int y = (int)GetTilePosition(transition).y;

            List<GameObject> transitionNeighbors = new List<GameObject>();
            transitionNeighbors = GetDryNeighborhood(transitionNeighbors, x, y); // Neighbors based on Ground layer 0

            // Consider all neighbors from every water field
            foreach (GameObject transitionNeighbor in transitionNeighbors) {
                int xNeighbor = (int)transitionNeighbor.GetComponent<Tile>().position.x;
                int yNeighbor = (int)transitionNeighbor.GetComponent<Tile>().position.y;

                // Left tile Check (Border Check)
                if ((xNeighbor - 1) >= 0) {
                    CreateWaterTransitionTile(xNeighbor, -1, yNeighbor, 0, layer, transitionNeighbor, transparentTile, 1, 0, 3, 2);
                }

                // Right tile Check
                if ((xNeighbor + 1) < this.map.GetLength(0)) {
                    CreateWaterTransitionTile(xNeighbor, 1, yNeighbor, 0, layer, transitionNeighbor, transparentTile, 0, 1, 2, 3);
                }

                // Top tile Check
                if ((yNeighbor + 1) < this.map.GetLength(1)) {
                    CreateWaterTransitionTile(xNeighbor, 0, yNeighbor, 1, layer, transitionNeighbor, transparentTile, 2, 0, 3, 1);
                }

                // Buttom tile Check
                if ((yNeighbor - 1) >= 0) {
                    CreateWaterTransitionTile(xNeighbor, 0, yNeighbor, -1, layer, transitionNeighbor, transparentTile, 0, 2, 1, 3);
                }

                // Top Left
                if ((xNeighbor - 1) >= 0 && (yNeighbor + 1) < this.map.GetLength(1)) {
                    CreateWaterTransitionTile(xNeighbor, -1, yNeighbor, 1, layer, transitionNeighbor, transparentTile, 3, 0, 3, 0);
                }

                // Button Left
                if ((xNeighbor - 1) >= 0 && (yNeighbor - 1) >= 0) {
                    CreateWaterTransitionTile(xNeighbor, -1, yNeighbor, -1, layer, transitionNeighbor, transparentTile, 1, 2, 1, 2);
                }

                // Top Right
                if ((xNeighbor + 1) < this.map.GetLength(0) && (yNeighbor + 1) < this.map.GetLength(1)) {
                    CreateWaterTransitionTile(xNeighbor, 1, yNeighbor, 1, layer, transitionNeighbor, transparentTile, 2, 1, 2, 1);
                }

                // Buttom Right
                if ((xNeighbor + 1) < this.map.GetLength(0) && (yNeighbor - 1) >= 0) {
                    CreateWaterTransitionTile(xNeighbor, 1, yNeighbor, -1, layer, transitionNeighbor, transparentTile, 0, 3, 0, 3);
                }

                // Set Sprite
                if (map[xNeighbor, yNeighbor, layer] != null) {
                    byte[] byteTile = map[xNeighbor, yNeighbor, layer].GetComponent<Tile>().byteMap;
                    int spriteID = Utility.GetSpriteIDByByteMap(byteTile);

                    
                    GameObject setWaterTile = Instantiate(waterTilePrefab, map[xNeighbor, yNeighbor, layer].transform.position, Quaternion.identity) as GameObject;
                    setWaterTile.GetComponent<Water>().SetByteMap(byteTile);
                    setWaterTile.transform.SetParent(this.transform);

                    map[xNeighbor, yNeighbor, layer] = setWaterTile;
                }
            }
        }

        Destroy(transparentTile);
    }

    private void CreateWaterTransitionTile(int x, int xOffset, int y, int yOffset, int layer, GameObject transitionNeighbor, GameObject transparentTile, int checkA, int writeA, int checkB, int writeB) {
        if (map[x + xOffset, y + yOffset, layer] != null) {
            if (map[x + xOffset, y + yOffset, layer].GetComponent<Tile>().type == Utility.TileType.Water) {

                byte[] checkByte;
                checkByte = map[x + xOffset, y + yOffset, layer].GetComponent<Tile>().byteMap;

                if (map[x, y, layer] == null) {
                    // Set new transition water tile
                    GameObject waterTransitionTile = (GameObject)Instantiate(transparentTile, transform);
                    SetTilePosition(waterTransitionTile, new Vector3(x, y, -0.01f));
                    waterTransitionTile.transform.SetParent(this.transform);
                    SetTileType(waterTransitionTile, transitionNeighbor.GetComponent<Tile>().type);

                    map[x, y, layer] = waterTransitionTile;
                }

                if (checkByte[checkA] == Utility.WATER) {
                    map[x, y, layer].GetComponent<Tile>().byteMap[writeA] = Utility.WATER;
                }

                if (checkByte[checkB] == Utility.WATER) {
                    map[x, y, layer].GetComponent<Tile>().byteMap[writeB] = Utility.WATER;
                }
            }
        }
    }

    private void GenerateGroundTransition(List<GameObject> transitionList) {
        int layer = 0;

        // Once for each earth tile on the map (full tile not mutated earth tile "Made to earth by neighbors")
        foreach (GameObject transition in transitionList) {
            int x = (int)GetTilePosition(transition).x;
            int y = (int)GetTilePosition(transition).y;

            List<GameObject> transitionNeighbors = new List<GameObject>();
            transitionNeighbors = GetNeighborhood(transitionNeighbors, x, y); // Neighbors based on Ground layer 0

            // Delete duplicates of type {EARTH, EARTH, EARTH, EARTH}
            foreach (GameObject duplicates in transitionList) {
                transitionNeighbors.Remove(duplicates);
            }

            // Consider all neighbors from every earth field
            foreach (GameObject checkZone in transitionNeighbors) {
                int xPosition = (int)checkZone.GetComponent<Tile>().position.x;
                int yPosition = (int)checkZone.GetComponent<Tile>().position.y;
                byte[] checkByte;

                // Left tile Check
                if ((xPosition - 1) >= 0) {
                    checkByte = map[xPosition - 1, yPosition, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[1] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[0] = Utility.EARTH;
                    }

                    if (checkByte[3] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[2] = Utility.EARTH;
                    }
                }

                // Right tile Check
                if ((xPosition + 1) < this.map.GetLength(0)) {
                    checkByte = map[xPosition + 1, yPosition, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[0] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[1] = Utility.EARTH;
                    }

                    if (checkByte[2] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[3] = Utility.EARTH;
                    }
                }

                // Top tile Check
                if ((yPosition + 1) < this.map.GetLength(1)) {
                    checkByte = map[xPosition, yPosition + 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[2] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[0] = Utility.EARTH;
                    }

                    if (checkByte[3] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[1] = Utility.EARTH;
                    }
                }

                // Buttom tile Check
                if ((yPosition - 1) >= 0) {
                    checkByte = map[xPosition, yPosition - 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[0] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[2] = Utility.EARTH;
                    }

                    if (checkByte[1] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[3] = Utility.EARTH;
                    }
                }

                // Top Left
                if ((xPosition - 1) >= 0 && (yPosition + 1) < this.map.GetLength(1)) {
                    checkByte = map[xPosition - 1, yPosition + 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[3] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[0] = Utility.EARTH;
                    }
                }

                // Button Left
                if ((xPosition - 1) >= 0 && (yPosition - 1) >= 0) {
                    checkByte = map[xPosition - 1, yPosition - 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[1] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[2] = Utility.EARTH;
                    }
                }

                // Top Right
                if ((xPosition + 1) < this.map.GetLength(0) && (yPosition + 1) < this.map.GetLength(1)) {
                    checkByte = map[xPosition + 1, yPosition + 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[2] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[1] = Utility.EARTH;
                    }
                }

                // Buttom Right
                if ((xPosition + 1) < this.map.GetLength(0) && (yPosition - 1) >= 0) {
                    checkByte = map[xPosition + 1, yPosition - 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[0] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[3] = Utility.EARTH;
                    }
                }

                // Set Sprite
                byte[] byteTile = checkZone.GetComponent<Tile>().byteMap;
                int spriteID = Utility.GetSpriteIDByByteMap(byteTile);
                checkZone.GetComponent<SpriteRenderer>().sprite = mapSprites[spriteID];
            }
        }
    }

    private List<GameObject> UpdateRiverBack(Vector2 direction, GameObject position) {
        List<GameObject> returnList = new List<GameObject>();

        if (position != null) {
            int xPosition = (int)position.GetComponent<Tile>().position.x;
            int yPosition = (int)position.GetComponent<Tile>().position.y;

            if (direction.x == 1 && direction.y == 1) {
                if (xPosition - 1 > 0) {
                    if (map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == -1) {
                if (xPosition + 1 < map.GetLength(0)) {
                    if (map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == 0 && direction.y == 1) {

                if (xPosition + 1 < map.GetLength(0) && yPosition - 1 >= 0) {
                    if (map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition - 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 0 && direction.y == -1) {

                if (xPosition + 1 < map.GetLength(0) && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == 1) {
                if (xPosition + 1 < map.GetLength(0)) {
                    if (map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == -1) {
                if (xPosition - 1 >= 0) {
                    if (map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition, 0]);
                    }
                }

                if (yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == 0) {
                if (xPosition - 1 >= 0 && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == 0) {
                if (xPosition + 1 < map.GetLength(0) && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition + 1 < map.GetLength(0) && yPosition - 1 >= 0) {
                    if (map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition - 1, 0]);
                    }
                }
            }
        }

        return returnList;
    }

    private List<GameObject> UpdateRiverStrive(Vector2 direction, GameObject position) {
        List<GameObject> returnList = new List<GameObject>();

        if (position != null) {
            int xPosition = (int)position.GetComponent<Tile>().position.x;
            int yPosition = (int)position.GetComponent<Tile>().position.y;

            if (direction.x == -1 && direction.y == -1) {
                if (xPosition - 1 > 0) {
                    if (map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == 1) {
                if (xPosition + 1 < map.GetLength(0)) {
                    if (map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == 0 && direction.y == -1) {

                if (xPosition + 1 < map.GetLength(0) && yPosition - 1 >= 0) {
                    if (map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition - 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 0 && direction.y == 1) {

                if (xPosition + 1 < map.GetLength(0) && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == -1) {
                if (xPosition + 1 < map.GetLength(0)) {
                    if (map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == 1) {
                if (xPosition - 1 >= 0) {
                    if (map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition, 0]);
                    }
                }

                if (yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == 0) {
                if (xPosition - 1 >= 0 && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == 0) {
                if (xPosition + 1 < map.GetLength(0) && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition + 1 < map.GetLength(0) && yPosition - 1 >= 0) {
                    if (map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition - 1, 0]);
                    }
                }
            }
        }

        return returnList;
    }

    private List<GameObject> UpdateRiverSide(Vector2 direction, GameObject position) {
        List<GameObject> returnList = new List<GameObject>();

        if (position != null) {
            int xPosition = (int)position.GetComponent<Tile>().position.x;
            int yPosition = (int)position.GetComponent<Tile>().position.y;

            if (direction.x == -1 && direction.y == -1 || direction.x == 1 && direction.y == 1) {
                if (xPosition + 1 < map.GetLength(0) && yPosition - 1 >= 0) {
                    if (map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition - 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition + 1, 0]);
                    }
                }
            } else if (direction.x == 0 && direction.y == -1 || direction.x == 0 && direction.y == 1) {

                if (xPosition + 1 < map.GetLength(0)) {
                    if (map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (xPosition - 1 >= 0) {
                    if (map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition, 0]);
                    }
                }
            } else if (direction.x == 1 && direction.y == -1 || direction.x == -1 && direction.y == 1) {
                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

                if (xPosition + 1 < map.GetLength(0) && yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition + 1, yPosition + 1, 0]);
                    }
                }
            } else if (direction.x == -1 && direction.y == 0 || direction.x == 1 && direction.y == 0) {
                if (yPosition + 1 < map.GetLength(1)) {
                    if (map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition + 1, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(map[xPosition, yPosition - 1, 0]);
                    }
                }
            }
        }

        return returnList;
    }

    private GameObject CreateWaterTile(GameObject referenceTile, Vector2 position) {
        if ((int)position.x >= 0 && (int)position.x < map.GetLength(0) &&
            (int)position.y >= 0 && (int)position.y < map.GetLength(1)) {

            GameObject newWaterTile = (GameObject)Instantiate(referenceTile, transform);

            newWaterTile.transform.SetParent(this.transform);
            SetTilePosition(newWaterTile, new Vector3(position.x, position.y, -0.01f));
            map[(int)position.x, (int)position.y, 1] = newWaterTile;

            return newWaterTile;
        }

        return null;
    }

    // Code for pattern generator
    private void VariateGeneratedLayer(Utility.TileType groundType, int layer, float coverage, float coherence) {
        List<GameObject> transitionList = new List<GameObject>();
        int toCover = (int)(cols * rows * coverage);
        int covered = 0;
        
        int earthSpriteID = Utility.GetSpriteIDByByteMap(Utility.EARTH_BYTE);

        while (covered < toCover) {
            int x = Random.Range(0, cols);
            int y = Random.Range(0, rows);

            switch (groundType) {
                case Utility.TileType.Ground:
                    GameObject tile = map[x, y, layer];

                    if (tile.GetComponent<SpriteRenderer>().sprite != mapSprites[earthSpriteID]) {

                        // Set first Random Earth Tile
                        tile.GetComponent<SpriteRenderer>().sprite = mapSprites[earthSpriteID]; // Earth
                        SetByteMap(tile, earthSpriteID);
                        SetTileType(tile, groundType);

                        // Create a list of Nighbors (increased chanse by coherence for this fields)
                        List<GameObject> neighbors = new List<GameObject>();
                        neighbors = GetNeighborhood(neighbors, x, y); // Neighbors based on ground layer 0

                        // Depend on coherence build 8er blocks of dirt
                        foreach (GameObject neighbor in neighbors) {
                            if (coherence >= Random.Range(0.0f, 1.0f)) {

                                GameObject tmpTile = map[(int)GetTilePosition(neighbor).x, (int)GetTilePosition(neighbor).y, layer];
                                tmpTile.GetComponent<SpriteRenderer>().sprite = mapSprites[earthSpriteID]; // Earth
                                SetByteMap(tmpTile, earthSpriteID);
                                SetTileType(tmpTile, groundType);
                                transitionList.Add(tmpTile);

                                if (covered < toCover) {
                                    covered++;

                                } else {
                                    break;
                                }
                            }
                        }
                        covered++;
                        transitionList.Add(tile);
                    }
                    break;
                case Utility.TileType.SoftGround:
                    break;
                case Utility.TileType.Mud:
                    break;
                case Utility.TileType.Desert:
                    break;
                case Utility.TileType.Asphalt:
                    break;
                case Utility.TileType.Water:
                    // Set first Random Water Tile on the Map border
                    if (Random.Range(0, 2) == 0) {
                        x = Random.Range(0, 2) * (cols - 1);
                    } else {
                        y = Random.Range(0, 2) * (rows - 1);
                    }

                    if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1) && map[x, y, layer] == null) {

                        // Set first bordered Water tile
                        GameObject waterTile = (GameObject)Instantiate(waterTilePrefab, transform);
                        waterTile.transform.SetParent(this.transform);

                        // Needed to provent from gui glitching (right layer order does not fix it)
                        SetTilePosition(waterTile, new Vector3(x, y, -0.01f));
                        map[x, y, layer] = waterTile;
                        transitionList.Add(waterTile);
                        covered++;

                        // Set the initial direction for the first water/river tile
                        Vector2 riverDirection = GetBorderedDirection(x, y);

                        // Random Chance to stop generate the river
                        float breakChance = 0.05f;

                        List<GameObject> backList = new List<GameObject>();
                        List<GameObject> sideList = new List<GameObject>();
                        List<GameObject> striveList = new List<GameObject>();

                        // Build the river 
                        float breakValue = Random.Range(0.0f, 1.0f);

                        while (covered < toCover && breakValue > breakChance) {
                            int xRange = x + (int)riverDirection.x;
                            int yRange = y + (int)riverDirection.y;

                            // Dont Try to work outside of map
                            if (x < 0 || x >= map.GetLength(0) || y < 0 || y >= map.GetLength(1)) {
                                break;
                            }

                            // if the neighbor fileds are inside the border of the map
                            if (xRange >= 0 && xRange < map.GetLength(0) && yRange >= 0 && yRange < map.GetLength(1)) {

                                // Initiate all the nighbors of the current river field.
                                backList = UpdateRiverBack(riverDirection, map[x, y, 0]);
                                sideList = UpdateRiverSide(riverDirection, map[x, y, 0]);
                                striveList = UpdateRiverStrive(riverDirection, map[x, y, 0]);

                            //  Dont Try to work outside of map
                            } else {
                                break;
                            }

                            // Get neighbors with coherence wights
                            float nextRiverTile = Random.Range(0.0f, (1.0f - breakChance));
                            bool forward = true;

                            // Set Coherence dependent chances
                            float back = 1.0f;
                            float side = back + coherence;                                  // min(1.0f) max(2.0f)
                            float strive = side + 2 * coherence;                            // min(1.0f) max(4.0f)
                            float front = strive + 4 * coherence;                           // min(1.0f) max(8.0f)
                            float fragmentSum = backList.Count * back + sideList.Count * side + striveList.Count * strive + front;   // min(4.0f) max(15.0f)

                            float fragmentChance = (1.0f - breakChance) / fragmentSum;

                            float backChance = backList.Count * back * fragmentChance;
                            float sideChance = backChance + sideList.Count * side * fragmentChance;
                            float striveChance = sideChance + striveList.Count * strive * fragmentChance;

                            Vector2 newTilePosition = new Vector2(0.0f, 0.0f);
                            int listIndex = Random.Range(0, 2);

                            if (backList.Count != 0 && backChance > nextRiverTile) {

                                if (backList.Count == 1) {
                                    newTilePosition = backList[0].GetComponent<Tile>().position;

                                } else {
                                    newTilePosition = backList[listIndex].GetComponent<Tile>().position;
                                }

                            } else if (sideList.Count != 0 && sideChance > nextRiverTile) {

                                if (sideList.Count == 1) {
                                    newTilePosition = sideList[0].GetComponent<Tile>().position;

                                } else {
                                    newTilePosition = sideList[listIndex].GetComponent<Tile>().position;
                                }

                            } else if (striveList.Count != 0 && striveChance > nextRiverTile) {

                                if (striveList.Count == 1) {
                                    newTilePosition = striveList[0].GetComponent<Tile>().position;

                                } else {
                                    newTilePosition = striveList[listIndex].GetComponent<Tile>().position;
                                }

                            } else if(forward) {
                                newTilePosition = riverDirection + map[x, y, 0].GetComponent<Tile>().position;
                                
                                // Check index outside map
                                if ((int)newTilePosition.x < 0 || (int)newTilePosition.x >= map.GetLength(0) ||
                                    (int)newTilePosition.y < 0 || (int)newTilePosition.y >= map.GetLength(1)) {

                                    break;
                                }

                                forward = false;

                            // Just in case of
                            } else {
                                break;
                            }

                            if (map[(int)newTilePosition.x, (int)newTilePosition.y, 1] == null) {
                                transitionList.Add(CreateWaterTile(waterTilePrefab, newTilePosition));
                                covered++;
                                forward = true;

                                Vector2 oldPosition = new Vector2(x, y);
                                riverDirection = newTilePosition - oldPosition;

                                // Update current position. Jumpt to the nex field on the next iteration.
                                x = (int)newTilePosition.x;
                                y = (int)newTilePosition.y;
  
                            } else {
                                backList.Remove(map[(int)newTilePosition.x, (int)newTilePosition.y, 1]);
                                sideList.Remove(map[(int)newTilePosition.x, (int)newTilePosition.y, 1]);
                                striveList.Remove(map[(int)newTilePosition.x, (int)newTilePosition.y, 1]);
                            }

                            breakValue = Random.Range(0.0f, 1.0f);
                        }
                    }
                    break;
                case Utility.TileType.Ice:
                    break;
                case Utility.TileType.FragileIce:
                    break;
                case Utility.TileType.Swamp:
                    break;
                default:
                    break;
            }
        }

        if (layer == 0) {
            GenerateGroundTransition(transitionList);
        } else {
            GenerateWaterTransition(transitionList);
        }
    }
    private Vector2 GetBorderedDirection(int x, int y) {
        // corner
        if (x == 0 && y == 0) {
            return new Vector2(1.0f, 1.0f);
        } else if (x == (map.GetLength(0) - 1) && y == (map.GetLength(1) - 1)) {
            return new Vector2(-1.0f, -1.0f);
        } else if (x == 0 && y == (map.GetLength(1) - 1)) {
            return new Vector2(1.0f, -1.0f);
        } else if (x == (map.GetLength(0) - 1) && y == 0) {
            return new Vector2(-1.0f, 1.0f);

        // side
        } else if (x == 0) {
            return new Vector2(1.0f, 0.0f);
        } else if (x == (map.GetLength(0) - 1)) {
            return new Vector2(-1.0f, 0.0f);
        } else if (y == 0) {
            return new Vector2(0.0f, 1.0f);
        } else {
            return new Vector2(0.0f, -1.0f);
        }
    }

    private List<GameObject> GetNeighborhood(List<GameObject> neighbors, int x, int y) {
        for (int verticalOffset = -1; verticalOffset < 2; verticalOffset++) {
            for (int horizontalOffset = -1; horizontalOffset < 2; horizontalOffset++) {
                if (x + horizontalOffset < map.GetLength(0) &&
                    x + horizontalOffset >= 0 &&
                    y + verticalOffset < map.GetLength(1) &&
                    y + verticalOffset >= 0) {

                    neighbors.Add(map[x + horizontalOffset, y + verticalOffset, 0]);
                }
            }
        }
        neighbors.Remove(map[x, y, 0]);

        return neighbors;
    }

    public void EnemyDies() {
        enemyCount--;

        if (enemyCount < 0) {
            Utility.Victory();
        } else {
            GameObject enemy = (GameObject)Instantiate(enemyPrefab, transform);
            enemy.transform.position = new Vector2(
                    Random.Range(1.0f, (float)map.GetLength(0) - 1.0f),
                    Random.Range(1.0f, (float)map.GetLength(1) - 1.0f)
            );
        }
    }

    private void GenerateDefaultLayer() {
        // Define Default Base Ground tile
        GameObject referenceTile = Instantiate(tilePrefab, transform.position, Quaternion.identity) as GameObject;
        
        int lawnSpriteID = Utility.GetSpriteIDByByteMap(Utility.LAWN_BYTE);
        referenceTile.GetComponent<SpriteRenderer>().sprite = mapSprites[lawnSpriteID]; // Lawn
        SetByteMap(referenceTile, lawnSpriteID); //Lawn
        SetTileType(referenceTile, Utility.TileType.Ground);

        for (int l = 0; l < layer; l++) {
            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < cols; x++) {

                    if (l == 0) {
                        // Generate Base Ground Layer
                        GameObject tile = (GameObject)Instantiate(referenceTile, transform);
                        tile.transform.SetParent(this.transform);

                        SetTilePosition(tile, new Vector2(x, y));

                        map[x, y, 0] = tile;

                    } else {
                        map[x, y, l] = null;
                    }
                }
            }
        }

        Destroy(referenceTile);
    }

    void SetTilePosition(GameObject tile, Vector2 tilePosition) {
        tile.transform.position = tilePosition;
        tile.GetComponent<Tile>().position = tilePosition;
    }

    void SetTilePosition(GameObject tile, Vector3 tilePosition) {
        tile.transform.position = tilePosition;
        tile.GetComponent<Tile>().position = tilePosition;
    }

    private Vector2 GetTilePosition(GameObject tile) {
        return tile.GetComponent<Tile>().position;
    }

    public GameObject GetTileOnPosition(Vector2 position) {
        return map[(int)position.x, (int)position.y, 0];
    }

    private void SetByteMap(GameObject tile, int position) {
        int maxLength = tile.GetComponent<Tile>().byteMap.Length;

        for (int i = 0; i < maxLength; i++) {
            tile.GetComponent<Tile>().byteMap[i] = Utility.BYTE_MAP[position, i];
        }
    }

    void SetTileType(GameObject tile, Utility.TileType type) {

        switch (type) {
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
