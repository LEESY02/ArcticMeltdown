using System;
using UnityEngine;

public class FlyingEnemy : MeleeEnemy
{
    [Header("Movement")]
    public float speed;
    public float detectionRange;
    public float detectionOffsetDownwards;
    public float fallingGravity;

    private bool chase;
    private bool grounded;
    private GameObject player;
    private Vector3 startingPosition;
    private Animator animator;
    private float distance;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    // Awake is called when the script instance is being loaded
    protected override void Awake()
    {
        base.Awake(); // Call MeleeEnemy Awake method
        Debug.Log("Awake called");
        InitializeComponents();
    }

    // OnEnable is called when the object becomes enabled and active
    private void OnEnable()
    {
        Debug.Log("OnEnable called");
        ResetState();
    }

    // Initialize components and variables
    private void InitializeComponents()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startingPosition = transform.position;
        animator = base.GetAnim(); // GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Reset the enemy state
    private void ResetState()
    {
        chase = false;
        grounded = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        boxCollider.enabled = true;
        if (animator != null)
        {
            animator.ResetTrigger("Dead");
            animator.SetBool("Grounded", false);
            animator.SetBool("Moving", false);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); // Call MeleeEnemy Update method

        if (IsDead())
        {
            HandleDeath();
            return;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            animator.ResetTrigger("Dead");
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        FaceTarget(player.transform.position);

        chase = distance < detectionRange;

        if (player == null)
            return;

        if (chase && animator.GetBool("Moving") && player.GetComponent<Health>().currentHealth > 0)
        {
            Chase(); // chase player
        }
        else if ((!chase && animator.GetBool("Moving")) || player.GetComponent<Health>().currentHealth < 1)
        {
            ReturnToStartPoint(); // go to starting position
            FaceTarget(startingPosition);
        }

        if (transform.position == startingPosition) animator.Play("Idle");
    }

    private void HandleDeath()
    {
        if (!grounded)
        {
            boxCollider.enabled = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = fallingGravity;
            animator.SetTrigger("Dead");
        }
    }

    private void Chase()
    {
        Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y - detectionOffsetDownwards, player.transform.position.z);
        transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
    }

    private void ReturnToStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);
    }

    private void FaceTarget(Vector3 target)
    {
        float direction = target.x - transform.position.x;
        if (direction < 0 && transform.localScale.x > 0 || direction > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDead() && collision.gameObject.CompareTag("Ground"))
        {
            boxCollider.enabled = false;
            rb.gravityScale = 0;
            grounded = true;
            animator.SetBool("Grounded", grounded);
            this.enabled = false;
        }
    }

    private bool IsDead()
    {
        return gameObject.GetComponent<Health>().currentHealth < 1;
    }
}
