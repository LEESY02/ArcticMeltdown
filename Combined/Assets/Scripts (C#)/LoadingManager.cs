using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            SceneManager.LoadScene(1); //Load level 2 (at index 1)
    }
}
