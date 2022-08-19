using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotting : MonoBehaviour {
    public GameObject bulletPrefab;
    public GameObject bulletShadowPrefab;
    private Turret turretScript;
    private Chassis chassisScript;
    public Vector2 shottingDirection;
    private bool reloaded;
    public float reloadTime;
    private float tankHeight;

    private void OnEnable() {
        EventManager.OnGunFire += GunFire;
    }

    private void OnDisable() {
        EventManager.OnGunFire -= GunFire;
    }

    private void OnDestroy() {
        EventManager.OnGunFire -= GunFire;
    }

    void GunFire() {
        
    }

    void Start() {
        tankHeight = 0.25f;
        reloadTime = 1.0f;
        reloaded = true;
        //bulletPrefab = Resources.Load("Resources/Bullet");
        chassisScript = this.transform.parent.gameObject.GetComponent<Chassis>();
        turretScript = this.gameObject.GetComponent<Turret>();

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
        AudioManager.instance.Play("Shot");

        Vector3 bulletetSpawnPoint = GetComponent<Renderer>().bounds.center;

        bulletetSpawnPoint.y += 0.15f;
        GameObject bullet = Instantiate(bulletPrefab, bulletetSpawnPoint, Quaternion.identity) as GameObject;
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        // Ignore the Creator (Plyer/Enemy)
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), this.transform.parent.gameObject.GetComponent<Collider2D>());
        bulletScript.shotHeight = tankHeight;

        GameObject bulletShadow = Instantiate(bulletShadowPrefab, GetComponent<Renderer>().bounds.center, Quaternion.identity) as GameObject;
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
