using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public Chassis chassisScript;
    public Shooting shottingScript;

    public Vector2 enemyPosition;
    public Vector2 targetPosition;

    private int health;
    private Map map;
    private float maxDriveTime = 2.0f; //sec
    private float visibility;

    private float maxMapSiceX;
    private float maxMapSiceY;
    private float distanceBorder;

    private float driveTime = 0.0f;
    private enum Direction { Up, Right, Down, Left };
    private Direction direction = Direction.Up;
    public GameObject turret;
    public LayerMask layerMask;

    void Start() {

        chassisScript = this.transform.gameObject.GetComponent<Chassis>();
        shottingScript = this.transform.gameObject.transform.GetChild(0).gameObject.GetComponent<Shooting>();
        map = GameObject.Find("Map").GetComponent<Map>();

        maxMapSiceX = (float)map.map.GetLength(0);
        maxMapSiceY = (float)map.map.GetLength(1);
        distanceBorder = 1.5f;
        visibility = 4.0f;

        health = 2;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            map.EnemyDies();
            Destroy(gameObject);
        }
    }

    void Update() {
        SimpleRandomDrive();
        ShootPlayer();
    }

    void FixedUpdate() {

    }

    public void ShootPlayer() {
        // TODO Buggy
        RaycastHit2D hit = Physics2D.Raycast(transform.position, turret.transform.up, visibility, layerMask);

        if (hit.collider != null && hit.collider.name.Equals("Player")) {
            shottingScript.Shoot();
        }
    }

    public void SimpleRandomDrive() {
        driveTime += Time.deltaTime;
        if (driveTime < Random.Range(0.5f, maxDriveTime)) {
            switch ((int)direction) {
                case 0: // W
                    if (transform.position.y < maxMapSiceX - distanceBorder) {
                        chassisScript.Move(0f);
                    }
                    break;
                case 1: // A
                    if (transform.position.x > distanceBorder) {
                        chassisScript.Rotate(0f, 0f);
                    }
                    break;
                case 2: // D
                    if (transform.position.x < maxMapSiceY - distanceBorder) {
                        chassisScript.Rotate(0f, 0f);
                    }
                    break;
                case 3: // S
                    if (transform.position.y > distanceBorder) {
                        chassisScript.Move(0f);
                    }
                    break;
                default:
                    break;
            }
        } else {
            direction = (Direction)((int)Random.Range(0, 4));
            driveTime = 0.0f;
        }
    }

    /*
    void UpdateEnviroment()
    {
        Vector3 position = GetComponent<Renderer>().bounds.center;
        Vector2 tankPosition = new Vector2(position.x, position.y);
        
        GameObject tile = map.GetTileOnPosition(tankPosition);
        Vector2 mapPosition = tile.GetComponent<Tile>().position;
    }
    */
}
