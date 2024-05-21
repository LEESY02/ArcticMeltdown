using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Scripting;

public class PlayerMovement : MonoBehaviour {
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

    private void Awake() {
        // Grab reference for rigidbody and animator from object;
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        gravityScale = body.gravityScale;
    }

    private void Update() { // records inputs
        horizontalInput = Input.GetAxis("Horizontal");

        // scale values in Vector3 of player scale
        float xScale = Math.Abs(transform.localScale.x);
        float yScale = Math.Abs(transform.localScale.y);
        float zScale = Math.Abs(transform.localScale.z);

        // flip player when moving left-right
        if(horizontalInput > 0f) {
            transform.localScale = new Vector3(xScale, yScale, zScale);
        } else if (horizontalInput < 0f) {
            transform.localScale = new Vector3(-xScale, yScale, zScale);
        }

        // Crouch logic
        if(Input.GetKey(KeyCode.S) && isGrounded()) {
            Crouch();
        } else {
            isCrouched = false;
        }

        // Set animator parameters
        anim.SetBool("Run", horizontalInput != 0);
        anim.SetBool("Grounded", isGrounded());
        anim.SetBool("Crouch", isCrouched);

        // Wall jump logic
        if(wallJumpCooldown > 0.2f) {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if (onWall() && !isGrounded()) {
                body.gravityScale = 0;
                body.velocity = new Vector2(body.velocity.x, -gravityScale / 2); // Simulate sliding down the wall
            } else {
                body.gravityScale = gravityScale;
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y); // Simulate sliding down the wall
            }

            if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) {
                Jump();
            }
        } else {
            wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump() {
        if(isGrounded()) {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
        } else if(onWall() && !isGrounded()) {
            if(horizontalInput == 0) {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * bounceForce * 2, jumpForce);
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            } else {
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * bounceForce, jumpForce);
            }
            wallJumpCooldown = 0;
        }
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
        // RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall() {
        RaycastHit2D raycastHitCircle = Physics2D.CircleCast(
            circleCollider.bounds.center, 
            circleCollider.radius, 
            new Vector2(transform.localScale.x, 0), 
            0.1f, 
            wallLayer);
        RaycastHit2D raycastHitBox = Physics2D.BoxCast(
            boxCollider.bounds.center, 
            boxCollider.bounds.size, 
            0, 
            new Vector2(transform.localScale.x, 0), 
            0.1f, 
            wallLayer);
        return raycastHitCircle.collider != null && raycastHitBox.collider != null;
    }

    public bool canAttack() {
        return !onWall();
    }

}
