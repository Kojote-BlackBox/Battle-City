using UnityEngine;
using System.Collections.Generic;

public class MapBuilder {

    public Map mapScript;
    private GeneratorTile generatorTile;
    private BuilderGround builderGround;
    private BuilderSoftGround builderSoftGround;
    private BuilderMud builderMud;
    private BuilderDesert builderDesert;
    private BuilderAsphalt builderAsphalt;
    private BuilderWater builderWater;
    private BuilderIce builderIce;
    private BuilderFragileIce builderFragileIce;
    private BuilderSwamp builderSwamp;

    public MapBuilder(GameObject origin) {
        this.mapScript = origin.GetComponent<Map>();
        this.generatorTile = new GeneratorTile(this);

        builderGround = new BuilderGround(this);
        //builderSoftGround = new BuilderSoftGround(this);
        //builderMud = new BuilderMud(this);
        //builderDesert = new BuilderDesert(this);
        //builderAsphalt = new BuilderAsphalt(this);
        builderWater = new BuilderWater(this);
        //builderIce = new BuilderIce(this);
        //builderFragileIce = new BuilderFragileIce(this);
        //builderSwamp = new BuilderSwamp(this);       
    }

    /// <summary>
    /// Initializes the base layer of the game map.
    /// This function is responsible for creating and setting up the first layer of the game world
    /// at the start of the game. It fills this layer with a specified type of tile, 
    /// ensuring that the foundational layer of the map is uniformly covered with these tiles.
    /// This setup is crucial as it forms the basic terrain on which the game will be played.
    /// </summary>
    /// <param name="tileData">TileData containing the properties of the tile to fill the base layer with.</param>
    public void InitializeBaseLayer(TileData tileData) {
        GameObject tilePattern = InstantiateToTilePattern(tileData);
        int layer = (int)tileData.LayerType;

        for (int y = 0; y < mapScript.rows; y++) {
            for (int x = 0; x < mapScript.cols; x++) {
                mapScript.map[x, y, layer] = tiling(tilePattern, new Vector2(x, y));
            }
        }

       GameObject.Destroy(tilePattern);
    }

    public GameObject InstantiateToTilePattern(TileData tileData) {
        GameObject cullingTile = GameObject.Instantiate(mapScript.tilePrefab, mapScript.transform);
        GameObject tile = cullingTile.GetComponent<Culling>().tile;
        tile.GetComponent<SpriteRenderer>().sortingOrder = (int)tileData.LayerType;

        Utility.SetDataToTile(tile, tileData);
        cullingTile.transform.SetParent(mapScript.transform);

        return cullingTile;
    }

    public GameObject tiling(GameObject tilePattern, Vector2 position) {
        GameObject cullingTile = GameObject.Instantiate(tilePattern, mapScript.transform);
        cullingTile.transform.position = position;

        return cullingTile;
    }

    // Wird zum Systematischen Random Generieren benutzt, um Endlosschleifen zu vermeiden.
    public List<Vector2> GenerateRandomPositionMap() {

        List<Vector2> allPositions = new List<Vector2>();
        for (int x = 0; x < mapScript.cols; x++) {
            for (int y = 0; y < mapScript.rows; y++) {
                allPositions.Add(new Vector2(x, y));
            }
        }

        // Mischen der Positionen
        System.Random rng = new System.Random();
        int n = allPositions.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            Vector2 value = allPositions[k];
            allPositions[k] = allPositions[n];
            allPositions[n] = value;
        }

