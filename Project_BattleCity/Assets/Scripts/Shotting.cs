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
    private float timer;
    private float tankHeight;


    void Start()
    {
        tankHeight = 0.25f;
        timer = 0.0f;
        reloadTime = 3.0f;
        reloaded = true;
        //bulletPrefab = Resources.Load("Resources/Bullet");
        chassisScript = this.transform.parent.gameObject.GetComponent<Chassis>();
        turretScript = this.gameObject.GetComponent<Turret>();

        shottingDirection = turretScript.turretDirection;
    }

    void Update()
    {
        shottingDirection = turretScript.turretDirection;

        if (Input.GetKeyDown(KeyCode.Space) && reloaded)
        {
            Shoot();
            reloaded = false;
        }

        if (!(reloaded))
        {
            timer += Time.deltaTime;
        }

        if (reloadTime < timer)
        {
            timer = 0.0f;
            reloaded = true;
        }
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
