using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSetup : MonoBehaviour {

    private float cloudSizeScale;

    private Map map;
    private Material mat;

    void Start() {

        cloudSizeScale = 0.7f;
        map = GameObject.Find("Map").GetComponent<Map>();
        mat = GetComponent<Renderer>().material;

        transform.localScale = new Vector3(map.rows, map.cols, 1);
        transform.position = new Vector3(map.rows / 2, map.cols / 2, 0);

        mat.SetFloat("_CloudSize", (map.cols + map.rows) * cloudSizeScale);
        mat.SetVector("_CloudSpeed", new Vector2(0.005f, 0.003f));
    }

    // todo value changer
    //public 
}
