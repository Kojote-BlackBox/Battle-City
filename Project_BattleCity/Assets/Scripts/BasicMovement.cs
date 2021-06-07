using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Down, Left, Right };

public class BasicMovement : MonoBehaviour
{
    public Direction tankDirection;
    public Animator animator;

    void Start()
    {
        tankDirection = Direction.Up;
    }

    /*
     * Movement just in one direction alowed.
     * First Input get the Prio
    */
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        if (movement.y == 0.0f)
        {
            if (movement.x < 0.0f)
            {
                tankDirection = Direction.Left;
            }
            else if (movement.x > 0.0f)
            {
                tankDirection = Direction.Right;
            }
        }

        if (movement.x == 0.0f)
        {
            if (movement.y < 0.0f)
            {
                tankDirection = Direction.Down;
            }
            else if (movement.y > 0.0f)
            {
                tankDirection = Direction.Up;
            }
        }

        if (movement.x != 0.0f && movement.y != 0.0f)
        {
            if (tankDirection == Direction.Left || tankDirection == Direction.Right)
            {
                movement.y = 0.0f;
            } 

            if (tankDirection == Direction.Down || tankDirection == Direction.Up)
            {
                movement.x = 0.0f;
            }
        }

        // Input.GetAxis("Horizontal")
        animator.SetFloat("Horizontal", movement.x);
        // Inpu.GetAxis("Vertical");
        animator.SetFloat("Vertical", movement.y);

        transform.position = transform.position + movement * Time.deltaTime;
    }
}
