using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBullet : EnemyDamage
{
    [SerializeField] private float force;
    [SerializeField] private float lifetime;
    private GameObject player;
    private Rigidbody2D rb;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;
        //gives an angle in radians then converted into degrees
        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
