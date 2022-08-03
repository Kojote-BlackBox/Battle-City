using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderAsphalt : MonoBehaviour {

    private MapBuilder mapBuilderScript;

    public BuilderAsphalt(MapBuilder origin) {
        this.mapBuilderScript = origin;
    }
}
