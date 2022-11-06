using System.Collections;
using UnityEngine;

public class Turret : Rotation {
    private const string KEY_DIRECTIONX = "directionX";
    private const string KEY_DIRECTIONY = "directionY";

    [Header("Base Values")]
    [Space(10)]
    [SerializeField]
    private SOTurret baseValues;
    [SerializeField]
    private float reloadTime;
    [SerializeField]
    private float projectileSpeed;

    private Animator animator;

    override protected void Awake() {
        base.Awake();

        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = baseValues.animationController;
        UpdateAnimationParameters();

        rotationTime = baseValues.rotationSpeed;
        reloadTime = baseValues.reloadTime;
        projectileSpeed = baseValues.projectileSpeed;
    
    }

    private void UpdateAnimationParameters() {
        animator.SetFloat(KEY_DIRECTIONX, currentDirection.x);
        animator.SetFloat(KEY_DIRECTIONY, currentDirection.y);
    }

    override public void Rotate(float directionInput, float rotationModifier) {
        base.Rotate(directionInput, rotationModifier);
        UpdateAnimationParameters();
    }
}
