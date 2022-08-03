using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderWater : MonoBehaviour {

    private MapBuilder mapBuilderScript;
    private Map mapScript;

    public BuilderWater(MapBuilder origin) {
        this.mapBuilderScript = origin;
        this.mapScript = mapBuilderScript.mapScript;
    }

    public void Generate(MapBuilder.BuildSet water) {

        // Holder for Transition Work
        List<GameObject> transitionList = new List<GameObject>();

        int toCover = (int)(mapScript.cols * mapScript.rows * water.coverage);
        int covered = 0;

        while (covered < toCover) {
            // Set first Random Water Tile on the Map border

            GameObject borderedTile = GetBorderedTile();
            if(borderedTile == null) {
                break;
            }
            

            int x = (int)borderedTile.GetComponent<Tile>().position.x;
            int y = (int)borderedTile.GetComponent<Tile>().position.y;

            if (mapBuilderScript.NotOutOfMap(x,y) && mapScript.map[x, y, water.layer] == null ) {

                // Set first bordered Water tile
                GameObject waterCullingTile = (GameObject)Instantiate(mapScript.waterTilePrefab, mapScript.transform);
                GameObject waterTile = waterCullingTile.GetComponent<Culling>().tile;
                waterCullingTile.transform.SetParent(mapScript.transform);

                waterCullingTile.transform.position = new Vector3(x, y, -0.01f);
                waterTile.GetComponent<Tile>().position = new Vector3(x, y, -0.01f);

                mapScript.map[x, y, water.layer] = waterTile;
                mapScript.map[x, y, Utility.GetLayer(Utility.TileType.Ground)].GetComponent<Tile>().passable = false;
                transitionList.Add(waterTile);
                covered++;

                // Set the initial direction for the first water/river tile
                Vector2 riverDirection = mapBuilderScript.GetBorderedDirection(x, y);

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
                    if (mapBuilderScript.OutOfMap(x,y)) {
                        break;
                    }

                    // if the neighbor fileds are inside the border of the map
                    if (mapBuilderScript.NotOutOfMap(xRange, yRange)) {

                        // Initiate all the nighbors of the current river field.
                        backList = mapBuilderScript.UpdateRiverBack(riverDirection, mapScript.map[x, y, 0]);
                        sideList = mapBuilderScript.UpdateRiverSide(riverDirection, mapScript.map[x, y, 0]);
                        striveList = mapBuilderScript.UpdateRiverStrive(riverDirection, mapScript.map[x, y, 0]);

                        //  Dont Try to work outside of map
                    } else {
                        break;
                    }

                    // Get neighbors with coherence wights
                    float nextRiverTile = Random.Range(0.0f, (1.0f - breakChance));
                    bool forward = true;

                    // Set Coherence dependent chances
                    float back = 1.0f;
                    float side = back + water.coherence;                                  // min(1.0f) max(2.0f)
                    float strive = side + 2 * water.coherence;                            // min(1.0f) max(4.0f)
                    float front = strive + 4 * water.coherence;                           // min(1.0f) max(8.0f)
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

                    } else if (forward) {
                        newTilePosition = riverDirection + mapScript.map[x, y, 0].GetComponent<Tile>().position;

                        // Check index outside map
                        if(mapBuilderScript.OutOfMap((int)newTilePosition.x, (int)newTilePosition.y)) {
                            break;
                        }

                        forward = false;

                        // Just in case of
                    } else {
                        break;
                    }

                    if (mapScript.map[(int)newTilePosition.x, (int)newTilePosition.y, 1] == null) {
                        transitionList.Add( mapBuilderScript.CreateWaterTile(mapScript.waterTilePrefab, newTilePosition) );
                        covered++;
                        forward = true;

                        Vector2 oldPosition = new Vector2(x, y);
                        riverDirection = newTilePosition - oldPosition;

                        // Update current position. Jumpt to the nex field on the next iteration.
                        x = (int)newTilePosition.x;
                        y = (int)newTilePosition.y;

                    } else {
                        backList.Remove(mapScript.map[(int)newTilePosition.x, (int)newTilePosition.y, 1]);
                        sideList.Remove(mapScript.map[(int)newTilePosition.x, (int)newTilePosition.y, 1]);
                        striveList.Remove(mapScript.map[(int)newTilePosition.x, (int)newTilePosition.y, 1]);
                    }

                    breakValue = Random.Range(0.0f, 1.0f);
                }
            }
        }
    }

    private GameObject GetBorderedTile() {  
        List<GameObject> borderedTiles = mapBuilderScript.GetBorderedTiles();
        borderedTiles = mapBuilderScript.DeleteAllWaterTiles(borderedTiles);

        if (borderedTiles.Count != 0) {
            int randomIndex = Random.Range(0, borderedTiles.Count + 1);

            return borderedTiles[randomIndex];
        } else {
            return null;
        }
    }
}
