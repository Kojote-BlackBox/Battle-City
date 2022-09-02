using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private Vector2 chassieDirection = Vector2.zero;
    private float turretDirection;

    [SerializeField]
    private Chassis chassisScript;
    [SerializeField]
    private Turret turretScript;
    [SerializeField]
    private Shooting shottingScript;

    private PlayerInput playerInput;

    private Map map; // TODO: rework map dingsdabumsda

    private void Awake() {
        playerInput = gameObject.GetComponent<PlayerInput>();
    }

    void Start() {
        map = GameObject.Find("Map").GetComponent<Map>();
        placePlayer();
    }

    private void OnEnable() {
        playerInput.enabled = true;
    }

    private void OnDisable() {
        playerInput.enabled = false;
    }

    // TODO: verschieben in game manager bzw. spawner
    private void placePlayer() {
        Vector2 playerPosition = new Vector2(map.map.GetLength(0) / 2, map.map.GetLength(1) / 2);

        while (map.map[(int)playerPosition.x, (int)playerPosition.y, 1] != null) {
            playerPosition = new Vector2(Random.Range(0, map.map.GetLength(0)), Random.Range(0, map.map.GetLength(1)));
        }
        chassisScript.gameObject.transform.position = playerPosition;
    }

    public void OnChassieMovement(InputAction.CallbackContext context) {
        chassieDirection = context.ReadValue<Vector2>().normalized;
    }

    public void OnTurretRotation(InputAction.CallbackContext context) {

        turretDirection = context.ReadValue<float>();
    }

    public void OnShoot(InputAction.CallbackContext context) {
        shottingScript.Shoot();
    }

    void FixedUpdate() {
        chassisScript.Rotate(chassieDirection.x, 0f);
        chassisScript.Move(chassieDirection.y);
        turretScript.Rotate(turretDirection, chassieDirection.x);
    }
}
