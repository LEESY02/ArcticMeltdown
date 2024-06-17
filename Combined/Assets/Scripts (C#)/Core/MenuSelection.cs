using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelection : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private float changeSoundVolume;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private float interactSoundVolume;
    private UIManager ui;
    private Transform snowball;
    private int currentPosition;

    // Start is called before the first frame update
    void Start()
    {
        snowball = GetComponent<Transform>();
        ui = FindFirstObjectByType<UIManager>();
    }

    private void Update()
    {
        //change position if the selection arrow
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1); //move pointer up
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1); //move pointer down

        //interact with the options
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) {
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
        snowball.position = new Vector3(
            snowball.position.x,
            options[currentPosition].position.y,
            0);
    }

    private void Interact()
    {
        if (currentPosition == 0)
        {
            ui.Restart();
        }
        else if (currentPosition == 1)
        {
            ui.Story();
        }
        else if (currentPosition == 2)
        {
            ui.Quit();
        }
    }
}
