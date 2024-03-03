using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Repräsentiert das Byte-Mapping für die Transparenzen eines Tiles.
[System.Serializable]
public class ByteMapping
{
    public bool topLeft;
    public bool topRight;
    public bool bottomLeft;
    public bool bottomRight;
}

public enum TileType
{
    Ground,
    Asphalt,
    Water,
    Ice,
    FragileIce,
    Snow,
    PavingStone,
    Object,
    Gravel,
    Desert,
    // Not Used
    SoftGround,
    Mud,
    Swamp
};

public enum LayerType
{
    Base = 0,         // Grund-Layer (Persistent)
    GroundOverlay,    // Grund-Overlayer (Gras, Asphalt, etc.)
    StateOverlay,     // Overlayer f�r Grundzust�nde (Risse, Pf�tzen)
    WeatherOverlay,   // Overlayer f�r Niederschlag (Regen, Schnee)
    ObjectOverlay     // Objekt Layer (Kleinere Varianzen)
}

// Repräsentiert die Soundeffekte, die mit einem Tile verbunden sind.
[System.Serializable]
public class SoundEffects
{
    public string onWalk;
    public string onDrive;
    public string onShoot;
    public string onSink;
    public string onFall;
    public string onCreate;
}

// Repräsentiert das Mapping eines einzelnen Tiles, einschließlich seines Namens, Byte-Mappings und anderer Attribute.
[System.Serializable]
public class TileMapping
{
    public LayerType layerType;
    public string spriteName;
    public ByteMapping byteMapping;
    public TileType tileType;
    public bool isPassable;
    public float slowDownFactor;
    public SoundEffects soundEffects;
}

// Eine Liste, die alle TileMappings enthält.
[System.Serializable]
public class TileMappingList
{
    public TileMapping[] Base;
    public TileMapping[] GroundOverlay;
    public TileMapping[] StateOverlay;
    public TileMapping[] WeatherOverlay;
    public TileMapping[] ObjectOverlay;


    // Zuweisung des layerType für jedes TileMapping in der TileMappingList
    public void AssignLayerType()
    {
        foreach (var tileMapping in Base)
        {
            tileMapping.layerType = LayerType.Base;
        }
        foreach (var tileMapping in GroundOverlay)
        {
            tileMapping.layerType = LayerType.GroundOverlay;
        }
        foreach (var tileMapping in StateOverlay)
        {
            tileMapping.layerType = LayerType.StateOverlay;
        }
        foreach (var tileMapping in WeatherOverlay)
        {
            tileMapping.layerType = LayerType.WeatherOverlay;
        }
        foreach (var tileMapping in ObjectOverlay)
        {
            tileMapping.layerType = LayerType.ObjectOverlay;
        }
    }

    public IEnumerable<TileMapping> GetAllTileMappings()
    {
        AssignLayerType();
        return Base.Concat(GroundOverlay).Concat(StateOverlay).Concat(WeatherOverlay).Concat(ObjectOverlay);
    }
}

// Eine Klasse, die alle relevanten Informationen eines Tiles enthält.
public class TileData
{
    public LayerType LayerType { get; set; }
    public Sprite Sprite { get; set; }
    public ByteMapping ByteMap { get; set; }
    public TileType TileType { get; set; }
    public bool IsPassable { get; set; }
    public float SlowDownFactor { get; set; }
    public SoundEffects SoundEffects { get; set; }

    public TileData(LayerType layerType, Sprite sprite, ByteMapping byteMap, TileType tileType, bool isPassable, float slowDownFactor, SoundEffects soundEffects)
    {
        LayerType = layerType;
        Sprite = sprite;
        ByteMap = byteMap;
        TileType = tileType;
        IsPassable = isPassable;
        SlowDownFactor = slowDownFactor;
        SoundEffects = soundEffects;
    }
}

// Die MapAtlas-Klasse ist verantwortlich für das Laden und Verwalten der Tile-Daten aus einer Atlas-Ressource.
// Sie bietet eine zentrale Schnittstelle, um Zugriff auf Tile-Informationen und Sprites basierend auf dem Tile-Namen zu erhalten.
public class MapAtlas
{

    private static MapAtlas instance;
    private Sprite[] atlas;
    private TileMappingList tileMappingList;
    private Dictionary<string, TileMapping> tileDataMapping = new Dictionary<string, TileMapping>();

    // Singleton-Instanz (Lazy Initialization)
    public static MapAtlas Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MapAtlas();
            }
            return instance;
        }
    }

    // Konstruktor: Lädt alle Tiles aus dem Atlas und die TileMappings aus einer JSON-Datei.
    private MapAtlas()
    {
        atlas = Resources.LoadAll<Sprite>("TileMap/atlas");
        TextAsset jsonText = Resources.Load<TextAsset>("TileMap/AtlasMapping");
        tileMappingList = JsonUtility.FromJson<TileMappingList>(jsonText.text);

        // Speichert jedes TileMapping im Dictionary für den schnellen Zugriff.
        foreach (var tileMapping in tileMappingList.GetAllTileMappings())
        {
            tileDataMapping[tileMapping.spriteName] = tileMapping;
        }
    }

    // Gibt ein TileData-Objekt für ein gegebenes Tile zurück, basierend auf dessen Namen.
    public TileData GetTileDataByName(string tileName)
    {
        if (tileDataMapping.TryGetValue(tileName, out TileMapping tileMapping))
        {
            var sprite = GetSpriteByName(tileMapping.spriteName);

            TileData tileData = new TileData(
                tileMapping.layerType,
                sprite,
                tileMapping.byteMapping,
                tileMapping.tileType,
                tileMapping.isPassable,
                tileMapping.slowDownFactor,
                tileMapping.soundEffects
            );

            return tileData;
        }

        // Rückgabe eines Standardwerts oder Fehlers, falls kein Mapping gefunden wird
        return null; // Oder werfen Sie eine Ausnahme
    }

    // Hilfsmethode, um ein Sprite anhand seines Namens zu erhalten.
    internal Sprite GetSpriteByName(string spriteName)
    {
        return atlas.FirstOrDefault(sprite => sprite.name == spriteName);
    }
}
