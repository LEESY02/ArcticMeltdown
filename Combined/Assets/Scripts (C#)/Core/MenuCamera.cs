using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private Transform menuPosition;
    [SerializeField] private float durationBeforePanningUpwards;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float panningSpeed;
    [SerializeField] private GameObject[] buttonsAndPointer;

    private float timer;
    private Vector3 velocity = Vector3.zero;
    private Tracker tracker;

    private void Awake()
    {
        Time.timeScale = 1;
        timer = 0;
        tracker = FindObjectOfType<Tracker>();
        DeactivateButtons();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (LoadedBefore())
        {
            transform.position = new Vector3(menuPosition.position.x, menuPosition.position.y, transform.position.z);
            ActivateButtons();
        } 
        else if (timer > durationBeforePanningUpwards)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                new Vector3(menuPosition.position.x, menuPosition.transform.position.y, transform.position.z), 
                ref velocity, 
                panningSpeed);

            if (timer > durationBeforePanningUpwards + 3) ActivateButtons();
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
}
