using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Shooting : MonoBehaviour {
    public GameObject projectilePrefab;
    public GameObject projectileShadowPrefab;

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
    }
    
    public void Shoot() {
        shottingDirection = turretScript.GetCurrentDirection();

        if (reloaded) {
            onShotAudioEvent?.Play(audioSource);

            Vector3 projectileSpawnPoint = GetComponent<Renderer>().bounds.center;

            projectileSpawnPoint.y += 0.15f;
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint, Quaternion.identity); // TODO
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            // Ignore the Creator (Player/Enemy)
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), transform.parent.gameObject.GetComponent<Collider2D>());
            projectileScript.shotHeight = tankHeight;

            GameObject projectileShadow = Instantiate(projectileShadowPrefab, GetComponent<Renderer>().bounds.center, Quaternion.identity);
            ProjectileShadow projectileShadowScript = projectileShadow.GetComponent<ProjectileShadow>();

            projectileScript.ownShadow = projectileShadow;
            projectileScript.velocityDirection = shottingDirection;

            Vector3 projectileRotation = -1 * shottingDirection;
            projectile.transform.right = projectileRotation;
            projectileShadowScript.transform.right = projectileRotation;
            projectileShadowScript.velocityDirection = shottingDirection;

            projectileShadowScript.Initialize(projectile);
            reloaded = false;
            Invoke("Reload", reloadTime);
        }
    }

    private void Reload() {
        reloaded = true;
    }
}
