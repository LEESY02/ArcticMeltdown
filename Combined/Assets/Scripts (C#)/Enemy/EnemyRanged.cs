using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPosition;
    public float range;
    public float attackCooldown;
    
    private GameObject player;
    private float timer;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log("Distance of player: " + distance);
        FacePlayer();

        if (distance < range)
        {
            timer += Time.deltaTime;
            if (timer > attackCooldown)
            {
                timer = 0;
                anim.SetTrigger("RangedAttack");
            }
        }
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
}
