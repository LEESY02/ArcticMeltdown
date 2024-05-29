using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHeart : MonoBehaviour
{
    public GameObject Heart;
    public GameObject Coin;

    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            // Debug.Log("inside");
            int h_or_c = Random.Range(0, 2);
            if (h_or_c == 0)
            {
                Instantiate(Heart, transform.position, Quaternion.identity);
            } else
            {
                Instantiate(Coin, transform.position, Quaternion.identity);
            }
        }
    }
    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
