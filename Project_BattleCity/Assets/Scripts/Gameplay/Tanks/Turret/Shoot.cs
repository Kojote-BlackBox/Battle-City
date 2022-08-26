using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Shoot : MonoBehaviour {
    public GameObject bulletPrefab;
    public GameObject bulletShadowPrefab;

    private Turret turretScript;
    public Vector2 shottingDirection;

    private bool reloaded; // TODO: gehört zu turret
    public float reloadTime; // TODO: gehört zu turret
    private float tankHeight;

    [SerializeField]
    private AudioEvent onShotAudioEvent;
    private AudioSource audioSource;

    private void Awake()
    {
        turretScript = gameObject.GetComponent<Turret>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Start() {
        tankHeight = 0.25f;
        reloadTime = 1.0f;
        reloaded = true;
        //bulletPrefab = Resources.Load("Resources/Bullet");

        shottingDirection = turretScript.turretDirection.normalized;
    }
    
    public void Input() {
        shottingDirection = turretScript.turretDirection.normalized;
        
        if (reloaded) {

            Shot();
            reloaded = false;
            Invoke("Reload", reloadTime);
        }
    }

    private void Reload() {
        reloaded = true;
    }

    public void Shot() {
        onShotAudioEvent?.Play(audioSource);

        Vector3 bulletetSpawnPoint = GetComponent<Renderer>().bounds.center;

        bulletetSpawnPoint.y += 0.15f;
        GameObject bullet = Instantiate(bulletPrefab, bulletetSpawnPoint, Quaternion.identity); // TODO
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        // Ignore the Creator (Player/Enemy)
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.parent.gameObject.GetComponent<Collider2D>());
        bulletScript.shotHeight = tankHeight;

        GameObject bulletShadow = Instantiate(bulletShadowPrefab, GetComponent<Renderer>().bounds.center, Quaternion.identity);
        BulletShadow bulletShadowScript = bulletShadow.GetComponent<BulletShadow>();

        bulletScript.ownShadow = bulletShadow;
        bulletScript.velocityDirection = shottingDirection;

        Vector3 bulletRotation = -1 * shottingDirection;
        bullet.transform.right = bulletRotation;
        bulletShadowScript.transform.right = bulletRotation;
        bulletShadowScript.velocityDirection = shottingDirection;

        bulletShadowScript.Initialize(bullet);
        //bulletShadow.transform.position = new Vector2(transform.position.x, transform.position.y - tankHeight);
    }
}
