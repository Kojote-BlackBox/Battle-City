using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderGround {

    private MapBuilder mapBuilderScript;
    private Map mapScript;

    public BuilderGround(MapBuilder origin) {
        this.mapBuilderScript = origin;
        this.mapScript = mapBuilderScript.mapScript;
    }

    // Main Funktion of the the Class
    public void Generate(MapBuilder.BuildSet ground) {

        // Holder for Transition Work
        List<GameObject> transitionList = new List<GameObject>();

        int toCover = (int)(mapScript.cols * mapScript.rows * ground.coverage);
        int covered = 0;

        int earthSpriteID = Utility.GetSpriteIDByByteMap(Utility.EARTH_BYTE());

        while (covered < toCover) {
            int x = Random.Range(0, mapScript.cols);
            int y = Random.Range(0, mapScript.rows);

            GameObject tile = mapScript.map[x, y, ground.layer];

            if (tile.GetComponent<SpriteRenderer>().sprite != mapScript.mapSprites[earthSpriteID]) {

                // Set first Random Earth Tile
                tile.GetComponent<SpriteRenderer>().sprite = mapScript.mapSprites[earthSpriteID]; // Earth
                mapBuilderScript.SetByteMap(tile, earthSpriteID);
                mapBuilderScript.SetTileType(tile, ground.groundType);

                // Create a list of Nighbors (increased chanse by coherence for this fields)
                List<GameObject> neighbors = new List<GameObject>();
                neighbors = mapBuilderScript.GetNeighborhood(neighbors, x, y); // Neighbors based on ground layer 0

                // Depend on coherence build 8er blocks of dirt
                foreach (GameObject neighbor in neighbors) {
                    if ( ground.coherence >= Random.Range(0.0f, 1.0f) ) {

                        GameObject tmpTile = mapScript.map[(int)mapBuilderScript.GetTilePosition(neighbor).x, (int)mapBuilderScript.GetTilePosition(neighbor).y, ground.layer];
                        tmpTile.GetComponent<SpriteRenderer>().sprite = mapScript.mapSprites[earthSpriteID]; // Earth
                        mapBuilderScript.SetByteMap(tmpTile, earthSpriteID);
                        mapBuilderScript.SetTileType(tmpTile, ground.groundType);
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
        }

        GenerateGroundTransition(transitionList);
    }

    private void GenerateGroundTransition(List<GameObject> transitionList) {
        int layer = 0;

        // Once for each earth tile on the map (full tile not mutated earth tile "Made to earth by neighbors")
        foreach (GameObject transition in transitionList) {
            int x = (int)mapBuilderScript.GetTilePosition(transition).x;
            int y = (int)mapBuilderScript.GetTilePosition(transition).y;

            List<GameObject> transitionNeighbors = new List<GameObject>();
            transitionNeighbors = mapBuilderScript.GetNeighborhood(transitionNeighbors, x, y); // Neighbors based on Ground layer 0

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
                    checkByte = mapScript.map[xPosition - 1, yPosition, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[1] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[0] = Utility.EARTH;
                    }

                    if (checkByte[3] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[2] = Utility.EARTH;
                    }
                }

                // Right tile Check
                if ((xPosition + 1) < this.mapScript.map.GetLength(0)) {
                    checkByte = this.mapScript.map[xPosition + 1, yPosition, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[0] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[1] = Utility.EARTH;
                    }

                    if (checkByte[2] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[3] = Utility.EARTH;
                    }
                }

                // Top tile Check
                if ((yPosition + 1) < this.mapScript.map.GetLength(1)) {
                    checkByte = mapScript.map[xPosition, yPosition + 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[2] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[0] = Utility.EARTH;
                    }

                    if (checkByte[3] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[1] = Utility.EARTH;
                    }
                }

                // Buttom tile Check
                if ((yPosition - 1) >= 0) {
                    checkByte = mapScript.map[xPosition, yPosition - 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[0] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[2] = Utility.EARTH;
                    }

                    if (checkByte[1] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[3] = Utility.EARTH;
                    }
                }

                // Top Left
                if ((xPosition - 1) >= 0 && (yPosition + 1) < this.mapScript.map.GetLength(1)) {
                    checkByte = mapScript.map[xPosition - 1, yPosition + 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[3] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[0] = Utility.EARTH;
                    }
                }

                // Button Left
                if ((xPosition - 1) >= 0 && (yPosition - 1) >= 0) {
                    checkByte = mapScript.map[xPosition - 1, yPosition - 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[1] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[2] = Utility.EARTH;
                    }
                }

                // Top Right
                if ((xPosition + 1) < this.mapScript.map.GetLength(0) && (yPosition + 1) < this.mapScript.map.GetLength(1)) {
                    checkByte = mapScript.map[xPosition + 1, yPosition + 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[2] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[1] = Utility.EARTH;
                    }
                }

                // Buttom Right
                if ((xPosition + 1) < this.mapScript.map.GetLength(0) && (yPosition - 1) >= 0) {
                    checkByte = mapScript.map[xPosition + 1, yPosition - 1, layer].GetComponent<Tile>().byteMap;

                    if (checkByte[0] == Utility.EARTH) {
                        checkZone.GetComponent<Tile>().byteMap[3] = Utility.EARTH;
                    }
                }

                // Set Sprite
                byte[] byteTile = checkZone.GetComponent<Tile>().byteMap;
                int spriteID = Utility.GetSpriteIDByByteMap(byteTile);
                checkZone.GetComponent<SpriteRenderer>().sprite = mapScript.mapSprites[spriteID];
            }
        }
    }
}
