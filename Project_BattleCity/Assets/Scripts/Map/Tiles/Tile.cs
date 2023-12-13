using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public LayerType layerType;
    public Sprite sprite;
    public ByteMapping byteMap;
    public TileType tileType;
    public bool isPassable;
    public float slowDownFactor;
    public SoundEffects soundEffects;
    public Vector2 position;

    // Konstruktor oder Initialisierungsmethoden entsprechend anpassen
}