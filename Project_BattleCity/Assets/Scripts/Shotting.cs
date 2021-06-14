using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletShadowPrefab;
    private Turret turretScript;
    private Chassis chassisScript;
    public Chassis.Direction shottingDirection;
    private bool reloaded;
    public float reloadTime;
    private float tankHeight;


    void Start()
    {
        tankHeight = 0.25f;
        reloadTime = 1.0f;
        reloaded = true;
        //bulletPrefab = Resources.Load("Resources/Bullet");
        chassisScript = this.transform.parent.gameObject.GetComponent<Chassis>();
        turretScript = this.gameObject.GetComponent<Turret>();

        shottingDirection = turretScript.turretDirection;
    }

    public void Input()
    {
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
   
    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, GetComponent<Renderer>().bounds.center, Quaternion.identity) as GameObject;
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), this.transform.parent.gameObject.GetComponent<Collider2D>());
        bullet.GetComponent<Bullet>().shotHeight = tankHeight;

        GameObject bulletShadow = Instantiate(bulletShadowPrefab, GetComponent<Renderer>().bounds.center, Quaternion.identity) as GameObject;
        bullet.GetComponent<Bullet>().ownShadow = bulletShadow;

        switch (shottingDirection)
        {
            case Chassis.Direction.Up:
                bullet.GetComponent<Bullet>().shotDirection = Vector2.up;
                bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);

                bulletShadow.GetComponent<Bullet>().shotDirection = Vector2.up;
                bulletShadow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                bulletShadow.transform.position = new Vector2(transform.position.x, transform.position.y - tankHeight); 
                break;
            case Chassis.Direction.Right:
                bullet.GetComponent<Bullet>().shotDirection = Vector2.right;
                bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);

                bulletShadow.GetComponent<Bullet>().shotDirection = Vector2.right;
                bulletShadow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                bulletShadow.transform.position = new Vector2(transform.position.x, transform.position.y - tankHeight);
                break;
            case Chassis.Direction.Down:
                bullet.GetComponent<Bullet>().shotDirection = Vector2.down;
                bullet.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);

                bulletShadow.GetComponent<Bullet>().shotDirection = Vector2.down;
                bulletShadow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                bulletShadow.transform.position = new Vector2(transform.position.x, transform.position.y - tankHeight / 2);
                break;
            case Chassis.Direction.Left:
                bullet.GetComponent<Bullet>().shotDirection = Vector2.left;

                bulletShadow.GetComponent<Bullet>().shotDirection = Vector2.left;
                bulletShadow.transform.position = new Vector2(transform.position.x, transform.position.y - tankHeight);
                break;
            default:
                break;
        }
    }
}
