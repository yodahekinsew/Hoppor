using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitWalls : MonoBehaviour
{
    public Transform topWall;
    public Transform leftWall;
    public Transform rightWall;

    void Start()
    {
        float height = 2 * Camera.main.orthographicSize;
        float width = 2 * Camera.main.aspect * Camera.main.orthographicSize;

        topWall.localScale = new Vector3(width + 1, 1, 0);
        topWall.localPosition = new Vector3(0, height / 2 + .5f, 0);

        leftWall.localScale = new Vector3(1, height + 1, 0);
        leftWall.localPosition = new Vector3(-width / 2 - .5f, 0, 0);

        rightWall.localScale = new Vector3(1, height + 1, 0);
        rightWall.localPosition = new Vector3(width / 2 + .5f, 0, 0);
    }
}
