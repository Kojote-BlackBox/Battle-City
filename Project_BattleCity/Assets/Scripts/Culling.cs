using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Culling : MonoBehaviour {

    public GameObject tile;

    void Awake() {
        this.GetComponent<SpriteRenderer>().bounds.SetMinMax(Vector3.zero, Vector3.one);
        //this.GetComponent<SpriteRenderer>().bounds.Expand(1f);
    }

    void Start() {
        foreach (Transform child in transform) {
            child.gameObject.active = false;
        }
    }

    void OnBecameInvisible() {
        foreach (Transform child in transform) {
            child.gameObject.active = false;
        }
     }

    void OnBecameVisible() {
        foreach (Transform child in transform) {
            child.gameObject.active = true;
        }
    }
}
