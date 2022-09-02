using UnityEngine;

public class Rotation : MonoBehaviour {
    [Header("Rotation Settings")]
    [Space(10)]
    [Tooltip("How fast the gameobject is rotating.")]
    public float rotationTime;
    [Tooltip("Whether the rotation is propagated to its children.")]
    public bool propagateRotation;

    protected Vector2 currentDirection;
    protected Vector2 inputDirection;
    protected float inputSum = 0.0f;
    protected float rotatationDegree = 0.0f;
    protected float stepRotation = 22.5f;
    protected Rotation[] childrenRotations;

    virtual protected void Awake() {
        inputDirection = Vector2.up;
        currentDirection = Vector2.up;

        childrenRotations = GetComponentsInChildren<Rotation>();
    }
    
    virtual protected void Start() {

    }

    virtual public void Rotate(float directionInput, float rotationModifier) {
        inputDirection.x = directionInput;

        UpdateDirection(rotationModifier);

        if (childrenRotations != null && propagateRotation) {
            foreach (var child in childrenRotations) {
                if (child == this) continue;
                child.Rotate(directionInput, rotationTime);
            }
        }
    }

    virtual protected void UpdateDirection(float rotationModifier) {
        inputSum += inputDirection.x * (Time.deltaTime * 90.0f) * (1.0f - rotationTime + rotationModifier);
        if (inputSum > stepRotation) {
            inputSum = 0.0f;
            rotatationDegree += stepRotation;
        } else if (inputSum < -stepRotation) {
            inputSum = 0.0f;
            rotatationDegree -= stepRotation;
            if (rotatationDegree < 0.0f) {
                rotatationDegree += 360.0f;
            }
        }
        rotatationDegree %= 360.0f;

        currentDirection = Quaternion.AngleAxis(rotatationDegree, -Vector3.forward) * Vector2.up;
    }

    public Vector2 GetCurrentDirection() {
        return currentDirection;
    }

    public Vector2 GetCurrentInputDirection() {
        return inputDirection;
    }

    public float GetRotationTime() {
        return rotationTime;
    }
}

