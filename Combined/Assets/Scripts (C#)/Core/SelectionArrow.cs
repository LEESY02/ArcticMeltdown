using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private float changeSoundVolume;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private float interactSoundVolume;
    private RectTransform rect;
    private int currentPosition;

    private void Awake() 
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //change position if the selection arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1); //move pointer up
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1); //move pointer down

        //interact with the options
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            Interact();
        }
    }

    private void ChangePosition(int change)
    {
        currentPosition += change;

        if (change != 0)
            SoundManager.instance.PlaySound(changeSound, changeSoundVolume);

        if (currentPosition < 0)
            currentPosition = options.Length - 1;
        else if (currentPosition > options.Length - 1)
            currentPosition = 0;

        //Assign the Y position of the current option to the arrow
        rect.position = new Vector3(
            rect.position.x,
            options[currentPosition].position.y,
            0);
    }

    private void Interact()
    {
        SoundManager.instance.PlaySound(interactSound, interactSoundVolume);

        //Access the button component on each option and call it's function
        options[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
