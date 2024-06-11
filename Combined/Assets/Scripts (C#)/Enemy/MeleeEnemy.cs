using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header ("Player Parameters")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    
    //references
    private Animator anim;
    private Health playerHealth;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Update() 
    {
        cooldownTimer += Time.deltaTime;
        if (PlayerInSight()) {
            if (gameObject.GetComponent<Horizontal>() != null) //check if is patrolling
            {
                gameObject.GetComponent<Horizontal>().enabled = false; // stop enemy from moving
            }
            if (cooldownTimer >= attackCooldown)
            {
                //attack
                cooldownTimer = 0;
                anim.SetTrigger("MeleeAttack");
                anim.SetBool("Moving", false);
            }
        } else {
            if (gameObject.GetComponent<Horizontal>() != null) //check if is patrolling
            {
                gameObject.GetComponent<Horizontal>().enabled = true; // continue moving
                anim.SetBool("Moving", true);
            }
        }
        
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        //damage player if still in range
        if (PlayerInSight())
        {
            playerHealth.TakeDamage(damage);
        }
    }
}
