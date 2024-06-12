using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private int index;
    
    private void Update()
    {
        index = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.CompareTag("Player");
        if (IsLastLevel()) // last page
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(index + 1);
        }
    }

    private bool IsLastLevel()
    {
        return index == SceneManager.sceneCountInBuildSettings - 1;
    }
}
