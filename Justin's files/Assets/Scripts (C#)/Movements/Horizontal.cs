using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Horizontal : MonoBehaviour
{
    //[SerializeField] private float movementDistace;
    [SerializeField] private float speed;
    private bool movingLeft;

    private void Update() {
        if (!enabled) return; // Early exit if script is disabled
        if (movingLeft) {
            //if (transform.position.x > leftEdge) {
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
        } else {
            //if (transform.position.x < rightEdge) {
            transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Wall")) {
            ChangeDirection();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) { 
            ChangeDirection();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Wall"))
        {
            if (!IsNextCollisionGround()) {
                // Only change direction if the enemy is no longer in contact with the wall
                transform.position = new Vector3(
                transform.position.x,
                transform.position.y + 0.02f,
                transform.position.z);
                ChangeDirection();
            }
        }
    }

    private void ChangeDirection()
    { 
        movingLeft = !movingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip the enemy horizontally
        transform.localScale = scale;
    }

    private bool IsNextCollisionGround()
    {
        // Define a raycast direction based on the character's movement direction
        Vector2 raycastDirection = movingLeft ? Vector2.left : Vector2.right;

        // Cast a ray to check if there's ground ahead
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, 0.5f); // Adjust distance as needed

        // If the ray hits something and it's tagged as ground, return true
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            return true;
        }
        return false;
    }
}
