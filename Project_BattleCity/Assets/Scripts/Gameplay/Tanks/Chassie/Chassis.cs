using UnityEngine;

public class Chassis : Rotation {
    private const string KEY_DIRECTIONX = "directionX";
    private const string KEY_DIRECTIONY = "directionY";

    [Header("Base Values")]
    [Space(10)]
    [SerializeField]
    private SOChassie baseValues;
    [SerializeField]
    private int health;
    [SerializeField]
    private int armor;
    [SerializeField]
    private float forwardSpeed;

    private Animator animator;
    private Map map;
    private float enviromentSpeedDebuff;
    private new Rigidbody2D rigidbody;
    private float backwardSpeed;

    private Vector2 lastPosition;

    private float movementThreshold; // Setzen Sie den Schwellenwert

    void Update() {
        Vector2 currentPosition = transform.position;
        float distanceMoved = Vector2.Distance(currentPosition, lastPosition);


        if (distanceMoved > movementThreshold) {
            // Erstellen der Event-Argumente
            PositionChangedArgs args = new PositionChangedArgs {
                NewPosition = currentPosition,
                GameObject = this.gameObject
            };

            // Auslösen des Events
            GameEvents.OnPositionChanged(this, args);

            // Aktualisieren der letzten Position
            lastPosition = currentPosition;
        }
    }

    override protected void Awake() {
        base.Awake();

        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = baseValues.animationController;
        UpdateAnimationParameters();

        rigidbody = GetComponent<Rigidbody2D>();
        backwardSpeed = baseValues.forwardSpeed * baseValues.backwardSpeedPercentage;

        //TODO Dynamic
        enviromentSpeedDebuff = 1.0f;

        health = baseValues.health;
        armor = baseValues.armor;
        forwardSpeed = baseValues.forwardSpeed;
        backwardSpeed = forwardSpeed * baseValues.backwardSpeedPercentage;
        this.rotationTime = baseValues.rotationSpeed;
    }

    override protected void Start() {
        map = GameObject.Find("Map").GetComponent<Map>();
        lastPosition = transform.position;
        movementThreshold = 0.25f;
    }

    private void UpdateAnimationParameters() {
        animator.SetFloat(KEY_DIRECTIONX, currentDirection.x);
        animator.SetFloat(KEY_DIRECTIONY, currentDirection.y);
    }

    override public void Rotate(float directionInput, float rotationModifier) {
        base.Rotate(directionInput, rotationModifier);
        UpdateAnimationParameters();
    }

    public void Move(float directionInput) {
        inputDirection.y = directionInput;
    }

    private void FixedUpdate() {
        if (inputDirection.y == 0f) return;

        // TODO link in sOChassie.rotationSpeed
        var forwardMovement = currentDirection * enviromentSpeedDebuff * Time.fixedDeltaTime;
        var movement = inputDirection.y < 0f ? -1 * forwardMovement * backwardSpeed : forwardMovement * forwardSpeed;

        rigidbody.MovePosition((Vector2)transform.position + movement);
    }
    void UpdateEnviroment() {
        Vector3 position = GetComponent<Renderer>().bounds.center;
        Vector2 tankPosition = new Vector2(position.x, position.y);

        GameObject tile = map.GetTileOnPosition(tankPosition);
        Vector2 mapPosition = tile.GetComponent<Tile>().position;

        this.enviromentSpeedDebuff = tile.GetComponent<Tile>().slowDownFactor;
    }
}
