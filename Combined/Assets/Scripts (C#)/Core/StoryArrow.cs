using UnityEngine;
using UnityEngine.UI;

public class StoryArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private float changeSoundVolume;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private float interactSoundVolume;
    [SerializeField] private float offset;
    private RectTransform rect;
    private int currentPosition;

    private void Awake() 
    {
        rect = GetComponent<RectTransform>();
        currentPosition = 0; // start at the main menu button
        UpdatePosition();
    }

    private void Update()
    {
        // Change position to MainMenu button from navigation buttons using up and down keys
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentPosition == 1 || currentPosition == 2) // if on navigation buttons
            {
                currentPosition = 0; // move to MainMenu button
                UpdatePosition();
                SoundManager.instance.PlaySound(changeSound, changeSoundVolume);
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentPosition == 0) // if on MainMenu button
            {
                currentPosition = 1; // move to first navigation button
                UpdatePosition();
                SoundManager.instance.PlaySound(changeSound, changeSoundVolume);
            }
        }

        // Switch between previous and next buttons using left and right keys
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentPosition == 1 || currentPosition == 2) // if on navigation buttons
            {
                int change = (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) ? 1 : -1;
                ChangePosition(change);
            }
        }

        // Interact with the options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            Interact();
        }
    }

    private void ChangePosition(int change)
    {
        currentPosition += change;

        if (change != 0)
            SoundManager.instance.PlaySound(changeSound, changeSoundVolume);

        if (currentPosition < 1)
            currentPosition = 2;
        else if (currentPosition > 2)
            currentPosition = 1;

        // Assign the position of the current option to the arrow
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        rect.position = new Vector3(
            options[currentPosition].position.x - options[currentPosition].rect.width * options[currentPosition].localScale.x / 2 - offset,
            options[currentPosition].position.y,
            0);
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound, interactSoundVolume);

        // Access the button component on each option and call its function
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
