using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public Tile tileScript;
    public Animator animator;
    public string currentWaterTile;

    void Start() {
        tileScript = this.transform.gameObject.GetComponent<Tile>();
        
        if(string.IsNullOrEmpty(currentWaterTile)) {
            tileScript.byteMap = Utility.WATER_BYTE;
            tileScript.passable = false;
        }
    }

    void OnBecameVisible() {
        if (!string.IsNullOrEmpty(currentWaterTile)) {
            currentWaterTile = "Water_";

            // create name coding for animation (animations include the sprites)
            foreach (byte b in tileScript.byteMap) {
                if (b == Utility.WATER) {
                    currentWaterTile += 1;
                } else {
                    currentWaterTile += 0;
                }
            }

            animator.Play(currentWaterTile);
        } 
    }

    // Just used for transitions.
    public void SetByteMap(byte[] byteMap) {
        tileScript.byteMap = byteMap;
        currentWaterTile = "Water_";

        // create name coding for animation (animations include the sprites)
        foreach (byte b in byteMap) {
            if(b == Utility.WATER) {
                currentWaterTile += 1;
            } else {
                currentWaterTile += 0;
            }
        }

        if (!(currentWaterTile.Equals("Water_1111"))) {
            Destroy(this.gameObject.GetComponent<Rigidbody2D>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
            tileScript.passable = true;
        }

        animator = GetComponent<Animator>();
        animator.Play(currentWaterTile);
    }

    // TODO Animation
    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(collision.gameObject);
    }
}
