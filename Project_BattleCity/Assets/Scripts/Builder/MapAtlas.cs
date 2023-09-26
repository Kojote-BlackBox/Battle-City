using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*  AGENDA
*   lawn = 0
*   earth = 1
*   water = 2
*   transparent = 10
*   
*   
*   
*                 Rain                           Rain
*      Ground      ->     SoftGround      Mud     ->    Swamp
*  Lawn            ->      Wet Lawn
*      Drive
*       ->  Earth  ->  ->  ->  ->  ->  Wet Earth
*       
*  
*   
*   Positionierung: "byteArray": [1, 2, 3, 4],
*   byteMap Code on Tile.cs
*   |-------|
*   | 1   2 |  
*   | 3   4 |
*   |-------|
*/

public class MapAtlas {

    private static MapAtlas instance;

    // Singleton-Instanz (Lazy Initialization)
    public static MapAtlas Instance {
        get {
            if (instance == null) {
                instance = new MapAtlas();
            }
            return instance;
        }
    }

    // Konstruktor auf private setzen, um die Erstellung von weiteren Instanzen zu verhindern
    //  "tileMappings": [ { "byteArray": [0, 0, 0, 0], "spriteName": "Lawn_Lawn_Lawn_Lawn" } ]
    private MapAtlas() {
        atlas = Resources.LoadAll<Sprite>("TileMap/GroundTileset");
        PopulateSpriteMapping();
    }

    private Sprite[] atlas;

    [System.Serializable]
    public class TileMapping {
        public byte[] byteArray;
        public string spriteName;
    }

    [System.Serializable]
    public class TileMappingList {
        public TileMapping[] tileMappings;
    }

    private TileMappingList tileMappingList;
    public Dictionary<string, string> spriteMapping = new Dictionary<string, string>();

    public TileMappingList LoadTileMappingsFromJSON() {
        TextAsset jsonText = Resources.Load<TextAsset>("TileMap/TileMappings");
        return JsonUtility.FromJson<TileMappingList>(jsonText.text);
    }

    public Sprite GetSpriteByID(int atlasID) {
        if (atlas != null) {
            return atlas[atlasID];
        }
        return null;
    }

    public Sprite GetSpriteByName(string spriteName) {
        foreach (Sprite sprite in atlas) {
            if (sprite.name == spriteName) {
                return sprite;
            }
        }
        return null;
    }

    public void PopulateSpriteMapping() {
        tileMappingList = LoadTileMappingsFromJSON();

        foreach (TileMapping mapping in tileMappingList.tileMappings) {
            string byteArrayAsString = System.BitConverter.ToString(mapping.byteArray).Replace("-", "");
            spriteMapping[byteArrayAsString] = mapping.spriteName;
        }
    }

    public byte[] GetByteArrayForTileType(string tileTypeName) {
        foreach (TileMapping mapping in tileMappingList.tileMappings) {
            if (mapping.spriteName.Contains(tileTypeName)) {
                return mapping.byteArray;
            }
        }
        return null;
    }

    public string GetSpriteNameByByteArray(byte[] byteArray) {
        string byteArrayAsString = System.BitConverter.ToString(byteArray).Replace("-", "");
        return spriteMapping[byteArrayAsString];
    }

    public string GetSpriteNameFromByteArray(byte[] byteArray) {
        foreach (TileMapping mapping in tileMappingList.tileMappings) {
            if (Enumerable.SequenceEqual(mapping.byteArray, byteArray)) {
                return mapping.spriteName;
            }
        }
        return null; // oder einen Standardwert zurückgeben, falls das Byte-Array nicht gefunden wird.
    }

}
