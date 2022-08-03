using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderIce : MonoBehaviour {

    private MapBuilder mapBuilderScript;

    public BuilderIce(MapBuilder origin) {
        this.mapBuilderScript = origin;
    }
}
