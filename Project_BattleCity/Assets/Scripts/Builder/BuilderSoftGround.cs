using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderSoftGround : MonoBehaviour {
    private MapBuilder mapBuilderScript;

    public BuilderSoftGround(MapBuilder origin) {
        this.mapBuilderScript = origin;
    }
}
