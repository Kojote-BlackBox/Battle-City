using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 movement;
    public Chassis chassisScript;
    public Turret turretScript;
    public Shotting shottingScript;
    private Map map;
    private int health;

    void Start() {
        movement = Vector2.up;
        map = GameObject.Find("Map").GetComponent<Map>();
        chassisScript = this.transform.gameObject.GetComponent<Chassis>();
        turretScript = this.transform.gameObject.transform.GetChild(0).gameObject.GetComponent<Turret>();
        shottingScript = this.transform.gameObject.transform.GetChild(0).gameObject.GetComponent<Shotting>();
        health = 2;

        // TODO Place Player in the middle of the Map
        transform.position = new Vector2(map.map.GetLength(0) / 2, map.map.GetLength(1) / 2);
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        chassisScript.Input(movement);

        if (Input.GetKeyDown(KeyCode.E))
        {
            turretScript.Input(1);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            turretScript.Input(-1);
        }
        if (Input.GetKeyDown(KeyCode.Space)){
            shottingScript.Input();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            map.EnemyDies();
            Destroy(gameObject);
        }
    }
}
