using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum PlatformDirection
{
    Horizontal,
    Vertical
}

public enum PlatformMovement
{
    Static,
    Moving
}

public enum PlatformRotation
{
    Static,
    Rotating
}

public enum PlatformType
{
    Normal,
    Spike,
    Bounce,
    Slide
}

public class Platform : MonoBehaviour
{
    public Level level;
    public PlatformDirection direction = PlatformDirection.Horizontal;
    public PlatformType type = PlatformType.Normal;
    public PlatformMovement movement = PlatformMovement.Static;
    public PlatformRotation rotation = PlatformRotation.Static;
    public SpriteRenderer sprite;
    public float thickness;
    public ParticleSystem landEffect;
    public Transform goalLine;
    public float bounceForce;
    public bool exitPlatform;
    public bool startingPlatform;
    private bool exited;
    private BoxCollider2D collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        thickness = collider.size.y;
    }

    public bool IsExit()
    {
        return exitPlatform;
    }

    public bool Exited()
    {
        return exited;
    }

    public void Exit()
    {
        if (startingPlatform) return;
        landEffect.Play();
        goalLine.gameObject.SetActive(false);
        exited = true;
        level.Exit();
    }

    public void Deactivate()
    {
        Color clear = sprite.color;
        clear.a = 0f;
        sprite.DOColor(clear, LevelSpawner.LEVEL_TRANSITION_TIME);
        collider.enabled = false;
        // Special Cases
        if (movement == PlatformMovement.Moving)
        {
            transform.parent.GetComponent<MovingPlatform>().Deactivate();
        }
    }

    public void SemiDeactivate()
    {
        if (type == PlatformType.Spike) print("Deactivating the spike!!!!");
        Color faded = sprite.color;
        faded.a = .5f;
        sprite.DOColor(faded, LevelSpawner.LEVEL_TRANSITION_TIME);

        // Special Cases
        if (movement == PlatformMovement.Moving)
        {
            transform.parent.GetComponent<MovingPlatform>().SemiDeactivate();
        }
    }
    public void Reactivate()
    {
        Color color = sprite.color;
        color.a = 1f;
        sprite.DOColor(color, LevelSpawner.LEVEL_TRANSITION_TIME);

        // Special Cases
        if (movement == PlatformMovement.Moving)
        {
            transform.parent.GetComponent<MovingPlatform>().Reactivate();
        }
    }

    public MovingDirection GetMovingDirection()
    {
        return transform.parent.GetComponent<MovingPlatform>().direction;
    }

    public Vector3 GetPosition()
    {
        if (movement == PlatformMovement.Moving) return transform.parent.position;
        return transform.position;
    }
}
