using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private float volume;

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
            Player playerMovement = collision.gameObject.GetComponent<Player>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }
            collision.GetComponent<Rigidbody2D>().velocity *= 0.2f;
            SoundManager.instance.PlaySound(teleportSound, volume);
            StartCoroutine(WaitForEndOfSound());
        }
    }

    private bool IsLastLevel()
    {
        return index == SceneManager.sceneCountInBuildSettings - 1;
    }

    private IEnumerator WaitForEndOfSound()
    {
        yield return new WaitForSeconds(teleportSound.length);
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
