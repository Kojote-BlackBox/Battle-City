using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewCampData", menuName = "ScriptableObjects/CampData", order = 1)]
public class CampData : ScriptableObject {
    [SerializeField] private bool isFriendly;
    [SerializeField] public Sprite campSprite;
    [SerializeField] public Color friendlyColor = Color.green;
    [SerializeField] public Color enemyColor = Color.red;

    public float maxCapTime;
    // Das Farbsprite, das zur Laufzeit eingef�rbt wird
    [HideInInspector] public SpriteRenderer colorSpriteRenderer;

    [HideInInspector] public bool caped;
    [HideInInspector] public float globalCapedTime;
    [HideInInspector] public int tankCount;

    // Event f�r isFriendly-�nderungen
    public UnityEvent OnCampCaptured = new UnityEvent();

    public bool IsFriendly {
        get { return isFriendly; }
        set {
            isFriendly = value;

            // Farbe des colorSpriteRenderer entsprechend anpassen
            if (colorSpriteRenderer != null) {
                colorSpriteRenderer.color = isFriendly ? friendlyColor : enemyColor;
            }

            // Bei �nderung von isFriendly das Event ausl�sen
            OnCampCaptured.Invoke();
        }
    }
}
