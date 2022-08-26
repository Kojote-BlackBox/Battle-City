using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Vector2 movement;
    public Chassis chassisScript;
    public Turret turretScript;
    public Shoot shottingScript;
    private Map map;
    private int health;

    private Vector2 turretDirection = new Vector2(0.0f, 0.0f);

    void Start() {
        movement = Vector2.up;
        map = GameObject.Find("Map").GetComponent<Map>();
        chassisScript = this.transform.gameObject.GetComponent<Chassis>();
        turretScript = this.transform.gameObject.transform.GetChild(0).gameObject.GetComponent<Turret>();
        shottingScript = this.transform.gameObject.transform.GetChild(0).gameObject.GetComponent<Shoot>();
        health = 2;

        placePlayer();
    }

    private void placePlayer() {
        Vector2 playerPosition = new Vector2(map.map.GetLength(0) / 2, map.map.GetLength(1) / 2);

        while (map.map[(int)playerPosition.x, (int)playerPosition.y, 1] != null) {
            playerPosition = new Vector2(Random.Range(0, map.map.GetLength(0)), Random.Range(0, map.map.GetLength(1)));
        }

        transform.position = playerPosition;
    }


    // Update is called once per frame
    void Update() {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        chassisScript.Input(movement);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = UnityEngine.Camera.main.ScreenToWorldPoint(mousePos);

        turretDirection.x = mousePos.x - transform.position.x;
        turretDirection.y = mousePos.y - transform.position.y;
        turretDirection.Normalize();
        turretScript.Input(turretDirection);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) {
            shottingScript.Input();
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;

        if (health <= 0) {
            Destroy(gameObject);
            Utility.GameOver();
        }
    }
}
