using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private Transform menuPosition;
    [SerializeField] private float durationBeforePanningUpwards;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float panningSpeed;
    [SerializeField] private GameObject[] buttonsAndPointer;

    [Header("Audio")]
    [SerializeField] private AudioClip startSound;
    [SerializeField] private float startSoundVolume;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private float menuMusicVolume;

    [Header("Camera Shake")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;

    private float timer;
    private Vector3 velocity = Vector3.zero;
    private Tracker tracker;

    private AudioSource startSoundSource;
    private AudioSource menuMusicSource;

    private bool menuMusicPlaying = false;

    private void Awake()
    {
        Time.timeScale = 1;
        timer = 0;
        tracker = FindObjectOfType<Tracker>();
        DeactivateButtons();
        InitializeAudioSources();
        StartCoroutine(PlayStartAndMovingUpSounds());
    }

    private void InitializeAudioSources()
    {
        startSoundSource = gameObject.AddComponent<AudioSource>();
        startSoundSource.clip = startSound;
        startSoundSource.volume = startSoundVolume;

        menuMusicSource = gameObject.AddComponent<AudioSource>();
        menuMusicSource.clip = menuMusic;
        menuMusicSource.volume = menuMusicVolume;
        menuMusicSource.loop = true;
    }

    private IEnumerator PlayStartAndMovingUpSounds()
    {
        if (!LoadedBefore())
        {
            startSoundSource.Play();
            StartCoroutine(ShakeCamera(shakeDuration, shakeMagnitude));
            yield return new WaitForSeconds(startSound.length);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (LoadedBefore())
        {
            transform.position = new Vector3(menuPosition.position.x, menuPosition.position.y, transform.position.z);
            ActivateButtons();
            PlayMenuMusic();
        }
        else if (timer > durationBeforePanningUpwards)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                new Vector3(menuPosition.position.x, menuPosition.transform.position.y, transform.position.z),
                ref velocity,
                panningSpeed);

            if (timer > durationBeforePanningUpwards + 3)
            {
                ActivateButtons();
                PlayMenuMusic();
            }
        }
    }

    private void PlayMenuMusic()
    {
        if (!menuMusicPlaying)
        {
            menuMusicSource.Play();
            menuMusicPlaying = true;
        }
    }

    private bool LoadedBefore()
    {
        return tracker.loadCount > 0;
    }

    private void ActivateButtons()
    {
        for (int i = 0; i < buttonsAndPointer.Length; i++)
        {
            buttonsAndPointer[i].SetActive(true);
        }
        FadeInButtons();
    }

    private void DeactivateButtons()
    {
        for (int i = 0; i < buttonsAndPointer.Length; i++)
        {
            buttonsAndPointer[i].SetActive(false);
        }
    }

    private void FadeInButtons()
    {
        for (int i = 0; i < buttonsAndPointer.Length; i++)
        {
            GameObject button = buttonsAndPointer[i];

            Text buttonText = button.GetComponent<Text>();
            if (buttonText != null)
            {
                StartCoroutine(FadeInText(buttonText));
            }
            else
            {
                Image buttonImage = button.GetComponent<Image>();
                if (buttonImage != null)
                {
                    StartCoroutine(FadeInImage(buttonImage));
                }
            }
        }
    }

    private IEnumerator FadeInText(Text text)
    {
        Color originalColor = text.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            text.color = Color.Lerp(originalColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        text.color = targetColor; // Ensure the text color is fully opaque at the end
    }

    private IEnumerator FadeInImage(Image image)
    {
        Color originalColor = image.color;
        Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            image.color = Color.Lerp(originalColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        image.color = targetColor; // Ensure the image color is fully opaque at the end
    }

    private IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
