using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderSwamp : MonoBehaviour {

    private MapBuilder mapBuilderScript;

    public BuilderSwamp(MapBuilder origin) {
        this.mapBuilderScript = origin;
    }
}
