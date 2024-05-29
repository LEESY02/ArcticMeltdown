using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] enemy;
    // Start is called before the first frame update
    void Start()
    {
        int type = Random.Range(0, enemy.Length);
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            Debug.Log("inside");
            Instantiate(enemy[type], transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
