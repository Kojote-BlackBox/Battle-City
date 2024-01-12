using System;
using UnityEngine;

public static class GameEvents {
    // Delegat f�r Positions�nderungs-Events
    public delegate void PositionChangedEventHandler(object source, PositionChangedArgs args);

    // Event, das ausgel�st wird, wenn sich die Position eines Objekts �ndert
    public static event PositionChangedEventHandler PositionChanged;

    // Methode, um das Event auszul�sen
    public static void OnPositionChanged(object source, PositionChangedArgs args) {
        PositionChanged?.Invoke(source, args);
    }
}

// Klasse f�r die Event-Argumente
public class PositionChangedArgs : EventArgs {
    public Vector2 NewPosition { get; set; }
    public GameObject GameObject { get; set; }
}
