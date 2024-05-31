using System;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //horizontal wall jump force
    [SerializeField] private float wallJumpY; //vertical wall jump force

    [Header ("Audio")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float jumpVolume;

    private Rigidbody2D body;
    private Animator anim;
    private bool isCrouched;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private bool isFalling; // track if the player is falling
    private bool onwall;

    private void Awake() {
        // Grab reference for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Update isFalling based on the player's vertical velocity and ground status
        isFalling = body.velocity.y < 0 && !isGrounded();

        // scale values in Vector3 of player scale
        float xScale = Mathf.Abs(transform.localScale.x);
        float yScale = Mathf.Abs(transform.localScale.y);
        float zScale = Mathf.Abs(transform.localScale.z);

        // flip player when moving left-right
        if (horizontalInput > 0f)
            transform.localScale = new Vector3(xScale, yScale, zScale);
        else if (horizontalInput < 0f)
            transform.localScale = new Vector3(-xScale, yScale, zScale);

        // Crouch logic
        if (Input.GetKey(KeyCode.S) && isGrounded())
            Crouch();
        else
            isCrouched = false;

        // Set animator parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("Crouch", isCrouched);
        anim.SetBool("Falling", isFalling); // Update the animator with the falling status

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            Jump();

        //Adjustable jump height
        if ((Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0) || (Input.GetKeyUp(KeyCode.W) && body.velocity.y > 0))
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (OnWall())
        {
            body.gravityScale = 0.8f;
            // Allow horizontal movement while on the wall
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }
        else {
            body.gravityScale = 1.5f;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if (isGrounded() || OnWall()) {
                coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
                jumpCounter = extraJumps; //Reset jump counter to extra jump value
            } else {
                coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
            }
        }
    }

    private void Jump()
    {
        //if coyote couter is 0 or less and not on the wall and dont have any extra jumps, don't do anything
        if (coyoteCounter <= 0 && !OnWall() && jumpCounter <= 0) return;
        SoundManager.instance.PlaySound(jumpSound, jumpVolume);

        if (OnWall()) {
            WallJump();
        } else {
            if (!isGrounded())
            {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
                jumpCounter--;
            }
            else
            {
                if (isGrounded() || coyoteCounter > 0) {
                    body.velocity = new Vector2(body.velocity.x, jumpForce);
                    coyoteCounter = 0; // Reset coyote counter to avoid double jumps
                } else if (jumpCounter > 0) {
                    body.velocity = new Vector2(body.velocity.x, jumpForce);
                    jumpCounter--;
                }
            }

            //Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }


    private Vector2 GetNearestWallPosition()
    {
        Vector2 playerPosition = transform.position;
        RaycastHit2D hitLeft = Physics2D.Raycast(playerPosition, Vector2.left, Mathf.Infinity, wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(playerPosition, Vector2.right, Mathf.Infinity, wallLayer);

        Vector2 nearestWallPosition = Vector2.zero;

        // Check which direction has a wall and find the nearest position
        if (hitLeft.collider != null && hitRight.collider != null)
        {
            nearestWallPosition = hitLeft.distance < hitRight.distance ? hitLeft.point : hitRight.point;
        }
        else if (hitLeft.collider != null)
        {
            nearestWallPosition = hitLeft.point;
        }
        else if (hitRight.collider != null)
        {
            nearestWallPosition = hitRight.point;
        }

        return nearestWallPosition;
    }

    private void Crouch() {
        isCrouched = true;
    }

    private bool isGrounded() {
        RaycastHit2D raycastHit = Physics2D.CircleCast(
            circleCollider.bounds.center, 
            circleCollider.radius, 
            Vector2.down, 
            0.1f, 
            groundLayer);
        return raycastHit.collider != null;
    }

    private bool OnWall() {
        return onwall;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            onwall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            onwall = false;
        }
    }

    public bool CanAttack() {
        return !OnWall();
    }
}
