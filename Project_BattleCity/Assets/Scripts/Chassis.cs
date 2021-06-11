using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chassis : MonoBehaviour
{
    public Animator animator;
    public float forwardSpeed;
    public float backwardSpeed;
    public float chassisTurnTime;
    public bool blockInput;

    // needed for movement Animation.
    public Vector2 movement;

    public enum Direction { Up, Right, Down, Left };
    public Direction chassisDirection;
    private Direction driveDirection;

    private float currentSpeed;
    private new Rigidbody2D rigidbody;

    private enum Block { free, x, y };
    private Block playerConrol = Block.free;

    void Start()
    {
        chassisTurnTime = 0.40f;
        blockInput = false;
        movement = Vector2.up;
        rigidbody = this.GetComponent<Rigidbody2D>();
        forwardSpeed = 1.2f;
        backwardSpeed = forwardSpeed / 2;
        chassisDirection = Direction.Up;
    }

    /*
     * Movement just in one direction alowed.
     * First Input get the Prio
    */
    void Update()
    {
        if (!(blockInput))
        {
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
        
        BlockMovement();
        UpdateChassieGui();
    }

    void UpdateChassieGui()
    {
        // Input.GetAxis("Horizontal")
        animator.SetFloat("Horizontal", movement.x);

        // Inpu.GetAxis("Vertical");
        animator.SetFloat("Vertical", movement.y);
    }

    // Allow up, down, left, right forbid strive, implement diverent reverse Speed.
    void BlockMovement()
    {
        switch (playerConrol)
        {
            case Block.free:
                // movement Horizontal
                if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
                {
                    playerConrol = Block.x;
                }

                // movement Vertical
                else if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
                {
                    playerConrol = Block.y;
                }
                break;

            case Block.x:
                if(Mathf.Abs(movement.x) < Mathf.Abs(movement.y))
                {
                    if(movement.y < 0.0f)
                    {
                        if(chassisDirection == Direction.Left)
                        {
                            ChassisDirectionGui(Direction.Left);
                        } else
                        {
                            ChassisDirectionGui(Direction.Right);
                        }
                    }
                    else
                    {
                        if(chassisDirection == Direction.Left)
                        {
                            ChassisDirectionGui(Direction.Right);
                        } else
                        {
                            ChassisDirectionGui(Direction.Left);
                        }
                    }
                    
                    playerConrol = Block.free;
                } else
                {
                    movement.y = 0.0f;
                }

                if(movement.x < 0.0f)
                {
                    ReverseGear(Direction.Left);
                } else {
                    ReverseGear(Direction.Right);
                }
                
                break;
            case Block.y:
                if (Mathf.Abs(movement.y) < Mathf.Abs(movement.x))
                {
                    if (movement.x < 0)
                    {
                        if (chassisDirection == Direction.Up)
                        {
                            ChassisDirectionGui(Direction.Left);
                        }
                        else
                        {
                            ChassisDirectionGui(Direction.Right);
                        }
                    }
                    else
                    {
                        if (chassisDirection == Direction.Up)
                        {
                            ChassisDirectionGui(Direction.Right);
                        }
                        else
                        {
                            ChassisDirectionGui(Direction.Left);
                        }
                    }
                    playerConrol = Block.free;
                } else
                {
                    movement.x = 0.0f;
                }

                if (movement.y < 0.0f)
                {
                    ReverseGear(Direction.Down);
                } else {
                    ReverseGear(Direction.Up);
                }
                break;
            default:
                break;
        }
        
    }

    void ChassisDirectionGui(Direction turnDirection)
    {
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        blockInput = true;
        animator.enabled = !animator.enabled;
        StartCoroutine(FreezeTank());

        // Soften Rotation (Animation)
        if (turnDirection == Direction.Left)
        {
            StartCoroutine(RotateTankLeft());
        }
        else
        {
            StartCoroutine(RotateTankRight());
        }
    }

    IEnumerator RotateTankLeft()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < chassisTurnTime)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, 16.0f), (elapsedTime / chassisTurnTime  / 4));

            // Resett Rotation
            if (elapsedTime > chassisTurnTime)
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator RotateTankRight()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < chassisTurnTime)
        {
            elapsedTime += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, -16.0f), (elapsedTime / chassisTurnTime / 4));

            // Resett Rotation
            if (elapsedTime > chassisTurnTime)
            {
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FreezeTank()
    {
        yield return new WaitForSeconds(chassisTurnTime);

        rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rigidbody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        blockInput = false;
        animator.enabled = !animator.enabled;       
    }

    void ReverseGear(Direction newDirection)
    {
        driveDirection = newDirection;

        switch (chassisDirection)
        {
            case Direction.Up:
                if (driveDirection == Direction.Down)
                {
                    currentSpeed = backwardSpeed;
                } else
                {
                    currentSpeed = forwardSpeed;
                    chassisDirection = driveDirection;
                }
                break;
            case Direction.Down:
                if (driveDirection == Direction.Up)
                {
                    currentSpeed = backwardSpeed;
                }
                else
                {
                    currentSpeed = forwardSpeed;
                    chassisDirection = driveDirection;
                }
                break;
            case Direction.Left:
                if (driveDirection == Direction.Right)
                {
                    currentSpeed = backwardSpeed;
                }
                else
                {
                    currentSpeed = forwardSpeed;
                    chassisDirection = driveDirection;
                }
                break;
            case Direction.Right:
                if (driveDirection == Direction.Left)
                {
                    currentSpeed = backwardSpeed;
                }
                else
                {
                    currentSpeed = forwardSpeed;
                    chassisDirection = driveDirection;
                }
                break;
            default:
                Debug.Log("ERROR NO Direction (DirectionDependetSpeed)");
                break;
        }
    }

    void FixedUpdate()
    {
        rigidbody.MovePosition((Vector2)transform.position + (movement * currentSpeed * Time.fixedDeltaTime));
    }
}
