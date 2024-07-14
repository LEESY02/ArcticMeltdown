using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip teleportSound;
    [SerializeField] private float teleportVolume;
    [SerializeField] private AudioClip notEnoughKillsSound;
    [SerializeField] private float notEnoughKillsVolume;

    [Header("Kills Tracker")]
    [SerializeField] private GameObject textbox;
    [SerializeField] private float durationOfText;
    public int killsRequired1;
    public int killsRequired2;
    public int killsRequired3;

    private int killsRequired;
    public int currentKills;

    private int index;
    private Tracker tracker;
    private Player player;

    private String notEnoughText;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        killsRequired = SceneManager.GetActiveScene().buildIndex == 2
                        ? killsRequired1
                        : SceneManager.GetActiveScene().buildIndex == 3
                            ? killsRequired2
                            : killsRequired3;
    }
    
    private void Update()
    {
        index = SceneManager.GetActiveScene().buildIndex;
        tracker = FindObjectOfType<Tracker>();
        currentKills = player.killCount;
        Debug.Log(currentKills);
        UpdateText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (IsEnufKills())
            {
                tracker.coinCount = collision.gameObject.GetComponent<Player>().coinCount;
                tracker.mostRecentHealth = collision.gameObject.GetComponent<Health>().currentHealth;
                Player playerMovement = collision.gameObject.GetComponent<Player>();
                if (playerMovement != null)
                {
                    playerMovement.enabled = false;
                }
                collision.GetComponent<Rigidbody2D>().velocity *= 0.2f;
                SoundManager.instance.PlaySound(teleportSound, teleportVolume);
                StartCoroutine(WaitForEndOfSound());
            }
            else
            {
                NotEnoughKills();
            }
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

    private bool IsEnufKills()
    {
        return currentKills >= killsRequired;
    }

    private void NotEnoughKills()
    {
        SoundManager.instance.PlaySound(notEnoughKillsSound, notEnoughKillsVolume);
        // instantiate a text to tell the player that he needs more kills
        StartCoroutine(ActivateThenDeactivateText());
    }

    private IEnumerator ActivateThenDeactivateText()
    {
        textbox.SetActive(true);
        yield return new WaitForSeconds(durationOfText);
        textbox.SetActive(false);
    }

    private void UpdateText()
    {
        if (!IsEnufKills()) // if not enough kills
        {
            notEnoughText = $"{currentKills}/{killsRequired} kills\n{killsRequired - currentKills} more kills required!";
        }
        textbox.GetComponent<TextMeshPro>().text = notEnoughText;
    }
}