using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] snowballs;

    [Header ("Audio")]
    [SerializeField] private AudioClip attackSound;
    private Animator anim;
    private Player playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake() {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Player>();
    }

    private void Update() {
        if (playerMovement.CanAttack() && Input.GetMouseButton(0) && cooldownTimer > attackCooldown) { // left click to attack
            Attack();
        }
        cooldownTimer -= Time.deltaTime;
    }

    private void Attack() {
        SoundManager.instance.PlaySound(attackSound);
        anim.SetTrigger("Attack");
        cooldownTimer = 0;
        //pool Snowball
        snowballs[FindSnowball()].transform.position = firePoint.position;
        snowballs[FindSnowball()].GetComponent<Snowball>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindSnowball() {
        for (int i = 0; i < snowballs.Length; i++) {
            if (!snowballs[i].activeInHierarchy) {
                return i;
            }
        }
        return 0;
    }    
}
