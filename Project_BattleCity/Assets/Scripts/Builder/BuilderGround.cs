using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class BuilderGround {

    private MapBuilder mapBuilderScript;
    private Map mapScript;

    public BuilderGround(MapBuilder origin) {
        this.mapBuilderScript = origin;
        this.mapScript = mapBuilderScript.mapScript;
    }

    // Main Funktion of the the Class, include the Main Logic to build Gras.
    public void Generate(TileData tileData, float coverage, float coherence) {
        if (tileData == null) {
            Debug.LogError("TileData ist null.");
            return;
        }

        List<Vector2> allPositions = mapBuilderScript.GenerateRandomPositionMap();
        List<GameObject> generatedTiles = new List<GameObject>();

        int totalTiles = mapScript.cols * mapScript.rows;
        int toCover = (int)(totalTiles * coverage);

        GameObject tilePattern = mapBuilderScript.InstantiateToTilePattern(tileData);

        foreach (var position in allPositions.ToList()) {
            if (generatedTiles.Count >= toCover) break;

            int x = (int)position.x;
            int y = (int)position.y;
            Vector2 pos = new Vector2(x, y);
            int layer = (int)tileData.LayerType;

            GameObject checkTile = mapScript.map[x, y, layer];
            
            if (checkTile == null) {
                GameObject tile = mapBuilderScript.tiling(tilePattern, pos);
                mapScript.map[x, y, layer] = tile;

                generatedTiles.Add(mapScript.map[x, y, layer]);
                allPositions.Remove(position);

                List<Vector2> neighborCoordinates = mapBuilderScript.GetAllNeighborCoordinates(x, y);

                foreach (Vector2 neighborPosition in neighborCoordinates) {
                    if (Random.Range(0.0f, 1.0f) < coherence) {

                        x = (int)neighborPosition.x;
                        y = (int)neighborPosition.y;
                        pos = new Vector2(x, y);
                        
                        GameObject field = mapScript.map[x, y, layer];

                        if (field == null) {
                            if (allPositions.Contains(neighborPosition)) {
                                tile = mapBuilderScript.tiling(tilePattern, pos);
                                mapScript.map[x, y, layer] = tile;

                                generatedTiles.Add(mapScript.map[x, y, layer]);
                                allPositions.Remove(neighborPosition);
                            }
                        }
                    }
                }
            }
        }
        
        GameObject.Destroy(tilePattern);
        mapBuilderScript.GenerateGroundTransition(tileData, generatedTiles);
    }
}
