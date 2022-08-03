using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFragileIce : MonoBehaviour {

    private MapBuilder mapBuilderScript;

    public BuilderFragileIce(MapBuilder origin) {
        this.mapBuilderScript = origin;
    }
}
