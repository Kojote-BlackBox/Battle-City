using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    private Chassis chassisScript;
    private Sprite[] sprites;
    public float turretRotationTime;
    private SpriteRenderer spriteRenderer;

    public Vector2 chassisDirection;
    public Vector2 turretDirection;
    public Vector2 inputDirection;

    public Animator animator;
    private AnimationNode anim;
    private float animationTimer;

    // Helper Values for UpdateDriveDirection
    private float inputSum = 0.0f;
    private float turretRotatationDegree = 0.0f;

    // For Synchronisation of animation and tank rotation every x degree.
    private float stepRotation = 22.5f;

    const string T1_TURRET_0 = "T1_Turret_0";
    const string T1_TURRET_22 = "T1_Turret_22";
    const string T1_TURRET_45 = "T1_Turret_45";
    const string T1_TURRET_67 = "T1_Turret_67";
    const string T1_TURRET_90 = "T1_Turret_90";
    const string T1_TURRET_112 = "T1_Turret_112";
    const string T1_TURRET_135 = "T1_Turret_135";
    const string T1_TURRET_157 = "T1_Turret_157";
    const string T1_TURRET_180 = "T1_Turret_180";
    const string T1_TURRET_202 = "T1_Turret_202";
    const string T1_TURRET_225 = "T1_Turret_225";
    const string T1_TURRET_247 = "T1_Turret_247";
    const string T1_TURRET_270 = "T1_Turret_270";
    const string T1_TURRET_292 = "T1_Turret_292";
    const string T1_TURRET_315 = "T1_Turret_315";
    const string T1_TURRET_337 = "T1_Turret_337";

    private void Awake() {
        anim = new AnimationNode("T1_Turret_0", 0f);
        anim.AddNode("T1_Turret_22", 22.5f);
        anim.AddNode("T1_Turret_45", 45f);
        anim.AddNode("T1_Turret_67", 67.5f);
        anim.AddNode("T1_Turret_90", 90f);
        anim.AddNode("T1_Turret_112", 112.5f);
        anim.AddNode("T1_Turret_135", 135f);
        anim.AddNode("T1_Turret_157", 157.5f);
        anim.AddNode("T1_Turret_180", 180f);
        anim.AddNode("T1_Turret_202", 202.5f);
        anim.AddNode("T1_Turret_225", 225f);
        anim.AddNode("T1_Turret_247", 247.5f);
        anim.AddNode("T1_Turret_270", 270f);
        anim.AddNode("T1_Turret_292", 292.5f);
        anim.AddNode("T1_Turret_315", 315f);
        anim.AddNode("T1_Turret_337", 337.5f);
        anim.CloseToLoop();
    }

    void Start() {
        animationTimer = 0.0f;
        chassisScript = transform.parent.gameObject.GetComponent<Chassis>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Tanks/T1");
        turretRotationTime = 0.3f;
        animator = GetComponent<Animator>();
        inputDirection = Vector2.up;
        turretDirection = Vector2.up;

        // waiting for chassis direction change and ynchronizat turret rotation to it.
        StartCoroutine(SynchronizatTurretToChassis());
    }

    IEnumerator SynchronizatTurretToChassis() {
        yield return new WaitUntil(() => chassisDirection != chassisScript.driveDirection);

        //Set Tower direction one Step in the site 
        if (chassisScript.inputDirection.x > 0) {
            anim.SetNextAnimation();

        } else if (chassisScript.inputDirection.x < 0) {
            anim.SetPreviousAnimation();
        }

        chassisDirection = chassisScript.driveDirection;

        StartCoroutine(SynchronizatTurretToChassis());
    }

    void Update() {
        animationTimer += Time.deltaTime;
    }

    public void Input(float directionInput) {
        if (directionInput != 0f) { 
           int i = 0;  
        }

        inputDirection.x = directionInput;
        inputDirection.y = 0f;

        UpdateTurretDirection();
        UpdateTurretGUI(anim.DirectionToDegree(inputDirection));
    }

    private void UpdateTurretDirection() {
        inputSum += inputDirection.x * (Time.deltaTime * 90.0f) * (1.0f - turretRotationTime);
        if (inputSum > stepRotation) {
            inputSum = 0.0f;
            turretRotatationDegree += stepRotation;
        } else if (inputSum < -stepRotation) {
            inputSum = 0.0f;
            turretRotatationDegree -= stepRotation;
            if (turretRotatationDegree < 0.0f) {
                turretRotatationDegree += 360.0f;
            }
        }
        turretRotatationDegree %= 360.0f;

        inputDirection = Quaternion.AngleAxis(turretRotatationDegree, -Vector3.forward) * Vector2.up;
    }

    private void PlayAnimation(string animationName) {
        if (animationTimer > turretRotationTime) {
            animationTimer = 0.0f;
            animator.Play(anim.Play(animationName));
        }
    }

    private void UpdateTurretGUI(float degree) {

        /*if (Mathf.Abs(anim.DirectionToDegree(anim.GetCurrentAnimationDirection()) - degree) < 22.5f) {
            turretDirection = anim.DegreeToDirection(degree);
        } else {
            turretDirection = anim.GetCurrentAnimationDirection();
        }*/

        if (degree >= 348.75f && degree < 360f || degree >= 0 && degree < 11.25f) {
            PlayAnimation(T1_TURRET_0);

        } else if (degree >= 11.25f && degree < 33.75f) {
            PlayAnimation(T1_TURRET_22);

        } else if (degree >= 33.75f && degree < 56.25f) {
            PlayAnimation(T1_TURRET_45);

        } else if (degree >= 56.25f && degree < 78.75f) {
            PlayAnimation(T1_TURRET_67);

        } else if (degree >= 78.75f && degree < 101.25f) {
            PlayAnimation(T1_TURRET_90);

        } else if (degree >= 101.25f && degree < 123.75f) {
            PlayAnimation(T1_TURRET_112);

        } else if (degree >= 123.75f && degree < 146.25f) {
            PlayAnimation(T1_TURRET_135);

        } else if (degree >= 146.25f && degree < 168.75f) {
            PlayAnimation(T1_TURRET_157);

        } else if (degree >= 168.75f && degree < 191.25f) {
            PlayAnimation(T1_TURRET_180);

        } else if (degree >= 191.25f && degree < 213.75f) {
            PlayAnimation(T1_TURRET_202);

        } else if (degree >= 213.75f && degree < 236.25f) {
            PlayAnimation(T1_TURRET_225);

        } else if (degree >= 236.25f && degree < 258.75f) {
            PlayAnimation(T1_TURRET_247);

        } else if (degree >= 258.75f && degree < 281.25f) {
            PlayAnimation(T1_TURRET_270);

        } else if (degree >= 281.25f && degree < 303.75f) {
            PlayAnimation(T1_TURRET_292);

        } else if (degree >= 303.75f && degree < 326.25f) {
            PlayAnimation(T1_TURRET_315);

        } else if (degree >= 326.25f && degree < 348.75f) {
            PlayAnimation(T1_TURRET_337);
        }
    }
}
