using System;
using UnityEngine;

public static class GameEvents {
    // Delegat für Positionsänderungs-Events
    public delegate void PositionChangedEventHandler(object source, PositionChangedArgs args);

    // Event, das ausgelöst wird, wenn sich die Position eines Objekts ändert
    public static event PositionChangedEventHandler PositionChanged;

    // Methode, um das Event auszulösen
    public static void OnPositionChanged(object source, PositionChangedArgs args) {
        PositionChanged?.Invoke(source, args);
    }
}

// Klasse für die Event-Argumente
public class PositionChangedArgs : EventArgs {
    public Vector2 NewPosition { get; set; }
    public GameObject GameObject { get; set; }
}
