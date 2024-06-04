using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {get; private set;}
    private AudioSource soundSource;
    private AudioSource musicSource;

    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        //Keep this object even when we go to new scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Check if the current scene is the menu scene
            if (SceneManager.GetActiveScene().name == "_MainMenu")
            {
                // Destroy the SoundManager object if in the menu scene
                Destroy(gameObject);
            }
        }
        //Destroy duplicate gameobjects
        else if (instance != null && instance != this)
            Destroy(gameObject);

        //Assign initial volumes
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }

    public void PlaySound(AudioClip sound, float volume)
    {
        soundSource.PlayOneShot(sound, volume);
    }

    public void ChangeSoundVolume(float change)
    {
        ChangeSourceVolume("soundVolume", change, soundSource);
    }

    public void ChangeMusicVolume(float change)
    {
        ChangeSourceVolume("musicVolume", change, musicSource);
    }

    private void ChangeSourceVolume(string volumeName, float change, AudioSource source)
    {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume += change;

        //check if we reached the maxinmum or minimum value
        if (currentVolume > 1)
        {
            currentVolume = 0;
        }
        else if (currentVolume < 0)
        {
            currentVolume = 1;
        }

        //assign final value
        source.volume = currentVolume;

        //save final value to player prefs
        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
}
