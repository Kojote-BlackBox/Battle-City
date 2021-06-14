using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Chassis chassisScript;
    private Sprite[] sprites;
    public Chassis.Direction turretDirection;
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

    public int inputDirection;
    public void Input(int rotationInput)
    {
        inputDirection += rotationInput;
        inputDirection = turretDirecionCheck(inputDirection);

        manualRotated = true;
    }

    private int turretDirecionCheck(int direction)
    {
        if (direction < 0)
        {
            direction = 3;
        }
        else if (direction > 3)
        {
            direction = 0;
        }

        return direction;
    }

    private Chassis.Direction KeyInputRotateTurret( )
    {
        turretDirection = chassisScript.chassisDirection;
        int currentDirection = ((int)turretDirection + inputDirection) % 4;
        return (Chassis.Direction)(currentDirection);
    }

    private void SynchronisatTurretToChassis()
    {
        if (!(chassisScript.blockInput))
        {
            turretDirection = KeyInputRotateTurret();
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
