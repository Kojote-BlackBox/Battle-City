using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public Tile tileScript;
    public Animator animator;
    public string currentWaterTile;

    void Start() {
        tileScript = this.transform.gameObject.GetComponent<Tile>();
        TileData tileData = Utility.WATER_TILE;

        if (string.IsNullOrEmpty(currentWaterTile)) {
            tileScript.byteMap = tileData.ByteMap;
            tileScript.isPassable = tileData.IsPassable;
        }
    }

    // Start the Water Animation
    void OnBecameVisible() {
        if (!string.IsNullOrEmpty(currentWaterTile)) {
            currentWaterTile = "Water_";

            // Erstellen Sie den Namen für die Animation basierend auf den Eigenschaften von ByteMapping
            ByteMapping byteMap = tileScript.byteMap;
            currentWaterTile += byteMap.topLeft ? "1" : "0";
            currentWaterTile += byteMap.topRight ? "1" : "0";
            currentWaterTile += byteMap.bottomLeft ? "1" : "0";
            currentWaterTile += byteMap.bottomRight ? "1" : "0";

            animator.Play(currentWaterTile);
        } 
    }

    // Just used for transitions.
    public void SetByteMap(ByteMapping byteMap) {
        tileScript.byteMap = byteMap;
        currentWaterTile = "Water_";

        // Erstellen Sie den Namen für die Animation basierend auf den Eigenschaften von ByteMapping
        currentWaterTile += byteMap.topLeft ? "1" : "0";
        currentWaterTile += byteMap.topRight ? "1" : "0";
        currentWaterTile += byteMap.bottomLeft ? "1" : "0";
        currentWaterTile += byteMap.bottomRight ? "1" : "0";

        if (!(currentWaterTile.Equals("Water_1111"))) {
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
            tileScript.isPassable = true; 
        }

        animator = GetComponent<Animator>();
        animator.Play(currentWaterTile);
    }

    // TODO Animation
    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(collision.gameObject);
    }
}
