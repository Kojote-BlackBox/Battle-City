using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChassie", menuName = "Gameplay/Tank/Chassie")]
public class SOChassie : ScriptableObject {
    public int health;
    public int armor;
    public float forwardSpeed;
    public float backwardSpeedPercentage;
    public float rotationSpeed;
    public AnimatorController animationController;
}