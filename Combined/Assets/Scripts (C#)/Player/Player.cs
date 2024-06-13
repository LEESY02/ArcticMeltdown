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

    [Header("Particles")]
    [SerializeField] private GameObject leftSlideEffect;
    [SerializeField] private GameObject rightSlideEffect;
    [SerializeField] private GameObject wallSlideEffect;

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
    [SerializeField] private float horizontalCooldownTime; // Cooldown duration
    private bool canMoveRight;

    [Header("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float jumpVolume;

    [Header("Colliders")]
    [SerializeField] private float crouchHeight;
    [SerializeField] private float crouchWidth;
    [SerializeField] private float offsetChangeY;
    [SerializeField] private float crouchRadius;

    private Tracker tracker;
    private Rigidbody2D body;
    private Animator anim;
    private bool isCrouched;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private float horizontalInput;
    private bool isFalling;
    private bool isSlamming;
    private UIManager uiManager;
    private float boxColliderHeight; // original height
    private float boxColliderWidth; // original width
    private float boxColliderOffsetY; // original y offset
    private float circleColliderRadius; // original radius

    public int coinCount;

    private bool canMoveHorizontally = true; // To handle horizontal movement cooldown


    private void Awake()
    {
        tracker = FindObjectOfType<Tracker>();
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        uiManager = Resources.FindObjectsOfTypeAll<UIManager>()[0];
        boxColliderHeight = boxCollider.size.y; // Store the original height
        boxColliderWidth = boxCollider.size.x; // Store the original width
        boxColliderOffsetY = boxCollider.offset.y; // Store the original y offset
        circleColliderRadius = circleCollider.radius; // Store the original radius
        coinCount = tracker.coinCount;
    }

    private void Update()
    {
        
        if (!canMoveHorizontally && ((canMoveRight && Input.GetAxis("Horizontal") < 0) || (!canMoveRight && Input.GetAxis("Horizontal") > 0)))
        {
            horizontalInput = 0;
        } 
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }

        Debug.Log("canMoveHorizontally: " + canMoveHorizontally);

        isFalling = body.velocity.y < 0 && !isGrounded();

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
            StandUp();
        }

        if (isCrouched && horizontalInput != 0 && !OnWall() && !isFalling) // sliding
        {
            if (horizontalInput > 0f)
            {
                rightSlideEffect.SetActive(true);
                // Debug.Log("rightslide");
            }
            else
            {
                leftSlideEffect.SetActive(true);
                // Debug.Log("leftslide");
            }
        }
        else
        {
            leftSlideEffect.SetActive(false);
            rightSlideEffect.SetActive(false);
        }

        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("Crouch", isCrouched);
        anim.SetBool("OnWall", OnWall() && !isGrounded());
        anim.SetBool("Falling", isFalling);

        if (isFalling && !OnWall() && !isSlamming)
        {
            body.gravityScale = defaultGravity * 2;
        }
        else if (OnWall())
        {
            body.gravityScale = defaultGravity * 2 / 3;
        }
        else
        {
            body.gravityScale = defaultGravity;
        }

        if (isSlamming)
        {
            body.velocity = Vector2.down * slamDownForce;
        }

        // Debug.Log("Grounded: " + isGrounded() + "\nWalled: " + OnWall());

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            Jump();

        if (OnWall() && !isGrounded())
        {
            wallSlideEffect.SetActive(true);
        }
        else
        {
            wallSlideEffect.SetActive(false);
            // body.gravityScale = 1.5f;
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
        // Determine the direction of the wall
        Vector2 playerPosition = transform.position;
        RaycastHit2D hitLeft = Physics2D.Raycast(playerPosition, Vector2.left, 0.3f, wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(playerPosition, Vector2.right, 0.3f, wallLayer);

        if (hitLeft.collider != null)
        {
            // Wall is to the left, jump to the right and upwards
            StartCoroutine(PerformWallJump(Vector2.right, 1));
        }
        else if (hitRight.collider != null)
        {
            // Wall is to the right, jump to the left and upwards
            StartCoroutine(PerformWallJump(Vector2.left, -1));
        }

        jumpCounter = extraJumps - 1;
    }

    private IEnumerator PerformWallJump(Vector2 wallDirection, int direction)
    {
        // Disable gravity during the initial phase of the jump to avoid the sudden pull down
        body.gravityScale = 0;
        canMoveRight = direction == 1;
        canMoveHorizontally = false; // Prevent horizontal movement

        // Smoothly apply the wall jump force over a short period
        float jumpDuration = 0.1f; // Duration over which the jump force is applied
        float elapsedTime = 0;

        while (elapsedTime < jumpDuration)
        {
            // Apply force diagonally away from the wall
            body.velocity = new Vector2(wallDirection.x * wallJumpX, jumpForce);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Restore gravity and enable horizontal movement after a short delay
        body.gravityScale = defaultGravity;
        yield return new WaitForSeconds(horizontalCooldownTime);
        canMoveHorizontally = true;
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

        // Reduce the size of the BoxCollider2D when crouching
        boxCollider.size = new Vector2(crouchWidth, crouchHeight); // Adjust the Y size as needed
        boxCollider.offset = new Vector2(boxCollider.offset.x, offsetChangeY); // Adjust the Y offset as needed
        circleCollider.radius = crouchRadius; // Adjust the radius as needed
    }

    private bool isGrounded()
    {
        RaycastHit2D hitFloor = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        return hitFloor.collider != null;
    }

    private bool OnWall()
    {
        Vector2 playerPosition = transform.position;
        RaycastHit2D hitLeft = Physics2D.Raycast(playerPosition, Vector2.left, 0.3f, wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(playerPosition, Vector2.right, 0.3f, wallLayer);
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

    private void StandUp()
    {
        isCrouched = false;
        // Restore the original size of the BoxCollider2D when standing up
        boxCollider.size = new Vector2(boxColliderWidth, boxColliderHeight); // Restore original Y size
        boxCollider.offset = new Vector2(boxCollider.offset.x, boxColliderOffsetY); // Restore original offset
        circleCollider.radius = circleColliderRadius; // Restore original radius
    }
}
