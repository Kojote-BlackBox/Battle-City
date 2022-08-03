using UnityEngine;
using System.Collections.Generic;

public class MapBuilder : MonoBehaviour {

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
    
    // Attribute Set for VariateGeneratedLayer function
    public struct BuildSet {
        public int layer;
        public Utility.TileType groundType;
        public float coverage;
        public float coherence;

        public BuildSet(Utility.TileType groundType, float coverage, float coherence) {
            this.groundType = groundType;

            if( this.groundType == Utility.TileType.Water || this.groundType == Utility.TileType.Asphalt ) {
                this.layer = 1;
            } else {
                this.layer = 0;
            }

            this.coverage = coverage;

            this.coherence = coherence;
            if( this.groundType == Utility.TileType.Water ) {
                if(this.coherence > 0.3f) {
                    this.coherence = 0.3f;
                }
            }
        }
    }

    public MapBuilder(GameObject origin) {
        this.mapScript = origin.GetComponent<Map>();
        this.generatorTile = new GeneratorTile(this);

        builderGround = new BuilderGround(this);
        builderSoftGround = new BuilderSoftGround(this);
        builderMud = new BuilderMud(this);
        builderDesert = new BuilderDesert(this);
        builderAsphalt = new BuilderAsphalt(this);
        builderWater = new BuilderWater(this);
        builderIce = new BuilderIce(this);
        builderFragileIce = new BuilderFragileIce(this);
        builderSwamp = new BuilderSwamp(this);       
    }


    // TODO
    /*
     * Die erzeugung von Tiles (aller arten) Ausgliedern Generallesieren.
     * Zu jedem Utility.TileType hier eine Publice Liste Machen
     * Wasser Transition über eine Wasserliste optimieren
     */
    public void FillMapWithTile(byte tileType) {
        // Define Default Base Ground tile
        GameObject referenceCullingTile = Instantiate(mapScript.tilePrefab, mapScript.transform.position, Quaternion.identity) as GameObject;
        GameObject referenceTile = referenceCullingTile.GetComponent<Culling>().tile;
        int layer = 0;

        byte[] tileByte = Utility.TileTypeToTilyByte(tileType);
        int lawnSpriteID = Utility.GetSpriteIDByByteMap(tileByte);
        referenceTile.GetComponent<SpriteRenderer>().sprite = mapScript.mapSprites[lawnSpriteID]; // Lawn

        //Set Byte Map 
        int tileByteMapSize = referenceTile.GetComponent<Tile>().byteMap.Length;
        for (int i = 0; i < tileByteMapSize; i++) {
            referenceTile.GetComponent<Tile>().byteMap[i] = Utility.BYTE_MAP[lawnSpriteID, i];
        } 

        SetTileType(referenceTile, Utility.TileType.Ground);

        for (int l = 0; l < mapScript.layer; l++) {
            for (int y = 0; y < mapScript.rows; y++) {
                for (int x = 0; x < mapScript.cols; x++) {

                    // Generate Base Ground Layer
                    if (l == 0) {
                        GameObject cullingTile = (GameObject)Instantiate(referenceCullingTile, mapScript.transform);
                        GameObject tile = cullingTile.GetComponent<Culling>().tile;
                        cullingTile.transform.SetParent(mapScript.transform);

                        cullingTile.transform.position = new Vector2(x, y);
                        tile.GetComponent<Tile>().position = new Vector2(x, y);

                        mapScript.map[x, y, layer] = tile;

                    } else {
                        mapScript.map[x, y, l] = null;
                    }
                }
            }
        }

        Destroy(referenceCullingTile);
    }

    public void SetTileType(GameObject tile, Utility.TileType type) {

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

    public void Generate( BuildSet buildType ) {
        Utility.TileType groundType = buildType.groundType;

        switch (groundType) {
            case Utility.TileType.Ground:
                builderGround.Generate(buildType);
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
                builderWater.Generate(buildType);
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

        GameObject waterTransitionCullingTile = Instantiate(this.mapScript.waterTilePrefab, this.mapScript.transform.position, Quaternion.identity) as GameObject;
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

    public void SetByteMap(GameObject tile, int position) {
        int maxLength = tile.GetComponent<Tile>().byteMap.Length;

        for (int i = 0; i < maxLength; i++) {
            tile.GetComponent<Tile>().byteMap[i] = Utility.BYTE_MAP[position, i];
        }
    }

    public List<GameObject> GetNeighborhood(List<GameObject> neighbors, int x, int y) {
        for (int verticalOffset = -1; verticalOffset < 2; verticalOffset++) {
            for (int horizontalOffset = -1; horizontalOffset < 2; horizontalOffset++) {
                if (NotOutOfMap(x + horizontalOffset, y + verticalOffset)) {
                    neighbors.Add(mapScript.map[x + horizontalOffset, y + verticalOffset, 0]);
                }
            }
        }
        neighbors.Remove(mapScript.map[x, y, 0]);

        return neighbors;
    }

    public Vector2 GetTilePosition(GameObject tile) {
        return tile.GetComponent<Tile>().position;
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

            GameObject newWaterCullingTile = (GameObject)Instantiate(referenceTile, mapScript.transform);
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
        // Set first bordered Water tile
        int layer = Utility.GetLayer(Utility.TileType.Water);
        List<GameObject> transitionList = new List<GameObject>();

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
        GameObject waterCullingTile = (GameObject)Instantiate(this.mapScript.waterTilePrefab, this.mapScript.transform);
        GameObject waterTile = waterCullingTile.GetComponent<Culling>().tile;
        waterCullingTile.transform.SetParent(this.mapScript.transform);

        waterCullingTile.transform.position = new Vector3(x, y, -0.01f);
        waterTile.GetComponent<Tile>().position = new Vector3(x, y, -0.01f);

        for (int i = 0; i < this.mapScript.layer; i++) {
            GameObject tile = this.mapScript.map[x, y, i];
            if (tile != null) {
                Destroy(tile.transform.parent.gameObject);
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
}

