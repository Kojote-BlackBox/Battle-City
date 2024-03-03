using UnityEngine;
using World;

public class FogSetup : MonoBehaviour {

    private float fogSizeScale;

    private Map map;
    private Material mat;

    void Start() {

        fogSizeScale = 0.7f;
        map = GameObject.Find("Map").GetComponent<Map>();
        mat = GetComponent<Renderer>().material;

        transform.localScale = new Vector3(map.rows, map.columns, 1);
        transform.position = new Vector3(map.rows / 2, map.columns / 2, 0);

        mat.SetFloat("_FogSize", (map.columns + map.rows) * fogSizeScale);        
    }

    // todo value changer
    //public 
}
