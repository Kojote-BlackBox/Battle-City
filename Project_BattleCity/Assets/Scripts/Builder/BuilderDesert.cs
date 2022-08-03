using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderDesert : MonoBehaviour {

    private MapBuilder mapBuilderScript;

    public BuilderDesert(MapBuilder origin) {
        this.mapBuilderScript = origin;
    }
}
