using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLine : MonoBehaviour
{
    public List<SpriteRenderer> dots;
    public float stepSize;
    public float minMagnitude;
    public float maxMagnitude;
    public float gravity;
    public Color dotColor;

    public void DrawHopLine(Vector3 initialVelocity)
    {
        Color color = dotColor;
        color.a = (initialVelocity.magnitude - minMagnitude) / (maxMagnitude - minMagnitude);

        List<Vector3> linePositions = new List<Vector3>();
        Vector3 velocity = initialVelocity;
        float startX = transform.position.x;
        float startY = transform.position.y;
        float t = 0;
        linePositions.Add(new Vector3(startX, startY, transform.position.z));
        for (int i = 0; i < dots.Count; i++)
        {
            float x = startX + velocity.x * t;
            float y = startY + velocity.y * t - gravity / 2 * t * t;
            t += stepSize;
            dots[i].color = color;
            dots[i].transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    public void ClearHopLine()
    {
        for (int i = 0; i < dots.Count; i++)
        {
            Color faded = dots[i].color;
            faded.a = 0;
            dots[i].color = faded;
        }
    }
}
