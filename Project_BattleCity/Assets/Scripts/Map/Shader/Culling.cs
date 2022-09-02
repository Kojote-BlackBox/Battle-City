#pragma warning disable 0618 // is deprecated but it the only way it works

using UnityEngine;

public class Culling : MonoBehaviour {

    public GameObject tile;

    void Awake() {
        GetComponent<SpriteRenderer>().bounds.SetMinMax(Vector3.zero, Vector3.one);
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
