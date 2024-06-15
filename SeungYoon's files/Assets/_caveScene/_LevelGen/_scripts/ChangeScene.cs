using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    void Enter(Collider other)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Cavescene 2":
                SceneManager.LoadScene("Snowscene 2");
                break;
            case "Snowscene 2":
                SceneManager.LoadScene("Skyscene 2");
                break;
            case "Skyscene 2":
                SceneManager.LoadScene("Cavescene 2");
                break;
        }
    }
}
