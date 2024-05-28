using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    private float cooldownTimer;

    private void Update() {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown) Attack();
    }

    private void Attack() {
        cooldownTimer = 0;
        int fireballIndex = FindFireball();
        if (fireballIndex != -1) {
            //fireballs[fireballIndex].transform.position = firePoint.position;
            fireballs[fireballIndex].GetComponent<EnemyProjectile>().ActivateProjectile(firePoint.position);
        }
    }

    private int FindFireball() {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy) return i;
        }
        return -1;
    }    
}
