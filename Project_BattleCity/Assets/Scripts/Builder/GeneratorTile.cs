using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTile {

    private MapBuilder mapBuilderScript;
    private Map mapScript;

    public GeneratorTile(MapBuilder origin) {
        this.mapBuilderScript = origin;
        this.mapScript = mapBuilderScript.mapScript;
    }
}
