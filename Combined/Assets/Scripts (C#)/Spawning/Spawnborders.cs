using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawnborders : MonoBehaviour
{
    public GameObject[] caveTiles;
    public GameObject[] groundTiles;
    public GameObject[] snowTiles;

    // Start is called before the first frame update
    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Level1":
                int rand1 = Random.Range(0, caveTiles.Length);
                Instantiate(caveTiles[rand1], transform.position, Quaternion.identity);
                break;
            case "Level2":
                int rand2 = Random.Range(0, groundTiles.Length);
                Instantiate(groundTiles[rand2], transform.position, Quaternion.identity);
                break;
            case "Level3":
                int rand3 = Random.Range(0, snowTiles.Length);
                Instantiate(snowTiles[rand3], transform.position, Quaternion.identity);
                break;
        }
    }

    /*
        // Update is called once per frame
        void Update()
        {

        }
    */
}
