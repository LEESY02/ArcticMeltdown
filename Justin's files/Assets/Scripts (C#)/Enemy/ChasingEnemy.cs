using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : EnemyDamage
{
    [Header("Enemy Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;

    [Header("Game Attributes")]
    [SerializeField] private GameObject cameraController;
    [SerializeField] private GameObject room;

    private float checkTimer;
    private Vector3 destination;
    private Vector3[] directions = new Vector3[4];
    private bool attacking;

    //Gets called everytime the monster object is activated
    private void OnEnable()
    {
        Stop();
    }

    private void Update() 
    {
        if (cameraController.GetComponent<CameraController>().GetRoom().Equals(room)) {
            gameObject.SetActive(true); // activate monster if Player is in the same room
        } else {
            gameObject.SetActive(false); // deativate monster if Player is not in the same room
        }

        if (attacking) {
            transform.Translate(destination * Time.deltaTime * speed);
        } else {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay) CheckForPlayer();
        }

        // scale values in Vector3 of player scale
        float xScale = Mathf.Abs(transform.localScale.x);
        float yScale = Mathf.Abs(transform.localScale.y);
        float zScale = Mathf.Abs(transform.localScale.z);

        if (destination == directions[1]) //check if monster is gg right
        {
            transform.localScale = new Vector3(xScale, yScale, zScale);
        }
        else if (destination == directions[0]) //check if monster is gg left
        {
            transform.localScale = new Vector3(-xScale, yScale, zScale);
        }
    }
        
    private void CheckForPlayer() 
    {
        CalculateDirections();

        //Check if monster sees the player
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null) 
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }

    }

    private void CalculateDirections() 
    {
        directions[0] = transform.right * range; //Right direction
        directions[1] = -transform.right * range; //Left direction
        directions[2] = transform.up * range; //Up direction
        directions[3] = -transform.up * range; //Down direction
    }

    private void Stop()
    {
        destination = gameObject.transform.position; //Set destination as current position so it doesn't move
        attacking = false;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Stop(); //Stop monster once he hits something
    }
}
