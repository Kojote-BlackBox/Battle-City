using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Direction tankDirection;
    private BasicMovement parent;
    private Sprite[] sprites;

    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<BasicMovement>();
        sprites = Resources.LoadAll<Sprite>("Tanks/T1");
    }

    void Update()
    {
        this.tankDirection = parent.tankDirection;

        switch (this.tankDirection)
        {
            case Direction.Up:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[4];
                break;
            case Direction.Down:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[3];
                break;
            case Direction.Left:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[9];
                break;
            case Direction.Right:
                this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[8];
                break;
            default:
                Debug.Log("ERROR: Tower.cs line 35 -  can not order right tower direction.");
                break;
        }
    }
}
