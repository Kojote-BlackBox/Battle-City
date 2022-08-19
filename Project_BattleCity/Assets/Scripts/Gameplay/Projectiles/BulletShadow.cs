using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShadow : MonoBehaviour {
    private float bulletSpeed;
    private float maxBulletDistance;
    private bool shooted;

    private Vector2 origin;
    private new Rigidbody2D rigidbody;
    public Vector2 velocityDirection;

    void Start() {
        rigidbody = this.GetComponent<Rigidbody2D>();
        origin = transform.position;


        transform.position = new Vector2(transform.position.x, transform.position.y - 0.15f);
    }

    public void Initialize(GameObject bulletOrigin) {
        Bullet bulletScript = bulletOrigin.GetComponent<Bullet>();

        this.bulletSpeed = bulletScript.bulletSpeed;
        this.maxBulletDistance = bulletScript.maxBulletDistance;
        this.velocityDirection = bulletScript.velocityDirection;
        this.shooted = false;
    }

    void Update() {
        if (velocityDirection != null && !shooted) {
            shooted = true;
            rigidbody.velocity = velocityDirection * bulletSpeed;
        }
    }
}
