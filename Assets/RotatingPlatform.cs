using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotatingDirection
{
    Clockwise,
    Counterclockwise
}

public class RotatingPlatform : MonoBehaviour
{
    public RotatingDirection direction;
    public Platform platform;
    public float rotationSpeed;
    private float rotation;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (direction == RotatingDirection.Clockwise)
        {
        }
        switch (direction)
        {
            case RotatingDirection.Clockwise:
                rotation -= rotationSpeed * Time.deltaTime;
                if (rotation < -360) rotation += 360;
                break;
            case RotatingDirection.Counterclockwise:
                rotation += rotationSpeed * Time.deltaTime;
                if (rotation > 360) rotation -= 360;
                break;
        }

        platform.transform.localEulerAngles = new Vector3(0, 0, rotation);
    }
}
