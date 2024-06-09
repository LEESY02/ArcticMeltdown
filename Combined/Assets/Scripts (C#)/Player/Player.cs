using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Health health;
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float speedBoost;
    [SerializeField] private float jumpForce;
    [SerializeField] private float slideSpeed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float slamDownForce;
    [SerializeField] private float defaultGravity;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallBounce;
    [SerializeField] private float moveDelay;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float jumpVolume;

    private Rigidbody2D body;
    private Animator anim;
    private bool isCrouched;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private float horizontalInput;
    private bool isFalling;
    private bool isSlamming;
    private UIManager uiManager;
    public int coinCount = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        uiManager = Resources.FindObjectsOfTypeAll<UIManager>()[0];
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        isFalling = body.velocity.y < 0 && !isGrounded();

        if (isSlamming)
        {
            body.velocity = new Vector2(0, -slamDownForce);
        }

        float xScale = Mathf.Abs(transform.localScale.x);
        float yScale = Mathf.Abs(transform.localScale.y);
        float zScale = Mathf.Abs(transform.localScale.z);

        if (horizontalInput > 0f)
            transform.localScale = new Vector3(xScale, yScale, zScale);
        else if (horizontalInput < 0f)
            transform.localScale = new Vector3(-xScale, yScale, zScale);

        if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            Crouch();
        }
        else
        {
            isCrouched = false;
        }

        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("Crouch", isCrouched);
        anim.SetBool("OnWall", OnWall() && !isGrounded());
        anim.SetBool("Falling", isFalling);

        if (isFalling) {
            body.gravityScale = defaultGravity * 4;
        } else {
            body.gravityScale = defaultGravity;
        }

        print("Grounded: " + isGrounded() + "\nWalled: " + OnWall());

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            Jump();

        if (OnWall() && !isGrounded())
        {
            body.velocity = new Vector2(horizontalInput * speed, -slideSpeed);
        }
        else
        {
            body.gravityScale = 1.5f;
            if (anim.GetBool("Run") && anim.GetBool("Crouch"))
            {
                body.velocity = new Vector2(horizontalInput * (speed + speedBoost), body.velocity.y);
            }
            else
            {
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            }

            if (isGrounded() || OnWall2())
            {
                coyoteCounter = coyoteTime;
                jumpCounter = extraJumps;
            }
            else
            {
                coyoteCounter -= Time.deltaTime;
            }
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !OnWall() && jumpCounter <= 0) return;
        SoundManager.instance.PlaySound(jumpSound, jumpVolume);

        if (OnWall())
        {
            WallJump();
        }
        else if (isGrounded() || coyoteCounter > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            coyoteCounter = 0;
        }
        else if (jumpCounter > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            jumpCounter--;
        }
    }

    private void WallJump()
    {
        // Determine the direction of the wall (left or right)
        float wallDirection = transform.localScale.x > 0 ? -1 : 1;

        // Apply the wall jump force only along the Y-axis
        body.velocity = new Vector2(body.velocity.x, jumpForce);

        // Disable horizontal input in the direction of the wall for a short moment
        StartCoroutine(DisableInputForDuration(moveDelay));

        transform.position = new Vector3(
            transform.position.x + wallBounce * wallDirection, // Adjusted by wall direction
            transform.position.y,
            transform.position.z
        );
        jumpCounter = extraJumps - 1;
    }

    private IEnumerator DisableInputForDuration(float duration)
    {
        horizontalInput = 0f; // Reset horizontal input
        yield return new WaitForSeconds(duration);
        horizontalInput = Input.GetAxis("Horizontal"); // Re-enable horizontal input after the duration
    }

    // Check if the player is sliding down the wall
    private bool isSlidingDownWall()
    {
        // If the player is on the wall and moving downwards, they are sliding down
        return OnWall() && body.velocity.y < 0;
    }

    private void Crouch()
    {
        isCrouched = true;

        if (!isGrounded())
        {
            isSlamming = true;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D hitFloor = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        return hitFloor.collider != null;
    }

    private bool OnWall()
    {
        Vector2 playerPosition = transform.position;
        RaycastHit2D hitLeft = Physics2D.Raycast(playerPosition, Vector2.left, 0.5f, wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(playerPosition, Vector2.right, 0.5f, wallLayer);
        return hitLeft.collider != null || hitRight.collider != null;
    }

    private bool OnWall2()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.2f, wallLayer);
        return colliders.Length > 0;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSlamming && isGrounded())
        {
            isSlamming = false;
            body.velocity = Vector2.zero;
        }
    }

    public bool CanAttack()
    {
        return !OnWall();
    }

    public void GameOver()
    {
        uiManager.GameOver();
    }
}
