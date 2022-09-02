using System.Collections;
using UnityEngine;

public class Turret : Rotation {
    private Chassis chassisScript;
    private Vector2 chassisDirection;
    private Animator animator;
    private AnimationNode anim;
    private float animationTimer;

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
        anim = new AnimationNode(T1_TURRET_0, 0f);
        anim.AddNode(T1_TURRET_22, 22.5f);
        anim.AddNode(T1_TURRET_45, 45f);
        anim.AddNode(T1_TURRET_67, 67.5f);
        anim.AddNode(T1_TURRET_90, 90f);
        anim.AddNode(T1_TURRET_112, 112.5f);
        anim.AddNode(T1_TURRET_135, 135f);
        anim.AddNode(T1_TURRET_157, 157.5f);
        anim.AddNode(T1_TURRET_180, 180f);
        anim.AddNode(T1_TURRET_202, 202.5f);
        anim.AddNode(T1_TURRET_225, 225f);
        anim.AddNode(T1_TURRET_247, 247.5f);
        anim.AddNode(T1_TURRET_270, 270f);
        anim.AddNode(T1_TURRET_292, 292.5f);
        anim.AddNode(T1_TURRET_315, 315f);
        anim.AddNode(T1_TURRET_337, 337.5f);
        anim.CloseToLoop();

        animator = GetComponent<Animator>();
        animationTimer = 0.0f;
        chassisScript = transform.parent.gameObject.GetComponent<Chassis>();
    }

    void Start() {
        // waiting for chassis direction change and ynchronizat turret rotation to it.
        //StartCoroutine(SynchronizatTurretToChassis());
    }

    IEnumerator SynchronizatTurretToChassis() {
        yield return new WaitUntil(() => chassisDirection != chassisScript.GetCurrentDirection());

        var chassieInputDirection = chassisScript.GetCurrentInputDirection();

        //Set Tower direction one Step in the site 
        if (chassieInputDirection.x > 0) {
            anim.SetNextAnimation();

        } else if (chassieInputDirection.x < 0) {
            anim.SetPreviousAnimation();
        }

        chassisDirection = chassieInputDirection;

        StartCoroutine(SynchronizatTurretToChassis());
    }

    void Update() {
        animationTimer += Time.deltaTime;
    }

    override public void Rotate(float directionInput, float rotationModifier) {
        base.Rotate(directionInput, rotationModifier);
        UpdateTurretGUI(anim.DirectionToDegree(currentDirection));
    }

    private void PlayAnimation(string animationName) {
        if (animationTimer > rotationTime) {
            animationTimer = 0.0f;
            animator.Play(anim.Play(animationName));
        }
    }

    private void UpdateTurretGUI(float degree) {
        if (Mathf.Abs(anim.DirectionToDegree(anim.GetCurrentAnimationDirection()) - degree) < 22.5f) {
            currentDirection = anim.DegreeToDirection(degree);
        } else {
            currentDirection = anim.GetCurrentAnimationDirection();
        }
            

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
