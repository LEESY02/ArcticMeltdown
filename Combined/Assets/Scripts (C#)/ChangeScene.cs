using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("INSIDE PORTAL");
        other.CompareTag("PLAYER");
        if (other.tag == "Player") {
            string sceneName = SceneManager.GetActiveScene().name;

            Debug.Log("INSIDE IF STATEMENT");
            Debug.Log(sceneName);

            switch (sceneName)
            {
                case "Level1 (SY)":
                    Debug.Log("ONE");
                    SceneManager.LoadScene("Level2 (SY)");
                    break;
                case "Level2 (SY)":
                    Debug.Log("TWO");
                    SceneManager.LoadScene("Level3 (SY)");
                    break;
                case "Level3 (SY)":
                    Debug.Log("THREE");
                    SceneManager.LoadScene("Level1 (SY)");
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
        {
        
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
