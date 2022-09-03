using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public SOProjectile coreValue;

    // private Value Set - copy from coreValue
    [Header("Add to core Values")]
    [Space(5)]
    [SerializeField]
    public float maxProjectileDistance;
    [SerializeField]
    private float projectileDistance;
    [SerializeField]
    private Vector2 origin;
    [SerializeField]
    private new Rigidbody2D rigidbody;
    [SerializeField]
    public Vector2 velocityDirection;
    [SerializeField]
    private bool shooted;
    [SerializeField]
    public int damage;
    [SerializeField]
    public float shotHeight;
    [SerializeField]
    public GameObject ownShadow;
    [SerializeField]
    public float projectileSpeed;

    private void Awake() {
        // TODO move to turret
        // projectileSpeed = coreValue.speed;
        damage = coreValue.damage;
        //coreValue.explosiveRadius
        maxProjectileDistance = coreValue.distance;
    }

    void Start() {
        shooted = false;

        // TODO move to turret
        projectileSpeed = 5.0f;
        rigidbody = this.GetComponent<Rigidbody2D>();
        origin = transform.position;
    }

    void Update() {
        projectileDistance = Vector2.Distance(origin, transform.position);
        if (maxProjectileDistance < projectileDistance) {
            Destroy(ownShadow);
            Destroy(gameObject);
        }

        if (velocityDirection != null && !shooted) {
            shooted = true;
            rigidbody.velocity = velocityDirection * projectileSpeed;
        }

        // Linear Bullet Drop
        float bulletFlyTimeInSec = maxProjectileDistance / projectileSpeed;
        float bulletDropEachSec = shotHeight / bulletFlyTimeInSec;
        //float bulletDropEachFrame = bulletDropEachSec * Time.deltaTime;
        //transform.position = new Vector2(transform.position.x, transform.position.y - bulletDropEachFrame);

        // Parabola Bullet Drop

        // x = v * t
        // y = -1/2g * t^2

        /* (-1/2 g/v^2) * s^2
         * 
          */


         float t = Time.deltaTime;
         float g = 1.5f;
         bulletDropEachFrame += g * t * t;
         transform.position = new Vector2(transform.position.x, transform.position.y - bulletDropEachFrame);
    }

    float bulletDropEachFrame = 0;

    void OnTriggerEnter2D(Collider2D hitInfo) {
        EnemyController enemy = hitInfo.GetComponent<EnemyController>();
        PlayerController player = hitInfo.GetComponent<PlayerController>();


        if (!hitInfo.CompareTag("Map") && !hitInfo.CompareTag("Finish")) {
            Destroy(ownShadow);
            Destroy(gameObject);
        }
    }
}
