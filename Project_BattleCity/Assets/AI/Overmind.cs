using UnityEngine;
public class Overmind : MonoBehaviour {
    public Map mapScript;

    public Overmind(GameObject origin) {
        this.mapScript = origin.GetComponent<Map>();
    }

    void OnEnable() {
        GameEvents.PositionChanged += OnTankPositionChanged;
    }

    void OnDisable() {
        GameEvents.PositionChanged -= OnTankPositionChanged;
    }

    private void OnTankPositionChanged(object source, PositionChangedArgs args) {
        //Debug.Log("Tank moved to: " + args.NewPosition);
        // Implementieren Sie hier Ihre Logik basierend auf der neuen Position
    }
}
