using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderMud : MonoBehaviour {

    private MapBuilder mapBuilderScript;

    public BuilderMud(MapBuilder origin) {
        this.mapBuilderScript = origin;
    }
}
