using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
            movingLeft = !movingLeft;
            gameObject.transform.localScale = new Vector3(
                -gameObject.transform.localScale.x,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z); 
        }
    }
}
