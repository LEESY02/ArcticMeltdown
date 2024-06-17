using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{

    public static Tracker instance {get; private set;}
    [Header("Player Stats")]
    public int coinCount;
    public float playerStartingHealth;
    public float enemyStartingHealth;
    public float mostRecentHealth;
    [Header("Movement")]
    public int extraJumps;
    public int jumpForce;
    public float speed;
    [Header("Attack")]
    public float attackCooldown;
    public float snowballSpeed;
    [Header("Menu Load Count")]
    public int loadCount;
    

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        coinCount = 0;
        loadCount = 0;
        mostRecentHealth = playerStartingHealth;
        DontDestroyOnLoad(gameObject);
    }
}
