using System;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float bounceForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private bool isCrouched;
    private BoxCollider2D boxCollider;
    private CircleCollider2D circleCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private float gravityScale;
    private int jumpCount;
    private bool isFalling; // track if the player is falling
    private bool onwall;

    private void Awake() {
        // Grab reference for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        gravityScale = body.gravityScale;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if (isGrounded()) jumpCount = 0;

        // Update isFalling based on the player's vertical velocity and ground status
        isFalling = body.velocity.y < 0 && !isGrounded();

        // scale values in Vector3 of player scale
        float xScale = Mathf.Abs(transform.localScale.x);
        float yScale = Mathf.Abs(transform.localScale.y);
        float zScale = Mathf.Abs(transform.localScale.z);

        // flip player when moving left-right
        if (horizontalInput > 0f)
        {
            transform.localScale = new Vector3(xScale, yScale, zScale);
        }
        else if (horizontalInput < 0f)
        {
            transform.localScale = new Vector3(-xScale, yScale, zScale);
        }

        // Crouch logic
        if (Input.GetKey(KeyCode.S) && isGrounded())
        {
            Crouch();
        }
        else
        {
            isCrouched = false;
        }

        // Set animator parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("Crouch", isCrouched);
        anim.SetBool("Falling", isFalling); // Update the animator with the falling status

        // Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            if (OnWall() && !isGrounded())
            {
                // Reduce vertical velocity to make the player slide off walls
                body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.8f);

                // Prevent horizontal movement when on a wall
                horizontalInput = 0f;

                // Reset horizontal velocity to prevent sticking to the wall
                body.velocity = new Vector2(0, body.velocity.y);
            }

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            body.gravityScale = gravityScale;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            {
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            // Regular jump if grounded
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
            jumpCount++;
        }
        else if (OnWall())
        {
            // Wall jump if on a wall
            float wallSide = Mathf.Sign(transform.position.x - GetNearestWallPosition().x); // Determine which side of the wall the player is on
            body.velocity = new Vector2(wallSide * speed, jumpForce);
            anim.SetTrigger("Jump");
            jumpCount++;
            wallJumpCooldown = 0; // Reset wall jump cooldown
        }
        else if (jumpCount < 2)
        {
            // Double jump if not grounded and not on a wall
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
            jumpCount++;
        }
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
        // RaycastHit2D raycastHitCircle = Physics2D.CircleCast(
        //     circleCollider.bounds.center, 
        //     circleCollider.radius, 
        //     new Vector2(transform.localScale.x, 0), 
        //     0.1f, 
        //     wallLayer);
        // RaycastHit2D raycastHitBox = Physics2D.BoxCast(
        //     boxCollider.bounds.center, 
        //     boxCollider.bounds.size, 
        //     0, 
        //     new Vector2(transform.localScale.x, 0), 
        //     0.1f, 
        //     wallLayer);
        // return raycastHitCircle.collider != null || raycastHitBox.collider != null;
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
