using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chassis : MonoBehaviour {
    public Animator animator;
    private AnimationNode anim;
    private float animationTimer;

    private Map map;
    private float enviromentSpeedDebuf;

    public float forwardSpeed;
    public float backwardSpeed;
    public float chassisTurnTime;

    // needed for movement Animation.
    public Vector2 inputDirection;
    public Vector2 driveDirection;

    private new Rigidbody2D rigidbody;

    // Helper Values for UpdateDriveDirection
    private float inputSum = 0.0f;
    private float chassieRotatationDegree = 0.0f;

    // For Synchronisation of animation and tank rotation every x degree.
    private float stepRotation = 22.5f;

    const string T1_CHASSIS_0 = "T1_Chassis_0";
    const string T1_CHASSIS_22 = "T1_Chassis_22";
    const string T1_CHASSIS_45 = "T1_Chassis_45";
    const string T1_CHASSIS_67 = "T1_Chassis_67";
    const string T1_CHASSIS_90 = "T1_Chassis_90";
    const string T1_CHASSIS_112 = "T1_Chassis_112";
    const string T1_CHASSIS_135 = "T1_Chassis_135";
    const string T1_CHASSIS_157 = "T1_Chassis_157";
    const string T1_CHASSIS_180 = "T1_Chassis_180";
    const string T1_CHASSIS_202 = "T1_Chassis_202";
    const string T1_CHASSIS_225 = "T1_Chassis_225";
    const string T1_CHASSIS_247 = "T1_Chassis_247";
    const string T1_CHASSIS_270 = "T1_Chassis_270";
    const string T1_CHASSIS_292 = "T1_Chassis_292";
    const string T1_CHASSIS_315 = "T1_Chassis_315";
    const string T1_CHASSIS_337 = "T1_Chassis_337";

    void Start() {
        enviromentSpeedDebuf = 1.0f;
        map = GameObject.Find("Map").GetComponent<Map>();
        chassisTurnTime = 0.2f;
        driveDirection = Vector2.up;
        rigidbody = this.GetComponent<Rigidbody2D>();
        forwardSpeed = 1.8f;
        backwardSpeed = forwardSpeed / 2;

        anim = new AnimationNode("T1_Chassis_0", 0f);
        anim.AddNode("T1_Chassis_22", 22.5f);
        anim.AddNode("T1_Chassis_45", 45f);
        anim.AddNode("T1_Chassis_67", 67.5f);
        anim.AddNode("T1_Chassis_90", 90f);
        anim.AddNode("T1_Chassis_112", 112.5f);
        anim.AddNode("T1_Chassis_135", 135f);
        anim.AddNode("T1_Chassis_157", 157.5f);
        anim.AddNode("T1_Chassis_180", 180f);
        anim.AddNode("T1_Chassis_202", 202.5f);
        anim.AddNode("T1_Chassis_225", 225f);
        anim.AddNode("T1_Chassis_247", 247.5f);
        anim.AddNode("T1_Chassis_270", 270f);
        anim.AddNode("T1_Chassis_292", 292.5f);
        anim.AddNode("T1_Chassis_315", 315f);
        anim.AddNode("T1_Chassis_337", 337.5f);
        anim.CloseToLoop();
        //anim.DebugLog();
    }

    void Update() {
        animationTimer += Time.deltaTime;
    }

    public void Input(Vector2 directionInput) {
        // reduce the initial value growth time (faster first rotation)
        directionInput.x *= 1000;
        if (directionInput.x > 1.0f) {
            directionInput.x = 1.0f;
        } else if (directionInput.x < -1.0f) {
            directionInput.x = -1.0f;
        }
        inputDirection = directionInput;

        // invert drive backward direction 
        if (inputDirection.y < 0) {
            inputDirection.x *= -1;
        }

        UpdateDriveDirection();
        UpdateChassisGUI(anim.DirectionToDegree(driveDirection));
        UpdateEnviroment();
    }

    private void UpdateDriveDirection() {
        inputSum += inputDirection.x * (Time.deltaTime * 90.0f) * (1.0f - chassisTurnTime);
        if (inputSum > stepRotation) {
            inputSum = 0.0f;
            chassieRotatationDegree += stepRotation;
        } else if (inputSum < -stepRotation) {
            inputSum = 0.0f;
            chassieRotatationDegree -= stepRotation;
            if (chassieRotatationDegree < 0.0f) {
                chassieRotatationDegree += 360.0f;
            }
        }
        chassieRotatationDegree %= 360.0f;

        driveDirection = Quaternion.AngleAxis(chassieRotatationDegree, -Vector3.forward) * Vector2.up;
    }

    void FixedUpdate() {
        if (inputDirection.y < 0) {
            rigidbody.MovePosition((Vector2)transform.position + (driveDirection * inputDirection.y * backwardSpeed * this.enviromentSpeedDebuf * Time.fixedDeltaTime));

        } else {
            rigidbody.MovePosition((Vector2)transform.position + (driveDirection * inputDirection.y * forwardSpeed * this.enviromentSpeedDebuf * Time.fixedDeltaTime));
        }
    }

    private void PlayAnimation(string animationName) {
        if (animationTimer > chassisTurnTime) {
            animationTimer = 0.0f;
            animator.Play(anim.Play(animationName));
        }
    }

    private void UpdateChassisGUI(float degree) {
        if (degree >= 348.75f && degree < 360f || degree >= 0 && degree < 11.25f) {
            PlayAnimation(T1_CHASSIS_0);

        } else if (degree >= 11.25f && degree < 33.75f) {
            PlayAnimation(T1_CHASSIS_22);

        } else if (degree >= 33.75f && degree < 56.25f) {
            PlayAnimation(T1_CHASSIS_45);

        } else if (degree >= 56.25f && degree < 78.75f) {
            PlayAnimation(T1_CHASSIS_67);

        } else if (degree >= 78.75f && degree < 101.25f) {
            PlayAnimation(T1_CHASSIS_90);

        } else if (degree >= 101.25f && degree < 123.75f) {
            PlayAnimation(T1_CHASSIS_112);

        } else if (degree >= 123.75f && degree < 146.25f) {
            PlayAnimation(T1_CHASSIS_135);

        } else if (degree >= 146.25f && degree < 168.75f) {
            PlayAnimation(T1_CHASSIS_157);

        } else if (degree >= 168.75f && degree < 191.25f) {
            PlayAnimation(T1_CHASSIS_180);

        } else if (degree >= 191.25f && degree < 213.75f) {
            PlayAnimation(T1_CHASSIS_202);

        } else if (degree >= 213.75f && degree < 236.25f) {
            PlayAnimation(T1_CHASSIS_225);

        } else if (degree >= 236.25f && degree < 258.75f) {
            PlayAnimation(T1_CHASSIS_247);

        } else if (degree >= 258.75f && degree < 281.25f) {
            PlayAnimation(T1_CHASSIS_270);

        } else if (degree >= 281.25f && degree < 303.75f) {
            PlayAnimation(T1_CHASSIS_292);

        } else if (degree >= 303.75f && degree < 326.25f) {
            PlayAnimation(T1_CHASSIS_315);

        } else if (degree >= 326.25f && degree < 348.75f) {
            PlayAnimation(T1_CHASSIS_337);
        }
    }

    void UpdateEnviroment() {
        Vector3 position = GetComponent<Renderer>().bounds.center;
        Vector2 tankPosition = new Vector2(position.x, position.y);

        GameObject tile = map.GetTileOnPosition(tankPosition);
        Vector2 mapPosition = tile.GetComponent<Tile>().position;

        this.enviromentSpeedDebuf = tile.GetComponent<Tile>().slowDown;
    }
}
