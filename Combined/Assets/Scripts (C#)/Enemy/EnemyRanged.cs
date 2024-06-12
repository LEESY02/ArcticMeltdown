using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyRanged : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPosition;
    public float range;
    public float attackCooldown;
    
    private GameObject player;
    private float timer;
    private Animator anim;
    private bool hasPlayedAlertAnimation;
    private bool isPlayerInRange;
    private CameraController cam;
    private GameObject[] rooms;
    private Transform currentRoom;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        cam = FindObjectOfType<CameraController>();
        rooms = cam.rooms;

        for (int i = 0; i < rooms.Length; i++)
        {
            if ((transform.position.x < rooms[i].transform.position.x + 15 && transform.position.x > rooms[i].transform.position.x - 15)
             && (transform.position.y < rooms[i].transform.position.y + 10 && transform.position.y > rooms[i].transform.position.y - 10))
            {
                currentRoom = rooms[i].transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        // Debug.Log("Distance of player: " + distance);
        FacePlayer();

        if (distance < range && PlayerInSameRoom())
        {
            isPlayerInRange = true;

            if (!hasPlayedAlertAnimation) // Check if alert animation has already been played
            {
                Alerted();
                hasPlayedAlertAnimation = true; // Set the flag to true after playing alert animation
            }

            timer += Time.deltaTime;
            if (timer > attackCooldown)
            {
                timer = 0;
                anim.SetTrigger("RangedAttack");
            }
        }
        else
        {
            isPlayerInRange = false;
            hasPlayedAlertAnimation = false; // Reset the flag when player exits the range
        }

        anim.SetBool("isInRange", isPlayerInRange); // Update the animator parameter
    }

    private void Shoot()
    {
        Instantiate(bullet, bulletPosition.position, Quaternion.identity);
    }

    private void FacePlayer()
    {
        float direction = player.transform.position.x - transform.position.x;
        if (direction < 0 && transform.localScale.x > 0 || direction > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z);
        }
    }

    private void Alerted()
    {
        anim.SetTrigger("Alerted");
    }

    private GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
            GameObject result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    private bool PlayerInSameRoom()
    {
        Transform playerPos = player.transform;
        return (playerPos.position.x < currentRoom.position.x + 15 && playerPos.position.x > currentRoom.position.x - 15)
             && (playerPos.position.y < currentRoom.position.y + 10 && playerPos.position.y > currentRoom.position.y - 10);
    }
}

