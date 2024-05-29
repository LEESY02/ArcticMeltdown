using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy_confirm : MonoBehaviour
{
    // List containing different enemies (in terms of types)
    public GameObject[] enemy_patrol;
    public GameObject[] enemy_range; 
    public GameObject[] enemy_flying;

    // Spawn locations {Patrol, Range, Flying}
    public GameObject[] spawn_locations;

    // Start is called before the first frame update
    void Start()
    {
        int type = Random.Range(0, 3); // Random type of enemy

        switch (type) // Get spawn location and spawn chosen type
        {
            case 0:
                int patrol_type = Random.Range(0, enemy_patrol.Length);
                transform.position = spawn_locations[type].transform.position;
                Instantiate(enemy_patrol[patrol_type], transform.position, Quaternion.identity);
                break;
            case 1:
                int range_type = Random.Range(0, enemy_range.Length);
                transform.position = spawn_locations[type].transform.position;
                Instantiate(enemy_patrol[range_type], transform.position, Quaternion.identity);
                break;
            case 2:
                int flying_type = Random.Range(0, enemy_range.Length);
                transform.position = spawn_locations[type].transform.position;
                Instantiate(enemy_patrol[flying_type], transform.position, Quaternion.identity);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
