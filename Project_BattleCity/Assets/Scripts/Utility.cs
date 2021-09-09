using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    public const byte LAWN = 0;
    public const byte EARTH = 1;
    public const byte WATER = 2;
    public const byte TRANSPARENT = 10;

    public static byte[] LAWN_BYTE = new byte[] { LAWN, LAWN, LAWN, LAWN };
    public static byte[] EARTH_BYTE = new byte[] { EARTH, EARTH, EARTH, EARTH };
    public static byte[] WATER_BYTE = new byte[] { WATER, WATER, WATER, WATER };
    public static byte[] TRANSPARENT_BYTE = new byte[] { TRANSPARENT, TRANSPARENT, TRANSPARENT, TRANSPARENT };

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
    public enum TileType { Ground, SoftGround, Mud, Desert, Asphalt, Water, Ice, FragileIce, Swamp };
    public enum Side { Up, Down, Left, Right };
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
