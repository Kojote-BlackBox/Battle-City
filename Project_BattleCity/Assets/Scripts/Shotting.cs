using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletShadowPrefab;
    private Turret turretScript;
    private Chassis chassisScript;
    public Vector2 shottingDirection;
    private bool reloaded;
    public float reloadTime;
    private float tankHeight;


    void Start() {
        tankHeight = 0.25f;
        reloadTime = 1.0f;
        reloaded = true;
        //bulletPrefab = Resources.Load("Resources/Bullet");
        chassisScript = this.transform.parent.gameObject.GetComponent<Chassis>();
        turretScript = this.gameObject.GetComponent<Turret>();

        shottingDirection = turretScript.turretDirection;
    }

    public void Input() {
        shottingDirection = turretScript.turretDirection;
        if (reloaded)
        {
            Shoot();
            reloaded = false;
            Invoke("Reload", reloadTime);
        }
    }

    private void Reload()
    {
        reloaded = true;
    }
   
    public void Shoot() {
        Vector3 buttetSpawnPoint = GetComponent<Renderer>().bounds.center;
        buttetSpawnPoint.y += 0.15f;
        GameObject bullet = Instantiate(bulletPrefab, buttetSpawnPoint, Quaternion.identity) as GameObject;
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), this.transform.parent.gameObject.GetComponent<Collider2D>());
        bullet.GetComponent<Bullet>().shotHeight = tankHeight;

        GameObject bulletShadow = Instantiate(bulletShadowPrefab, GetComponent<Renderer>().bounds.center, Quaternion.identity) as GameObject;
        bullet.GetComponent<Bullet>().ownShadow = bulletShadow;

        bullet.GetComponent<Bullet>().shotDirection = shottingDirection;
        // TODO Rotarion Arichmeik depends on shottingDirection
        //z: 0 = left, 90 = down, -90 = up, 180 = right
    
        bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        bulletShadow.GetComponent<Bullet>().shotDirection = shottingDirection;
        //z: 0 = left, 90 = down, -90 = up, 180 = right
        bulletShadow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        bulletShadow.transform.position = new Vector2(transform.position.x, transform.position.y - tankHeight); 
    }
}
