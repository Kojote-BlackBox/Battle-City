using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float maxBulletDistance;
    private float bulletDistance;
    private Vector2 origin;
    private new Rigidbody2D rigidbody;
    public Vector2 shotDirection;
    private bool shooted;
    public int damage;
    public float shotHeight;
    public GameObject ownShadow;
 
    void Start()
    {
        damage = 1;
        shooted = false;
        bulletSpeed = 5.0f;
        maxBulletDistance = 4.0f;
        rigidbody = this.GetComponent<Rigidbody2D>();
        origin = transform.position;
    }

    void Update()
    {
        bulletDistance = Vector2.Distance(origin, transform.position);
        if (maxBulletDistance < bulletDistance){
            Destroy(gameObject);
        }

        if(shotDirection != null && !shooted)
        {
            shooted = true;
            rigidbody.velocity = shotDirection * bulletSpeed;
        }

        // Parabola Bullet Drop
        // TODO

        // Linear Bullet Drop
        float bulletFlyTimeInSec = maxBulletDistance / bulletSpeed;
        float bulletDropEachSec = shotHeight / bulletFlyTimeInSec;
        float bulletDropEachFrame = bulletDropEachSec * Time.deltaTime;
        transform.position = new Vector2(transform.position.x, transform.position.y - bulletDropEachFrame);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(ownShadow);
        Destroy(gameObject);
    }
}
