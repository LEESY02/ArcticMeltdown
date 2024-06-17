using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject imageToDecend;
    [SerializeField] private float decendDistance;
    [SerializeField] private float decendSpeed;
    [SerializeField] private Transform originalPosition;

    private void OnDisable()
    {
        imageToDecend.transform.position = originalPosition.position;
    }

    private void OnEnable()
    {
        // Reset image position in case it was disabled elsewhere
        imageToDecend.transform.position = originalPosition.position;

        // Start the descent coroutine
        StartCoroutine(DescendImage());
    }

    private IEnumerator DescendImage() 
    {
        // Calculate the final position based on descent distance
        Vector3 targetPosition = originalPosition.position + Vector3.down * decendDistance;

        // Move the image towards the target position over time
        while (imageToDecend.transform.position != targetPosition)
        {
            imageToDecend.transform.position = Vector3.MoveTowards(imageToDecend.transform.position, targetPosition, decendSpeed * Time.deltaTime);
            yield return null;
        }
    }

}
