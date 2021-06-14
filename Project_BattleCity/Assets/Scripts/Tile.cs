using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Utility.TileType type;
    public Vector2 position;
    public bool passable;
    public float slowDown = 1.0f;
}


