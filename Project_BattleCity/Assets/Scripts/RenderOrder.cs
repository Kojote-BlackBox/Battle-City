using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOrder : MonoBehaviour {

    private Map map;
    private int top, bottom, left, right;
    private bool skip, skipTop, skipBottom, skipLeft, skipRight;
    private GameObject centerTile;
    private float loadingDelay = 0.5f;

    void Start() {
        map = GameObject.Find("Map").GetComponent<Map>();

        skip = false;
        skipTop = false;
        skipBottom = false;
        skipLeft = false;
        skipRight = false;
    }

    void Update() {
        RenderCameraFrustum();
    }

    void RenderCameraFrustum() {
        centerTile = map.map[(int)transform.position.x, (int)transform.position.y, 0];
        centerTile.SetActive(true);
        //centerTile.GetComponent<SpriteRenderer>().enabled = true;

        if (!skip) {
            SetBorders();

        // Update Border
        } else {
            int x = (int)centerTile.GetComponent<Tile>().position.x;
            int y = (int)centerTile.GetComponent<Tile>().position.y;

            for (int cols = x - left; cols < x + right; cols++) {
                for (int rows = y - bottom; rows < y + top; rows++) {
                    for (int z = 0; z < map.layer; z++) {
                        if(cols >= 0 && cols < map.cols && rows >= 0 && rows < map.rows) {
                            if (map.map[cols, rows, z] != null) {
                                map.map[cols, rows, z].SetActive(true);
                                //map.map[cols, rows, z].GetComponent<SpriteRenderer>().enabled = true;
                            }
                        }
                    }
                }
            }
        }
    }

    private void SetBorders() {
        int x = (int)centerTile.GetComponent<Tile>().position.x;
        int y = (int)centerTile.GetComponent<Tile>().position.y;

        if (!skipTop) {
            top = GetTop(x, y);
        }
        
        if (!skipBottom) {
            bottom = GetBottom(x, y);
        }
        
        if (!skipLeft) {
            left = GetLeft(x, y);
        }
        
        if (!skipRight) {
            right = GetRight(x, y);
        }

        if(true == skipTop && true == skipBottom && true == skipLeft && true == skipRight) {

            for (int cols = x - left; cols < x + right; cols++) {
                for (int rows = y - bottom; rows < y + top; rows++) {
                    for (int z = 0; z < map.layer; z++) {
                        if (map.map[cols, rows, z] != null) {
                            map.map[cols, rows, z].SetActive(true);
                            //map.map[cols, rows, z].GetComponent<SpriteRenderer>().enabled = true;
                        }
                    }
                }
            }

            skip = true;
        }
    }

    private int GetTop(int x, int y) {
        int returnValue = 0;

        while (map.map[x, y + returnValue, 0].activeSelf) {
            returnValue++;
            map.map[x, y + returnValue, 0].SetActive(true);
            //map.map[x, y + returnValue, 0].GetComponent<SpriteRenderer>().enabled = true;

            if (!map.map[x, y + returnValue, 0].GetComponent<Renderer>().isVisible) {
                Invoke("StopTop", loadingDelay);
                break;
            }
        }

        return ++returnValue;
    }

    private void StopTop() {
        skipTop = true;
    }

    private int GetBottom(int x, int y) {
        int returnValue = 0;

        while (map.map[x, y - returnValue, 0].activeSelf) {
            returnValue++;
            map.map[x, y - returnValue, 0].SetActive(true);
            //map.map[x, y - returnValue, 0].GetComponent<SpriteRenderer>().enabled = true;


            if (!map.map[x, y - returnValue, 0].GetComponent<Renderer>().isVisible) {
                Invoke("StopBottom", loadingDelay);
                break;
            }
        }

        return ++returnValue;
    }

    private void StopBottom() {
        skipBottom = true;
    }

    private int GetLeft(int x, int y) {
        int returnValue = 0;

        while (map.map[x - returnValue, y, 0].GetComponent<Renderer>().isVisible) {
            returnValue++;
            map.map[x - returnValue, y, 0].SetActive(true);
            //map.map[x - returnValue, y, 0].GetComponent<SpriteRenderer>().enabled = true;

            if (!map.map[x - returnValue, y, 0].GetComponent<Renderer>().isVisible) {
                Invoke("StopLeft", loadingDelay);
                break;
            }
        }

        return ++returnValue;
    }

    private void StopLeft() {
        skipLeft = true;
    }

    private int GetRight(int x, int y) {
        int returnValue = 0;

        while (map.map[x + returnValue, y, 0].GetComponent<Renderer>().isVisible) {
            returnValue++;
            map.map[x + returnValue, y, 0].SetActive(true);
            //map.map[x + returnValue, y, 0].GetComponent<SpriteRenderer>().enabled = true;

            if (!map.map[x + returnValue, y, 0].GetComponent<Renderer>().isVisible) {
                Invoke("StopRight", loadingDelay);
                break;
            }
        }

        return ++returnValue;
    }

    private void StopRight() {
        skipRight = true;
    }
}