        return allPositions;
    }

    // # float coverage
    // # float coherence
    public void Generate( TileData tileData, float coverage, float coherence) {

        if (tileData == null) {
            Debug.LogError("MapBuilder_Generate: tileData ist null.");
            return;
        }

        switch (tileData.TileType) {
            case TileType.Ground:
                // TODO Variance Erde 2 und 3 im inneren von 1
                // TODO Grafische transparenzen
                builderGround.Generate(tileData, coverage, coherence);
                break;
            case TileType.Asphalt:
                break;
            case TileType.Water:
                builderWater.Generate(tileData, coverage, coherence);
                break;
            case TileType.Ice:
                break;
            case TileType.FragileIce:
                break;
            case TileType.Snow:
                break;
            case TileType.PavingStone:
                break;
            case TileType.Object:
                break;
            case TileType.Gravel:
                break;
            case TileType.Desert:
                break;
            case TileType.SoftGround:
                break;
            case TileType.Mud:
                break;
            case TileType.Swamp:
                break;
            default:
                Debug.LogWarning("TileType nicht erkannt: " + tileData.TileType);
                break;
        }
}

    /*

    public void GenerateWaterTransitions() {
        List<GameObject> waterTiles = new List<GameObject>();
        int groundLayer = Utility.GetLayer(Utility.TileType.Ground);
        int watterLayer = Utility.GetLayer(Utility.TileType.Water);

        // Go Trouth the Map
        // TODO can by optimized by Adding the Waterfields in the list by the creation Step.
        for (int x = 0; x < mapScript.map.GetLength(0); x++) {
            for (int y = 0; y < mapScript.map.GetLength(1); y++) {

                // Water Tile Found
                if(mapScript.map[x,y, watterLayer] != null) {
                    mapScript.map[x, y, groundLayer].GetComponent<Tile>().passable = false;
                    waterTiles.Add(mapScript.map[x, y, groundLayer]);
                }
            }
        }

        // look on every Water Tile
        foreach (GameObject waterTile in waterTiles) {
            List<GameObject> neighbors = GetDryNeighborhood(waterTile);

            foreach (GameObject neighbor in neighbors) {
                CreateWaterTransition(neighbor);
            }
        }
    }
    
    private void CreateWaterTransition(GameObject updatedTile) {
        int x = (int)updatedTile.GetComponent<Tile>().position.x;
        int y = (int)updatedTile.GetComponent<Tile>().position.y;
        Vector3 transitionTilePosition = new Vector3(x, y, -0.01f);

        byte[] byteMap = new byte[4];
        //byteMap = new byte[] { Utility.TRANSPARENT, Utility.TRANSPARENT, Utility.TRANSPARENT, Utility.TRANSPARENT };
        byteMap = Utility.TRANSPARENT_BYTE();

        bool continueLoop = true;

        for (int verticalOffset = -1; verticalOffset < 2 && continueLoop; verticalOffset++) {
            for (int horizontalOffset = -1; horizontalOffset < 2 && continueLoop; horizontalOffset++) {

                // Speed Up by not Analysing his own position
                if(verticalOffset == 0 && horizontalOffset == 0) {
                    continue;
                }

                if ( NotOutOfMap(x + horizontalOffset, y + verticalOffset) ) {

                    // Skipp not Water nighbors
                    if (mapScript.map[x + horizontalOffset, y + verticalOffset, 1] == null) {
                        continue;
                    }

                    if ( mapScript.map[x + horizontalOffset, y + verticalOffset, 0].GetComponent<Tile>().passable == false ) { 

                        Utility.Direction direction = new Utility.Direction(horizontalOffset, verticalOffset);
                        byteMap = Utility.UpdateByteMapForTransition(Utility.WATER, byteMap, direction);
                    }
                }

                // Speeds Up because a full Water Field can not get even more Water.
                if (byteMap == Utility.WATER_BYTE()) {
                    continueLoop = false;
                }
            }
        }

        if (byteMap == Utility.TRANSPARENT_BYTE()) {
            return;
        }

        GameObject waterTransitionCullingTile = GameObject.Instantiate(this.mapScript.waterTilePrefab, this.mapScript.transform.position, Quaternion.identity) as GameObject;
        waterTransitionCullingTile.transform.SetParent(this.mapScript.transform);
        waterTransitionCullingTile.transform.position = transitionTilePosition;

        GameObject waterTransitionTile = waterTransitionCullingTile.GetComponent<Culling>().tile;
        waterTransitionTile.GetComponent<Water>().SetByteMap(byteMap);
        waterTransitionTile.GetComponent<Water>().GetComponent<Tile>().position = transitionTilePosition;

        this.mapScript.map[x, y, 1] = waterTransitionTile;

        if (byteMap == Utility.WATER_BYTE()) {
            waterTransitionTile.GetComponent<Water>().tileScript.passable = false;
            CreateWaterTransition(waterTransitionTile);
        }        
    }

    */

    public bool NotOutOfMap(int x, int y) {
        return !OutOfMap(x, y);
    }

    public bool OutOfMap(int x, int y) {
        if ( x < mapScript.map.GetLength(0) && x >= 0 && y < mapScript.map.GetLength(1) && y >= 0) {
            return false;
        } else {
            return true;
        }
    }

    public List<Vector2> GetAllNeighborCoordinates(int x, int y) {
        List<Vector2> neighborCoordinates = new List<Vector2>();

        for (int verticalOffset = -1; verticalOffset < 2; verticalOffset++) {
            for (int horizontalOffset = -1; horizontalOffset < 2; horizontalOffset++) {
                if (NotOutOfMap(x + horizontalOffset, y + verticalOffset)) {
                    neighborCoordinates.Add(new Vector2(x + horizontalOffset, y + verticalOffset));
                }
            }
        }

        neighborCoordinates.Remove(new Vector2(x, y));
        return neighborCoordinates;
    }

    public List<GameObject> GetAllNeighborTiles(int x, int y, int z) {
        List<GameObject> neighbors = new List<GameObject>();

        for (int verticalOffset = -1; verticalOffset < 2; verticalOffset++) {
            for (int horizontalOffset = -1; horizontalOffset < 2; horizontalOffset++) {
                if (NotOutOfMap(x + horizontalOffset, y + verticalOffset)) {
                    if(mapScript.map[x + horizontalOffset, y + verticalOffset, z] != null) {
                        neighbors.Add(mapScript.map[x + horizontalOffset, y + verticalOffset, z]);
                    }
                }
            }
        }

        neighbors.Remove(mapScript.map[x, y, z]);
        return neighbors;
    }

    public Vector2 GetBorderedDirection(int x, int y) {
        // corner
        if (x == 0 && y == 0) {
            return new Vector2(1.0f, 1.0f);
        } else if (x == (mapScript.map.GetLength(0) - 1) && y == (mapScript.map.GetLength(1) - 1)) {
            return new Vector2(-1.0f, -1.0f);
        } else if (x == 0 && y == (mapScript.map.GetLength(1) - 1)) {
            return new Vector2(1.0f, -1.0f);
        } else if (x == (mapScript.map.GetLength(0) - 1) && y == 0) {
            return new Vector2(-1.0f, 1.0f);

            // side
        } else if (x == 0) {
            return new Vector2(1.0f, 0.0f);
        } else if (x == (mapScript.map.GetLength(0) - 1)) {
            return new Vector2(-1.0f, 0.0f);
        } else if (y == 0) {
            return new Vector2(0.0f, 1.0f);
        } else {
            return new Vector2(0.0f, -1.0f);
        }
    }

    public List<GameObject> UpdateRiverBack(Vector2 direction, GameObject position) {
        List<GameObject> returnList = new List<GameObject>();

        if (position != null) {
            int xPosition = (int)position.GetComponent<Tile>().position.x;
            int yPosition = (int)position.GetComponent<Tile>().position.y;

            if (direction.x == 1 && direction.y == 1) {
                if (xPosition - 1 > 0) {
                    if (mapScript.map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == -1) {
                if (xPosition + 1 < mapScript.map.GetLength(0)) {
                    if (mapScript.map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == 0 && direction.y == 1) {

                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition - 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 0 && direction.y == -1) {

                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == 1) {
                if (xPosition + 1 < mapScript.map.GetLength(0)) {
                    if (mapScript.map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == -1) {
                if (xPosition - 1 >= 0) {
                    if (mapScript.map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition, 0]);
                    }
                }

                if (yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == 0) {
                if (xPosition - 1 >= 0 && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == 0) {
                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition - 1, 0]);
                    }
                }
            }
        }

        return returnList;
    }

    public List<GameObject> UpdateRiverSide(Vector2 direction, GameObject position) {
        List<GameObject> returnList = new List<GameObject>();

        if (position != null) {
            int xPosition = (int)position.GetComponent<Tile>().position.x;
            int yPosition = (int)position.GetComponent<Tile>().position.y;

            if (direction.x == -1 && direction.y == -1 || direction.x == 1 && direction.y == 1) {
                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition - 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition + 1, 0]);
                    }
                }
            } else if (direction.x == 0 && direction.y == -1 || direction.x == 0 && direction.y == 1) {

                if (xPosition + 1 < mapScript.map.GetLength(0)) {
                    if (mapScript.map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (xPosition - 1 >= 0) {
                    if (mapScript.map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition, 0]);
                    }
                }
            } else if (direction.x == 1 && direction.y == -1 || direction.x == -1 && direction.y == 1) {
                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition + 1, 0]);
                    }
                }
            } else if (direction.x == -1 && direction.y == 0 || direction.x == 1 && direction.y == 0) {
                if (yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition + 1, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition - 1, 0]);
                    }
                }
            }
        }

        return returnList;
    }

    public List<GameObject> UpdateRiverStrive(Vector2 direction, GameObject position) {
        List<GameObject> returnList = new List<GameObject>();

        if (position != null) {
            int xPosition = (int)position.GetComponent<Tile>().position.x;
            int yPosition = (int)position.GetComponent<Tile>().position.y;

            if (direction.x == -1 && direction.y == -1) {
                if (xPosition - 1 > 0) {
                    if (mapScript.map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == 1) {
                if (xPosition + 1 < mapScript.map.GetLength(0)) {
                    if (mapScript.map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == 0 && direction.y == -1) {

                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition - 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 0 && direction.y == 1) {

                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == -1) {
                if (xPosition + 1 < mapScript.map.GetLength(0)) {
                    if (mapScript.map[xPosition + 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition, 0]);
                    }
                }

                if (yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == 1) {
                if (xPosition - 1 >= 0) {
                    if (mapScript.map[xPosition - 1, yPosition, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition, 0]);
                    }
                }

                if (yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition, yPosition + 1, 0]);
                    }
                }

            } else if (direction.x == -1 && direction.y == 0) {
                if (xPosition - 1 >= 0 && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition - 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition - 1 >= 0 && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition - 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition - 1, yPosition - 1, 0]);
                    }
                }

            } else if (direction.x == 1 && direction.y == 0) {
                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition + 1 < mapScript.map.GetLength(1)) {
                    if (mapScript.map[xPosition + 1, yPosition + 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition + 1, 0]);
                    }
                }

                if (xPosition + 1 < mapScript.map.GetLength(0) && yPosition - 1 >= 0) {
                    if (mapScript.map[xPosition + 1, yPosition - 1, 1] == null) {
                        returnList.Add(mapScript.map[xPosition + 1, yPosition - 1, 0]);
                    }
                }
            }
        }

        return returnList;
    }

    public GameObject CreateWaterTile(GameObject referenceTile, Vector2 position) {
        if (NotOutOfMap((int)position.x, (int)position.y)) {

            GameObject newWaterCullingTile = GameObject.Instantiate(referenceTile, mapScript.transform);
            GameObject newWaterTile = newWaterCullingTile.GetComponent<Culling>().tile;

            newWaterCullingTile.transform.SetParent(this.mapScript.transform);
            newWaterCullingTile.transform.position = new Vector3(position.x, position.y, -0.01f);
            newWaterTile.GetComponent<Tile>().position = new Vector3(position.x, position.y, -0.01f);

            mapScript.map[(int)position.x, (int)position.y, 1] = newWaterTile;

            return newWaterTile;
        }

        return null;
    }

    public List<GameObject> GetDryNeighborhood(GameObject tile) {
        List<GameObject> neighbors = new List<GameObject>();
        int x = (int)tile.GetComponent<Tile>().position.x;
        int y = (int)tile.GetComponent<Tile>().position.y;

        for (int verticalOffset = -1; verticalOffset < 2; verticalOffset++) {
            for (int horizontalOffset = -1; horizontalOffset < 2; horizontalOffset++) {

                if(NotOutOfMap(x + horizontalOffset, y + verticalOffset)) {

                    if (mapScript.map[x + horizontalOffset, y + verticalOffset, 1] == null) {
                        neighbors.Add(mapScript.map[x + horizontalOffset, y + verticalOffset, 0]);
                    }
                }
            }
        }
        neighbors.Remove(mapScript.map[x, y, 0]);

        return neighbors;
    }

    /* 
    * Übergang zum Land machen
    * Hintergrundplane aufziehen
    * 
    * TODO weitere Wasserfelder generieren statt eine Plane aufzuziehen
    */
    public void BuildToIsland() {
        // Set Water Border
        for (int x = 0; x < this.mapScript.cols; x++) {
            SetNewWaterTile(x, 0);
            SetNewWaterTile(x, (this.mapScript.rows - 1));
        }

        for (int y = 0; y < this.mapScript.rows; y++) {
            SetNewWaterTile(0, y);
            SetNewWaterTile((this.mapScript.cols - 1), y);
        }

        // Set water Plane
        GameObject waterBackground = GameObject.Find("Background");
        waterBackground.GetComponent<SpriteRenderer>().size = new Vector2(this.mapScript.cols + 30f, this.mapScript.rows + 30f);
        waterBackground.transform.position = new Vector3(this.mapScript.cols / 2, this.mapScript.rows / 2, 4.0f);
    }

    private void SetNewWaterTile(int x, int y) {
        GameObject waterCullingTile = GameObject.Instantiate(this.mapScript.waterTilePrefab, this.mapScript.transform);
        GameObject waterTile = waterCullingTile.GetComponent<Culling>().tile;
        waterCullingTile.transform.SetParent(this.mapScript.transform);

        waterCullingTile.transform.position = new Vector3(x, y, -0.01f);
        waterTile.GetComponent<Tile>().position = new Vector3(x, y, -0.01f);

        for (int i = 0; i < this.mapScript.layer; i++) {
            GameObject tile = this.mapScript.map[x, y, i];
            if (tile != null) {
                GameObject.Destroy(tile.transform.parent.gameObject);
            }  
        }

        this.mapScript.map[x, y, 1] = waterTile;
    }

    public List<GameObject> GetBorderedTiles() {
        List<GameObject> borderedTiles = new List<GameObject>();
        int groundLayer = 0;

        borderedTiles.Add(mapScript.map[0, 0, groundLayer]);
        borderedTiles.Add(mapScript.map[0, mapScript.rows - 1, groundLayer]);
        borderedTiles.Add(mapScript.map[mapScript.cols - 1, 0, groundLayer]);
        borderedTiles.Add(mapScript.map[mapScript.cols - 1, mapScript.rows - 1, groundLayer]);

        for (int x = 1; x < (mapScript.cols - 2); x++) {
            borderedTiles.Add(mapScript.map[x, 0, groundLayer]);
            borderedTiles.Add(mapScript.map[x, mapScript.rows - 1, groundLayer]);
        }

        for (int y = 1; y < (mapScript.rows - 2); y++) {
            borderedTiles.Add(mapScript.map[0, y, groundLayer]);
            borderedTiles.Add(mapScript.map[mapScript.cols - 1, y, groundLayer]);
        }

        return borderedTiles;
    }

    public List<GameObject> DeleteAllWaterTiles(List<GameObject> tileList) {
        int waterLayer = 1;

        for (int i = tileList.Count - 1; i >= 0; i--) {
            if(tileList[i] != null) {
                Vector2 tilePosition = tileList[i].GetComponent<Tile>().position;

                if (mapScript.map[(int)tilePosition.x, (int)tilePosition.y, waterLayer] != null) {
                    tileList.RemoveAt(i);
                }
            }
        }

        return tileList;
    }













    public bool SaveToUse(int x, int y, int z, out ByteMapping byteMapping) {
        byteMapping = null;
        GameObject field = mapScript.map[x, y, z];

        if (field != null) {
            GameObject culledTile = field.GetComponent<Culling>().tile;
            if (culledTile != null) {
                Tile tile = culledTile.GetComponent<Tile>();
                if (tile != null) {
                    byteMapping = tile.byteMap;
                    if (byteMapping != null) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void GenerateGroundTransition(TileData tileData, List<GameObject> solidTileList) {
        // Once for each earth tile on the map (full tile not mutated earth tile "Made to earth by neighbors")
        foreach (GameObject solidCulledTile in solidTileList) {
            GameObject solidTile = solidCulledTile.GetComponent<Culling>().tile;
            Vector2 pos = solidTile.transform.position;

            int x = (int)pos.x;
            int y = (int)pos.y;
            int layer = (int)tileData.LayerType;

            List<Vector2> transitionNeighborsPosition = GetAllNeighborCoordinates(x, y);

            foreach (GameObject culledDuplicates in solidTileList) {
                GameObject duplicates = culledDuplicates.GetComponent<Culling>().tile;

                if (duplicates != null) {
                    Vector2 duplicatesPosition = duplicates.transform.position;
                    if (duplicatesPosition != null) {
                        transitionNeighborsPosition.Remove(duplicatesPosition);
                    }
                }
            }

            // Consider all neighbors from every earth field
            foreach (Vector2 neighborPosition in transitionNeighborsPosition) {
                x = (int)neighborPosition.x;
                y = (int)neighborPosition.y;

                GameObject field = mapScript.map[x, y, layer];

                if (field == null) {
                    //Das übergangsfeld ist leer und sollte vor der Manipulation erzeugt werden.
                    GameObject tilePattern = InstantiateToTilePattern(tileData);
                    tilePattern.transform.position = neighborPosition;

                    GameObject neighborCulledTile = tilePattern.GetComponent<Culling>().tile;
                    Tile neighborTile = neighborCulledTile.GetComponent<Tile>();
                    neighborTile.byteMap = UpdateByteMap(x, y, layer);
                    string spriteByteMapCode = ConvertByteMapToStringCode(neighborTile.byteMap);
                    string spriteName = UpdateSpriteName(tileData.Sprite.name, spriteByteMapCode);

                    // Set Sprite
                    neighborCulledTile.GetComponent<SpriteRenderer>().sprite = Utility.GetTileDataByName(spriteName).Sprite;
                    mapScript.map[x, y, layer] = tilePattern;
                }
            }
        }
    }

    public string UpdateSpriteName(string spriteName, string spriteByteMapCode) {
        string[] parts = spriteName.Split('_');

        // Zwecks Debug
        //parts[0] = "Water";
        parts[1] = spriteByteMapCode;

        return string.Join("_", parts);
    }

    public string ConvertByteMapToStringCode(ByteMapping byteMap) {
        // Du könntest hier auch StringBuilder verwenden, wenn die Leistung eine Rolle spielt
        string code = "";

        // Je nachdem, wie du die Reihenfolge wünschst
        if (byteMap.topLeft) code += "TL";
        if (byteMap.topRight) code += "TR";
        if (byteMap.bottomLeft) code += "BL";
        if (byteMap.bottomRight) code += "BR";

        return code;
    }

    public ByteMapping UpdateByteMap(int x, int y, int layer) {
        ByteMapping tmpBuyteMap = new ByteMapping();
        ByteMapping returnValue = tmpBuyteMap;
        // Left tile Check
        if ((x - 1) >= 0) {
            if (SaveToUse(x - 1, y, layer, out tmpBuyteMap)) {
                returnValue.topLeft = returnValue.topLeft || tmpBuyteMap.topRight;
                returnValue.bottomLeft = returnValue.bottomLeft || tmpBuyteMap.bottomRight;
            }
        }

        // Right tile Check
        if ((x + 1) < mapScript.map.GetLength(0)) {
            if (SaveToUse(x + 1, y, layer, out tmpBuyteMap)) {
                returnValue.topRight = returnValue.topRight || tmpBuyteMap.topLeft;
                returnValue.bottomRight = returnValue.bottomRight || tmpBuyteMap.bottomLeft;
            }
        }

        // Top tile Check
        if ((y + 1) < mapScript.map.GetLength(1)) {
            if (SaveToUse(x, y + 1, layer, out tmpBuyteMap)) {
                returnValue.topLeft = returnValue.topLeft || tmpBuyteMap.bottomLeft;
                returnValue.topRight = returnValue.topRight || tmpBuyteMap.bottomRight;
            }
        }

        // Buttom tile Check
        if ((y - 1) >= 0) {
            if (SaveToUse(x, y - 1, layer, out tmpBuyteMap)) {
                returnValue.bottomLeft = returnValue.bottomLeft || tmpBuyteMap.topLeft;
                returnValue.bottomRight = returnValue.bottomRight || tmpBuyteMap.topRight;
            }
        }

        // Top Left
        if ((x - 1) >= 0 && (y + 1) < mapScript.map.GetLength(1)) {
            if (SaveToUse(x - 1, y + 1, layer, out tmpBuyteMap)) {
                returnValue.topLeft = returnValue.topLeft || tmpBuyteMap.bottomRight;
            }
        }

        // Button Left
        if ((x - 1) >= 0 && (y - 1) >= 0) {
            if (SaveToUse(x - 1, y - 1, layer, out tmpBuyteMap)) {
                returnValue.bottomLeft = returnValue.bottomLeft || tmpBuyteMap.topRight;
            }
        }

        // Top Right
        if ((x + 1) < mapScript.map.GetLength(0) && (y + 1) < mapScript.map.GetLength(1)) {
            if (SaveToUse(x + 1, y + 1, layer, out tmpBuyteMap)) {
                returnValue.topRight = returnValue.topRight || tmpBuyteMap.bottomLeft;
            }
        }

        // Buttom Right
        if ((x + 1) < mapScript.map.GetLength(0) && (y - 1) >= 0) {
            if (SaveToUse(x + 1, y - 1, layer, out tmpBuyteMap)) {
                returnValue.bottomRight = returnValue.bottomRight || tmpBuyteMap.topLeft;
            }
        }

        return returnValue;
    }
}

