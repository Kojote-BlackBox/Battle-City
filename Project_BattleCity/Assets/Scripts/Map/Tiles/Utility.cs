using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    //public GUI GUI;

    public const byte LAWN = 0;
    public const byte EARTH = 1;
    public const byte WATER = 2;
    public const byte TRANSPARENT = 10;

    public static byte[] LAWN_BYTE() {
        return new byte[] { LAWN, LAWN, LAWN, LAWN };
    }

    public static byte[] EARTH_BYTE() {
        return new byte[] { EARTH, EARTH, EARTH, EARTH };
    }

    public static byte[] WATER_BYTE() {
        return new byte[] { WATER, WATER, WATER, WATER };
    }

    public static byte[] TRANSPARENT_BYTE() {
        return new byte[] { TRANSPARENT, TRANSPARENT, TRANSPARENT, TRANSPARENT };
    }

    /* byteMap Code on Tile.cs
     * --------
     * | 0  1 |
     * | 2  3 |
     * --------
     */
    public static byte[,] BYTE_MAP = new byte[,] {
        {WATER, TRANSPARENT, TRANSPARENT, WATER},
        {TRANSPARENT, WATER, WATER, TRANSPARENT},
        {EARTH, EARTH, EARTH, LAWN},
        {EARTH, EARTH, LAWN, LAWN},
        {EARTH, EARTH, LAWN, EARTH},
        {LAWN, LAWN, LAWN, EARTH},
        {EARTH, LAWN, LAWN, EARTH},
        {LAWN, LAWN, EARTH, LAWN},
        {TRANSPARENT, WATER, WATER, TRANSPARENT},
        {WATER, TRANSPARENT, TRANSPARENT, WATER},
        {EARTH, LAWN, EARTH, LAWN},
        {LAWN, LAWN, LAWN, LAWN},
        {LAWN, EARTH, LAWN, EARTH},
        {EARTH, EARTH, EARTH, EARTH},
        {TRANSPARENT, WATER, WATER, WATER},
        {WATER, TRANSPARENT, WATER, WATER},
        {EARTH, LAWN, EARTH, EARTH},
        {LAWN, LAWN, EARTH, EARTH},
        {LAWN, EARTH, EARTH, EARTH},
        {LAWN, EARTH, LAWN, LAWN},
        {LAWN, EARTH, EARTH, LAWN},
        {EARTH, LAWN, LAWN, LAWN},
        {WATER, WATER, TRANSPARENT, WATER},
        {WATER, WATER, WATER, TRANSPARENT},
        {TRANSPARENT, TRANSPARENT, TRANSPARENT, WATER},
        {TRANSPARENT, TRANSPARENT, WATER, WATER},
        {TRANSPARENT, TRANSPARENT, WATER, TRANSPARENT},
        {TRANSPARENT, TRANSPARENT, TRANSPARENT, WATER},
        {TRANSPARENT, TRANSPARENT, WATER, WATER},
        {TRANSPARENT, TRANSPARENT, WATER, TRANSPARENT},
        {TRANSPARENT, WATER, WATER, WATER},
        {WATER, TRANSPARENT, WATER, WATER},
        {TRANSPARENT, WATER, TRANSPARENT, WATER},
        {WATER, WATER, WATER, WATER},
        {WATER, TRANSPARENT, WATER, TRANSPARENT},
        {TRANSPARENT, WATER, TRANSPARENT, WATER},
        {WATER, WATER, WATER, WATER},
        {WATER, TRANSPARENT, WATER, TRANSPARENT},
        {WATER, WATER, TRANSPARENT, WATER},
        {WATER, WATER, WATER, TRANSPARENT},
        {TRANSPARENT, WATER, TRANSPARENT, TRANSPARENT},
        {WATER, WATER, TRANSPARENT, TRANSPARENT},
        {WATER, TRANSPARENT, TRANSPARENT, TRANSPARENT},
        {TRANSPARENT, WATER, TRANSPARENT, TRANSPARENT},
        {WATER, WATER, TRANSPARENT, TRANSPARENT},
        {WATER, TRANSPARENT, TRANSPARENT, TRANSPARENT}
    };


    /*                 Rain                           Rain
     *      Ground      ->     SoftGround      Mud     ->    Swamp
     *  Lawn            ->      Wet Lawn
     *      Drive
     *       ->  Earth  ->  ->  ->  ->  ->  Wet Earth
     *       
     *       
     */
    public enum TileType { Ground, SoftGround, Mud, Desert, Asphalt, Water, Ice, FragileIce, Swamp, Snow };

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

    public static int GetLayer(TileType tileType) {
        int returnValue = -1;

        switch (tileType) {
            case (TileType.Asphalt):
            case (TileType.Desert):
            case (TileType.FragileIce):
            case (TileType.Ground):
            case (TileType.Ice):
            case (TileType.Mud):
            case (TileType.SoftGround):
            case (TileType.Swamp):
                returnValue = 0;
                break;
            case (TileType.Water):
                returnValue = 1;
                break;
        }

        return returnValue;
    }

    public static int GetSpriteIDByByteMap(byte[] byteMap) {

        int counter = 0;
        int spriteID = 0;

        for (int x = 0; x < BYTE_MAP.GetLength(0); x++) {
            for (int y = 0; y < BYTE_MAP.GetLength(1); y++) {

                if (byteMap[y] != BYTE_MAP[x, y]) {
                    counter = 0;
                    break;
                } else {
                    counter++;
                }
            }
            if (counter == 4) {
                spriteID = x;
                break;
            }
        }

        return spriteID;
    }

    public static byte[] TileTypeToTilyByte(byte type) {
        byte[] returnValue = { 0 };

        switch (type) {
            case 0: // LAWN
                returnValue = Utility.LAWN_BYTE();
                break;
            case 1: // EARTH
                returnValue = Utility.EARTH_BYTE();
                break;
            case 2: // WATER
                returnValue = Utility.WATER_BYTE();
                break;
            case 10: // TRANSPARENT
                returnValue = Utility.TRANSPARENT_BYTE();
                break;
        }

        return returnValue;
    }

    public static void Victory() {
        //QuitGame();
    }

    public static void GameOver() {
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
