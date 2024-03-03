using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

public static class Utility {


    //public GUI GUI;

    public static byte[] ConvertToByteArray(ByteMapping byteMapping) {
        return new byte[] {
            Convert.ToByte(byteMapping.topLeft),
            Convert.ToByte(byteMapping.topRight),
            Convert.ToByte(byteMapping.bottomLeft),
            Convert.ToByte(byteMapping.bottomRight)
        };
    }

    //So werden die sachen erst geladen, wenn es tatsächlich benötigt werden.
    private static TileData _earthTile;
    private static TileData _grassTile;
    private static TileData _snowTile;
    private static TileData _waterTile;
    private static TileData _fragilIceTile;
    private static TileData _iceTile;
    private static TileData _desertTile;
    private static TileData _asphaltTile;

    public static TileData EARTH_TILE {
        get {
            if (_earthTile == null) {
                _earthTile = GetTileDataByName("Earth_TLTRBLBR_V1_A1");
            }
            return _earthTile;
        }
    }
    public static TileData GRASS_TILE {
        get {
            if (_grassTile == null) {
                _grassTile = GetTileDataByName("Grass_TLTRBLBR_V1_A1");
            }
            return _grassTile;
        }
    }
    public static TileData SNOW_TILE {
        get {
            if (_snowTile == null) {
                _snowTile = GetTileDataByName("Snow_TLTRBLBR_V1_A1");
            }
            return _snowTile;
        }
    }
    public static TileData WATER_TILE {
        get {
            if (_waterTile == null) {
                _waterTile = GetTileDataByName("Water_TLTRBLBR_V1_A1");
            }
            return _waterTile;
        }
    }
    public static TileData FRAGIL_ICE_TILE {
        get {
            if (_fragilIceTile == null) {
                _fragilIceTile = GetTileDataByName("FragileIce_TLTRBLBR_V1_A1");
            }
            return _fragilIceTile;
        }
    }
    public static TileData ICE_TILE {
        get {
            if (_iceTile == null) {
                _iceTile = GetTileDataByName("Ice_TLTRBLBR_V1_A1");
            }
            return _iceTile;
        }
    }
    public static TileData DESERT_TILE {
        get {
            if (_desertTile == null) {
                _desertTile = GetTileDataByName("Desert_TLTRBLBR_V1_A1");
            }
            return _desertTile;
        }
    }
    public static TileData ASPHALT_TILE {
        get {
            if (_asphaltTile == null) {
                _asphaltTile = GetTileDataByName("Asphalt_TLTRBLBR_V1_A1");
            }
            return _asphaltTile;
        }
    }

    public static TileData GetTileDataByName(string tileName) {
        return MapAtlas.Instance.GetTileDataByName(tileName);
    }

    public static void SetDataToTile(GameObject culledTile, TileData tileData) {
        culledTile.GetComponent<SpriteRenderer>().sprite = tileData.Sprite;
        Tile tile = culledTile.GetComponent<Tile>();

        tile.layerType = tileData.LayerType;
        tile.sprite = tileData.Sprite;
        tile.byteMap = tileData.ByteMap;
        tile.tileType = tileData.TileType;
        tile.isPassable = tileData.IsPassable;
        tile.slowDownFactor = tileData.SlowDownFactor;
        tile.soundEffects = tileData.SoundEffects;
    }

    public static TileData GetDataFromTile(GameObject field) {
        GameObject culledTile = field.GetComponent<Culling>().tile;
        Tile tile = culledTile.GetComponent<Tile>();

        return new TileData(
            tile.layerType,
            culledTile.GetComponent<SpriteRenderer>().sprite,
            tile.byteMap,
            tile.tileType,
            tile.isPassable,
            tile.slowDownFactor,
            tile.soundEffects
        );
    }

    /*                 Rain                           Rain
     *      Ground      ->     SoftGround      Mud     ->    Swamp
     *  Lawn            ->      Wet Lawn
     *      Drive
     *       ->  Earth  ->  ->  ->  ->  ->  Wet Earth
     *       
     *       
     */

    public enum Side { Up, Down, Left, Right };

    public enum WindRose { N, NO, O, SO, S, SW, W, NW };

    public struct Direction {
        public int x;
        public int y;
        public WindRose windRose;

        public Direction(WindRose windRose) {
                this.windRose = windRose;

                switch (windRose) {
                    case WindRose.N:
                        this.x = 0;
                        this.y = 1;
                        break;
                    case WindRose.O:
                        this.x = 1;
                        this.y = 0;
                        break;
                    case WindRose.SO:
                        this.x = 1;
                        this.y = -1;
                        break;
                    case WindRose.S:
                        this.x = 0;
                        this.y = -1;
                        break;
                    case WindRose.SW:
                        this.x = -1;
                        this.y = -1;
                        break;
                    case WindRose.W:
                        this.x = -1;
                        this.y = 0;
                        break;
                    case WindRose.NW:
                        this.x = -1;
                        this.y = 1;
                        break;
                    default:
                        this.x = 1;
                        this.y = 1;
                        break;
                }
            }

        public Direction(int x, int y) {
            this.x = x;
            this.y = y;

            if (x == -1 && y == -1) {
                windRose = WindRose.SW;
            } else if (x == -1 && y == 0) {
                windRose = WindRose.W;
            } else if (x == -1 && y == 1) {
                windRose = WindRose.NW;
            } else if (x == 0 && y == -1) {
                windRose = WindRose.S;
            } else if (x == 0 && y == 1) {
                windRose = WindRose.N;
            } else if (x == 1 && y == -1) {
                windRose = WindRose.SO;
            } else if (x == 1 && y == 0) {
                windRose = WindRose.O;
            } else {
                windRose = WindRose.NO;
            }
        }
    }

    public static byte[] UpdateByteMapForTransition(byte tileType, byte[] updatedByte, Direction direction) {

        switch (direction.windRose) {
            case WindRose.N:
                updatedByte[0] = tileType;
                updatedByte[1] = tileType;
                break;
            case WindRose.O:
                updatedByte[1] = tileType;
                updatedByte[3] = tileType;
                break;
            case WindRose.SO:
                updatedByte[3] = tileType;
                break;
            case WindRose.S:
                updatedByte[2] = tileType;
                updatedByte[3] = tileType;
                break;
            case WindRose.SW:
                updatedByte[2] = tileType;
                break;
            case WindRose.W:
                updatedByte[0] = tileType;
                updatedByte[2] = tileType;
                break;
            case WindRose.NW:
                updatedByte[0] = tileType;
                break;
            default:
                updatedByte[1] = tileType;
                break;
        }

        return updatedByte;
    }

    public static void Victory() {
        // Handle Victory condition
        Debug.Log("Spiel gewonnen!");
        QuitGame();
    }

    public static void GameOver() {
        // Handle Victory condition
        Debug.Log("Spiel verloren!");
        QuitGame();
    }

    public static void QuitGame() {
        // save any game data here
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                    Application.Quit();
        #endif
    }
}
