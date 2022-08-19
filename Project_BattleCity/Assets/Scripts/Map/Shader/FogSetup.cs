using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogSetup : MonoBehaviour {

    private float fogSizeScale;

    private Map map;
    private Material mat;

    void Start() {

        fogSizeScale = 0.7f;
        map = GameObject.Find("Map").GetComponent<Map>();
        mat = GetComponent<Renderer>().material;

        transform.localScale = new Vector3(map.rows, map.cols, 1);
        transform.position = new Vector3(map.rows / 2, map.cols / 2, 0);

        mat.SetFloat("_FogSize", (map.cols + map.rows) * fogSizeScale);        
    }

    // todo value changer
    //public 
}
