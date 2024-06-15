using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{

    public static Tracker instance {get; private set;}
    public int coinCount;
    public float playerStartingHealth;
    public float enemyStartingHealth;
    public float mostRecentHealth;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        coinCount = 0;
        mostRecentHealth = playerStartingHealth;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}