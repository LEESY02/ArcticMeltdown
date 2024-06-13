using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private int index;
    private Tracker tracker;
    
    private void Update()
    {
        index = SceneManager.GetActiveScene().buildIndex;
        tracker = FindObjectOfType<Tracker>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tracker.coinCount = collision.gameObject.GetComponent<Player>().coinCount;
            tracker.mostRecentHealth = collision.gameObject.GetComponent<Health>().currentHealth;
            if (IsLastLevel()) // last page
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(index + 1);
            }
        }
    }

    private bool IsLastLevel()
    {
        return index == SceneManager.sceneCountInBuildSettings - 1;
    }
}
