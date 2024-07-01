using UnityEngine;
using UnityEngine.UI;

public class ControlsPageButton : MonoBehaviour
{
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private float interactSoundVolume;
    private Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            ReturnToMainMenu();
    }

    private void ReturnToMainMenu()
    {
        SoundManager.instance.PlaySound(interactSound, interactSoundVolume);
        button.onClick.Invoke();
    }
}
