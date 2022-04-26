using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MovingDirection
{
    Horizontal,
    Vertical
}

public enum PlatformAlignment
{
    Parallel,
    Perpendicular
}

public class MovingPlatform : MonoBehaviour
{
    public MovingDirection direction;
    public PlatformAlignment alignment;
    public Platform platform;
    public Transform platformSlider;
    public float parallelBoundary;
    public float perpendicularBoundary;
    public float moveSpeed;
    private bool movingLeft;

    void Update()
    {
        float boundary = (alignment == PlatformAlignment.Parallel ? parallelBoundary : perpendicularBoundary);
        if (movingLeft)
        {
            if (direction == MovingDirection.Horizontal)
            {
                platform.transform.localPosition += Vector3.left * moveSpeed * Time.deltaTime;
                if (platform.transform.localPosition.x <= -boundary)
                {
                    platform.transform.localPosition = Vector3.left * boundary;
                    movingLeft = false;
                }
            }
            else
            {
                platform.transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
                if (platform.transform.localPosition.y >= boundary)
                {
                    platform.transform.localPosition = Vector3.up * boundary;
                    movingLeft = false;
                }
            }
        }
        else
        {
            if (direction == MovingDirection.Horizontal)
            {
                platform.transform.localPosition += Vector3.right * moveSpeed * Time.deltaTime;
                if (platform.transform.localPosition.x >= boundary)
                {
                    platform.transform.localPosition = Vector3.right * boundary;
                    movingLeft = true;
                }
            }
            else
            {
                platform.transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
                if (platform.transform.localPosition.y <= -boundary)
                {
                    platform.transform.localPosition = Vector3.down * boundary;
                    movingLeft = true;
                }
            }

        }
    }

    public void Deactivate()
    {
        SpriteRenderer sliderSprite = platformSlider.GetComponent<SpriteRenderer>();
        Color clear = sliderSprite.color;
        clear.a = 0f;
        sliderSprite.DOColor(clear, LevelSpawner.LEVEL_TRANSITION_TIME);
    }

    public void SemiDeactivate()
    {
        SpriteRenderer sliderSprite = platformSlider.GetComponent<SpriteRenderer>();
        Color faded = sliderSprite.color;
        faded.a = .5f;
        sliderSprite.DOColor(faded, LevelSpawner.LEVEL_TRANSITION_TIME);
    }

    public void Reactivate()
    {
        SpriteRenderer sliderSprite = platformSlider.GetComponent<SpriteRenderer>();
        Color color = sliderSprite.color;
        color.a = 1f;
        sliderSprite.DOColor(color, LevelSpawner.LEVEL_TRANSITION_TIME);
    }
}
