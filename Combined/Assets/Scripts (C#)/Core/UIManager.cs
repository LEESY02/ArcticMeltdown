using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private float volume;

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen;

    private Tracker tracker;

    private void Awake() 
    {
        gameOverScreen.SetActive(false); //deactivate the gameOverScreen maually
        pauseScreen.SetActive(false); //deactivate the pauseScreen maually
        tracker = FindObjectOfType<Tracker>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if pause screen is already active unpause and viceversa
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }
    }

    //activate game over screen
    #region  Game Over
    public void GameOver() 
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound, volume);
    }

    //game over functions
    public void Restart()
    {
        // unpause if needed
        if (Time.timeScale == 0) 
        {
            Time.timeScale = 1; //time continues at 1x speed
            if (FindFirstObjectByType<Player>() != null)
                FindFirstObjectByType<Player>().enabled = true;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0) // if at main menu
        {
            tracker.mostRecentHealth = tracker.playerStartingHealth;
            tracker.coinCount = 0;
            SceneManager.LoadScene(2); // load level 1
        }
        else
        {
            // Reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void MainMenu()
    {
        // SceneManager.UnloadSceneAsync(1);
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit(); //Quits the game (only works on build)

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //exits play mode in Unity (will only be executed in the editor)
        #endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        //if status == true pause | if status == false unpause
        pauseScreen.SetActive(status);

        if (status)
        {
            Time.timeScale = 0; //pauses the time
            FindFirstObjectByType<Player>().enabled = false;
        }
        else
        {
            Time.timeScale = 1; //time continues at 1x speed
            FindFirstObjectByType<Player>().enabled = true;
        }
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(0.2f); //increase sound volume by 20%
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(0.2f); //increase music volume by 20%
    }
    #endregion

    #region Story
        public void Story()
        {
            SceneManager.LoadScene(1);
        }
    #endregion
}
