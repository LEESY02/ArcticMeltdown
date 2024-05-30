using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Horizontal : MonoBehaviour
{
    //[SerializeField] private float movementDistace;
    [SerializeField] private float speed;
    private bool movingLeft;
    // private float leftEdge;
    // private float rightEdge;

    // private void Awake() {
    //     leftEdge = transform.position.x - movementDistace;
    //     rightEdge = transform.position.x + movementDistace;
    // }

    private void Update() {
        if (movingLeft) {
            //if (transform.position.x > leftEdge) {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            // } else {
            //     movingLeft = false;
            // }
        } else {
            //if (transform.position.x < rightEdge) {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            // } else {
            //     movingLeft = true;
            // }
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
            // Only change direction if the enemy is no longer in contact with the wall
            transform.position =new Vector3(
                movingLeft ? transform.position.x + 0.1f : transform.position.x - 0.1f,
                transform.position.y + 0.1f,
                transform.position.z);
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    { 
        movingLeft = !movingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Flip the enemy horizontally
        transform.localScale = scale;
    }
}
