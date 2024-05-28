using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertical : MonoBehaviour
{
    //[SerializeField] private float movementDistace;
    [SerializeField] private float upSpeed;
    [SerializeField] private float downSpeed;
    private bool movingUp;
    private float topEdge;
    private float bottomEdge;

    // private void Awake() {
    //     topEdge = transform.position.x - movementDistace;
    //     bottomEdge = transform.position.x + movementDistace;
    // }

    private void Update() {
        if (movingUp) {
            //if (transform.position.x > topEdge) {
                transform.position = new Vector3(transform.position.x, transform.position.y + upSpeed * Time.deltaTime, transform.position.z);
            // } else {
            //     movingUp = false;
            // }
        } else {
            //if (transform.position.x < bottomEdge) {
                transform.position = new Vector3(transform.position.x, transform.position.y - downSpeed * Time.deltaTime, transform.position.z);
            // } else {
            //     movingUp = true;
            // }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Ground") movingUp = !movingUp; // change direction only if collision with non-player
    }
}
