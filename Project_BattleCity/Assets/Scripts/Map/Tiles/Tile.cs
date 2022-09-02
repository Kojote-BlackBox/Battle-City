using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  0 = Lawn, 1 = Earth, 2 = Watter 
/* byteMap Code on Tile.cs
 * --------
 * | 0  1 |
 * | 2  3 |
 * --------
 */

public class Tile : MonoBehaviour {
    public Utility.TileType type;
    public byte[] byteMap = new byte[4];
    public Vector2 position;
    public bool passable;
    public float slowDown = 1.0f;
}


