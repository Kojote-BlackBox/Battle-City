using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Chassis chassisScript;
    private Sprite[] sprites;
    private int keyInputTurretRotation;
    Chassis.Direction turretDirection;
    public float turretRotationTime;
    private SpriteRenderer spriteRenderer;
    private bool manualRotated;

    void Start()
    {
        chassisScript = this.transform.parent.gameObject.GetComponent<Chassis>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        turretDirection = chassisScript.chassisDirection;
        sprites = Resources.LoadAll<Sprite>("Tanks/T1");
        turretRotationTime = 0.35f;
        manualRotated = false;
    }

    void Update()
    {
        SynchronisatTurretToChassis();
    }

    private Chassis.Direction KeyInputRotateTurret(Chassis.Direction turretDirection )
    {
        int direction = (int)turretDirection;
        // Rotate Turret one Step left
        if (Input.GetKeyDown(KeyCode.E))
        {
            keyInputTurretRotation += 1;
            manualRotated = true;
        }

        // Rotate Turret one Step Right
        if (Input.GetKeyDown(KeyCode.Q))
        {
            keyInputTurretRotation -= 1;
            manualRotated = true;
            if (keyInputTurretRotation < 0)
            {
                keyInputTurretRotation = 3;
            }
        }
      
        return (Chassis.Direction) ((direction + keyInputTurretRotation) % 4);
    }

    private void SynchronisatTurretToChassis()
    {
        if (!(chassisScript.blockInput))
        {
            turretDirection = KeyInputRotateTurret(chassisScript.chassisDirection);
        }

        switch (turretDirection)
        {
            case Chassis.Direction.Up:
                 StartCoroutine(ChangeTurretGui(sprites[4], turretRotationTime));
                 break;
            case Chassis.Direction.Right:
                StartCoroutine(ChangeTurretGui(sprites[8], turretRotationTime));
                break;
            case Chassis.Direction.Down:
                StartCoroutine(ChangeTurretGui(sprites[3], turretRotationTime));
                break;
            case Chassis.Direction.Left:
                StartCoroutine(ChangeTurretGui(sprites[9], turretRotationTime));
                break;
            default:
                break;
        }

        IEnumerator ChangeTurretGui(Sprite sprite, float rotationTime)
        {
            // delay
            if (manualRotated)
            {
                manualRotated = false;
                yield return new WaitForSeconds(rotationTime);
            }
            spriteRenderer.sprite = sprite;
        }
    }
}
