using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private const float MAX_HOP_TIME = 3f;
    public GameStateManager game;
    public SpriteRenderer sprite;

    [Header("Hop Line")]
    public LineRenderer hopLine;
    public DottedLine dottedHopLine;
    public Transform hopLineStart;
    public int numSteps;
    public float stepSize;

    [Header("Physics")]
    public Rigidbody2D rigid;
    public BoxCollider2D collider;
    public float height;
    public float width;
    public float gravity;
    public float gravityScale;

    [Header("Hopping")]
    public float hopForceMultiplier;
    public float maxHopForce;
    public float minMagnitude;
    private bool canHop;
    private Vector3 hopVelocity;
    private Vector3 attachDir;
    private float attachDistance;
    private Sequence landSequence;
    private float hopTime = MAX_HOP_TIME;
    private bool hopping = false;


    [Header("Sliding")]
    public float slideSpeed;

    [Header("Touch")]
    public float touchDistanceThreshold;
    private bool touching;
    private Vector3 touchDown;

    private float cameraWidth;

    private Transform lastPlatform;
    private Vector3 lastPosition;

    private bool firstHop = true;

    private void Start()
    {
        game = GameObject.Find("/GameStateManager").GetComponent<GameStateManager>();
        cameraWidth = Camera.main.aspect * Camera.main.orthographicSize;
    }

    void Update()
    {
        if (GameStateManager.state != GameState.Play) return;

        if (Input.GetMouseButtonDown(0))
        {
            touching = true;
            touchDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (canHop && touching)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hopVelocity = hopForceMultiplier * (touchDown - mousePos);
            hopVelocity = Mathf.Clamp(hopVelocity.magnitude, 0, maxHopForce) * hopVelocity.normalized;
            dottedHopLine.DrawHopLine(hopVelocity);
        }

        if (touching && Input.GetMouseButtonUp(0))
        {
            touching = false;
            dottedHopLine.ClearHopLine();
            if (canHop
                // && !LevelSpawner.transitioning
                && Vector3.Dot(attachDir, hopVelocity.normalized) > 0
                && hopVelocity.magnitude > minMagnitude)
            {
                if (firstHop)
                {
                    firstHop = false;
                    GameObject.Find("Instructions").GetComponent<TextMeshPro>().DOColor(new Color(1, 1, 1, 0), .5f);
                }
                // if (landSequence.IsActive()) landSequence.Kill();
                BounceCounter.numBounces = 0;
                canHop = false;
                hopping = true;
                transform.parent = null;
                rigid.gravityScale = gravityScale;
                rigid.velocity = hopVelocity;
                rigid.sharedMaterial.bounciness = 0.5f;
            }
        }

        if (transform.parent != null)
        {
            // if (attachDir == Vector3.left) transform.localPosition = new Vector3(transform.localPosition.x, attachDistance, transform.localPosition.z);
            // if (attachDir == Vector3.right) transform.localPosition = new Vector3(transform.localPosition.x, -attachDistance, transform.localPosition.z);
            // if (attachDir == Vector3.up) transform.localPosition = new Vector3(transform.localPosition.x, attachDistance, transform.localPosition.z);
        }

        if (hopping)
        {
            print("Hoppping " + hopTime);
            hopTime -= Time.deltaTime;
            if (hopTime < 0)
            {
                hopping = false;
                hopTime = MAX_HOP_TIME;
                sprite.DOColor(new Color(1, 1, 1, 0), .25f).OnComplete(() =>
                {
                    transform.parent = lastPlatform;
                    transform.localPosition = lastPosition;
                    rigid.sharedMaterial.bounciness = 0;
                    rigid.velocity = Vector3.zero;
                    rigid.gravityScale = 0;
                    sprite.DOColor(new Color(1, 1, 1, 1), .25f).OnComplete(() =>
                    {
                        canHop = true;
                    });
                });
            }
        }

        if (transform.parent == null
            && (transform.position.y < -Camera.main.orthographicSize)
            || transform.position.x > cameraWidth + 2
            || transform.position.x < -cameraWidth - 2)
        {
            game.EndGame();
            Destroy(gameObject);
        }
    }

    private Vector3 GetAverageContact(Collision2D other)
    {
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        other.GetContacts(contacts);
        int numContacts = other.GetContacts(contacts);
        Vector3 averagePos = Vector3.zero;
        for (int i = 0; i < numContacts; i++)
        {
            averagePos += (Vector3)contacts[i].point;
        }
        return averagePos / numContacts;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") && canHop)
        {
            Platform otherPlatform = other.gameObject.GetComponent<Platform>();
            switch (otherPlatform.type)
            {
                case PlatformType.Normal:
                    break;
                case PlatformType.Spike:
                    break;
                case PlatformType.Bounce:
                    break;
                case PlatformType.Slide:
                    print("Slid off of the platform");
                    canHop = false;
                    transform.parent = null;
                    rigid.gravityScale = gravityScale;
                    otherPlatform.GetComponent<BoxCollider2D>().offset = Vector2.zero;
                    break;
            }
        }
    }

    private bool InBounds(float val, float lower, float higher)
    {
        if (val < lower) return false;
        if (val > higher) return false;
        return true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall")) BounceCounter.numBounces++;
        if (other.gameObject.CompareTag("Platform"))
        {
            Platform otherPlatform = other.gameObject.GetComponent<Platform>();
            BoxCollider2D otherCollider = other.collider as BoxCollider2D;
            LayerMask platformMask = LayerMask.GetMask("Platform");
            Vector3 collisionPoint = GetAverageContact(other);
            Vector3 localCollisionPoint = other.transform.InverseTransformPoint(collisionPoint);

            if (otherPlatform.type == PlatformType.Spike)
            {
                CameraShake.Instance.TriggerShake(.25f, .15f);
                Vibration.VibrateNope();
                collider.enabled = false;
                rigid.velocity = (transform.position - otherPlatform.transform.position).normalized * 10;
                return;
            }

            float boundaryDistanceX = otherCollider.size.x / 2 - Mathf.Abs(localCollisionPoint.x);
            float boundaryDistanceY = otherCollider.size.y / 2 - Mathf.Abs(localCollisionPoint.y);
            float diffX = collisionPoint.x - transform.position.x;
            float diffY = collisionPoint.y - transform.position.y;
            float playerDiffX = other.transform.position.x - transform.position.x;
            float playerDiffY = other.transform.position.y - transform.position.y;

            bool landed = false;
            // if (boundaryDistanceX > boundaryDistanceY)
            // { // Vertical Collision
            if (otherPlatform.direction == PlatformDirection.Horizontal
                && transform.position.y > collisionPoint.y
                && boundaryDistanceX >= width / 4)
            {
                rigid.position = new Vector3(
                    transform.position.x,
                    other.transform.position.y + otherCollider.size.y,
                    transform.position.z
                );
                landed = true;
                attachDir = Vector3.up;
            }
            if (otherPlatform.direction == PlatformDirection.Vertical
                && boundaryDistanceX >= height / 4)
            {
                rigid.position = new Vector3(
                    other.transform.position.y + otherCollider.size.x,
                    transform.position.y,
                    transform.position.z
                );
                landed = true;
                if (transform.position.x > collisionPoint.x) attachDir = Vector3.right;
                else attachDir = Vector3.left;
            }
            // }

            if (!landed)
            { // Horizontal Collision
                print("OOOOH YOU ACTUALLY DIDN'T LAND ON ANYTHING");
                if (otherPlatform.direction == PlatformDirection.Vertical
                    && !(otherPlatform.type == PlatformType.Slide && transform.position.y < otherPlatform.transform.position.y))
                    rigid.velocity = new Vector2(-Mathf.Abs(hopVelocity.x) * Mathf.Sign(playerDiffX), -10 * Mathf.Sign(playerDiffY));
                if (otherPlatform.direction == PlatformDirection.Horizontal)
                {
                    if (otherPlatform.movement == PlatformMovement.Moving
                        && otherPlatform.GetMovingDirection() == MovingDirection.Horizontal) rigid.velocity = new Vector2(-10 * Mathf.Sign(playerDiffX), -10 * Mathf.Sign(playerDiffY));
                    else rigid.velocity = new Vector2(-Mathf.Abs(hopVelocity.x) * Mathf.Sign(playerDiffX), -10 * Mathf.Sign(playerDiffY));
                }

            }

            if (landed)
            {
                print("Landed on a platform");
                Vibration.VibratePeek();
                rigid.sharedMaterial.bounciness = 0;
                rigid.velocity = Vector3.zero;
                rigid.gravityScale = 0;

                if (otherPlatform.IsExit() && !otherPlatform.Exited()) otherPlatform.Exit();
                transform.parent = other.transform;

                attachDistance = width / 2 + otherPlatform.thickness / 2;
                if (attachDir == Vector3.left) transform.localPosition = new Vector3(transform.localPosition.x, attachDistance, transform.localPosition.z);
                if (attachDir == Vector3.right) transform.localPosition = new Vector3(transform.localPosition.x, -attachDistance, transform.localPosition.z);
                if (attachDir == Vector3.up) transform.localPosition = new Vector3(transform.localPosition.x, attachDistance, transform.localPosition.z);

                switch (otherPlatform.type)
                {
                    case PlatformType.Normal:
                        canHop = true;
                        // landSequence = DOTween.Sequence();
                        // landSequence.Append(transform.DOLocalMoveX(0, .5f));
                        lastPlatform = transform.parent;
                        lastPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case PlatformType.Spike:
                        canHop = true;
                        // landSequence = DOTween.Sequence();
                        // landSequence.Append(transform.DOLocalMoveX(0, .5f));
                        break;
                    case PlatformType.Bounce:
                        rigid.velocity = otherPlatform.transform.up * otherPlatform.bounceForce;
                        rigid.gravityScale = 3;
                        transform.parent = null;
                        break;
                    case PlatformType.Slide:
                        otherPlatform.GetComponent<BoxCollider2D>().offset = new Vector2(height / 2, 0);
                        canHop = true;
                        rigid.velocity = Vector3.down * slideSpeed;
                        break;
                }

                if (canHop)
                {
                    hopping = false;
                    hopTime = MAX_HOP_TIME;
                }
            }
        }
    }
}
