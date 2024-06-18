using System.Collections;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject imageToDescend;
    [SerializeField] private float descendDuration; // Duration to reach the target position
    [SerializeField] private Transform originalPosition;
    [SerializeField] private Transform targetPosition;

    private void OnDisable()
    {
        imageToDescend.transform.position = originalPosition.position;
    }

    private void OnEnable()
    {
        // Reset image position in case it was disabled elsewhere
        imageToDescend.transform.position = originalPosition.position;

        // Start the descent coroutine
        StartCoroutine(DescendImage());
    }

    private IEnumerator DescendImage()
    {
        Vector3 startPosition = originalPosition.position;
        Vector3 endPosition = targetPosition.position;
        float elapsedTime = 0;

        while (elapsedTime < descendDuration)
        {
            imageToDescend.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / descendDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the image is exactly at the target position at the end
        imageToDescend.transform.position = endPosition;
    }
}
